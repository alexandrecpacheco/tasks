using System.Data.Common;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Entities;

namespace Tasks.Domain.Interfaces.Data.Repository
{
    public interface ITaskRepository
    {
        Task<TaskEntity> GetTaskAsync(int taskId);
        Task<IEnumerable<TaskEntity>> GetAllTasksAsync();
        Task<TaskEntity> CreateAsync(TaskRequest request, DbConnection dbConnection, DbTransaction dbTransaction);
    }
}
