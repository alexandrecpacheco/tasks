using Dapper;
using DbUp;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Serilog;
using System.Data.Common;
using System.Reflection;
using Tasks.Domain.Interfaces.Data;

namespace Tasks.Infraestructure
{
    public class Database : IDatabase
    {
        private readonly IConfiguration _configuration;

        public Database(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<DbConnection> CreateAndOpenConnection(CancellationToken stoppingToken = default)
        {
            var connection = new SqlConnection(GetConnectionString());
            DefaultTypeMap.MatchNamesWithUnderscores = true;
            await connection.OpenAsync(stoppingToken);

            return connection;
        }

        public async Task ExecuteInTransaction(Func<DbConnection, DbTransaction, Task> action, CancellationToken cancellationToken = default)
        {
            await using var conn = await CreateAndOpenConnection(cancellationToken);
            await using var transaction = await conn.BeginTransactionAsync(cancellationToken);
            try
            {
                await action(conn, transaction);
                await transaction.CommitAsync(cancellationToken);
            }
            catch (TaskCanceledException ex)
            {
                Log.Error(ex, "Task canceled transaction - rolling back");
                await transaction.RollbackAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Exception while executing transaction - rolling back");
                try
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
                catch (Exception ex2)
                {
                    Log.Error(ex2, "Error rolling back transaction");
                }

                throw;
            }
        }

        public void UpgradeIfNecessary()
        {
            Log.Information("Upgrading Database");
            EnsureDatabase.For.SqlDatabase(GetConnectionString());
            var upgradeDatabase = DeployChanges.To
                .SqlDatabase(GetConnectionString())
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToAutodetectedLog()
                .Build();

            var result = upgradeDatabase.PerformUpgrade();
            if (result.Successful == false)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(result.Error);
                Console.ResetColor();
                Log.Error("Failed to upgrade the database - {Error}", result.Error.Message);
#if DEBUG
                Console.ReadLine();
#endif
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Success!");
            Console.ResetColor();
        }
        private string GetConnectionString()
        {
            var connectionString = _configuration.GetSection("ConnectionStrings:DefaultConnection").Value;
            return connectionString is null ? string.Empty : connectionString;
        }
    }
}
