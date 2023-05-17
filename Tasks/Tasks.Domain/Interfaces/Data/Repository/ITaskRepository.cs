using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Entities;

namespace Tasks.Domain.Interfaces.Data.Repository
{
    public interface ITaskRepository
    {
        Task<TaskEntity> GetTaskAsync(int taskId);
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity> CreateAsync(TaskRequest request, DbConnection dbConnection, DbTransaction dbTransaction);
        Task<TaskEntity> UpdateAsync(TaskRequest request, DbConnection dbConnection, DbTransaction dbTransaction);
        Task DeleteAsync(int id, DbConnection dbConnection, DbTransaction dbTransaction);
    }
}
