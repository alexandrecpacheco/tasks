﻿using Dapper;
using System.Data.Common;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Entities;
using Tasks.Domain.Interfaces.Data;
using Tasks.Domain.Interfaces.Data.Repository;

namespace Tasks.Infraestructure.Data
{
    public class TaskRepository : ITaskRepository
    {
        private readonly IDatabase _database;

        public TaskRepository(IDatabase database)
        {
            _database = database;
        }

        public async Task<TaskEntity> GetTaskAsync(int taskId)
        {
            await using var conn = await _database.CreateAndOpenConnection();

            string query = @"
                    SELECT t.id, t.description, date, status
                    FROM [task] t
                    WHERE id = @taskId
                    ";

            var result = await conn.QueryAsync<TaskEntity>(query, new { taskId });

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<TaskEntity>> GetAllTasksAsync()
        {
            await using var conn = await _database.CreateAndOpenConnection();

            string query = @"
                    SELECT t.id, t.description, date, status
                    FROM [task] t
                    ";

            var result = await conn.QueryAsync<TaskEntity>(query);

            return result;
        }

        public async Task<TaskEntity> CreateAsync(TaskRequest request, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            const string query = @"
                    INSERT INTO TASK (description, date, status, created_at)
                    VALUES (@Description, GETDATE(), @Status, GETDATE());

                    SELECT t.id, t.description, t.date, t.status
                    FROM task t
                    WHERE t.id = SCOPE_IDENTITY();
            ";

            return await dbConnection.QueryFirstAsync<TaskEntity>(query, request, dbTransaction);
        }

        public async Task<TaskEntity> UpdateAsync(TaskRequest request, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            const string query = @"
                    UPDATE task
                    SET Description = @Description,
                    Status = @Status,
                    Date = @Date
                    WHERE Id = @Id;
                    
                    SELECT t.id, t.description, t.date, t.status
                    FROM task t
                    WHERE t.id = @Id;
            ";

            var result = await dbConnection.QueryAsync<TaskEntity>(query, request, dbTransaction);

            return result.FirstOrDefault();
        }

        public async Task DeleteAsync(int id, DbConnection dbConnection, DbTransaction dbTransaction)
        {
            const string query = @"
                    DELETE
                    FROM task
                    WHERE id = @id;
            ";

            await dbConnection.ExecuteAsync(query, new { id }, dbTransaction);
        }
    }
}
