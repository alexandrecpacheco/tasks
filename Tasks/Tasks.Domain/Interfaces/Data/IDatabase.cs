﻿using System.Data.Common;

namespace Tasks.Domain.Interfaces.Data
{
    public interface IDatabase
    {
        Task<DbConnection> CreateAndOpenConnection(CancellationToken stoppingToken = default);
        Task ExecuteInTransaction(Func<DbConnection, DbTransaction, Task> action,
            CancellationToken cancellationToken = default);
        void UpgradeIfNecessary();
    }
}
