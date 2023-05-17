using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.DTO.Response;

namespace Tasks.Domain.Interfaces.Data.Service
{
    public interface ITaskService
    {
        Task<TaskResponse> GetTaskAsync(int taskId);
        Task<IEnumerable<TaskResponse>> GetAllTasksAsync();
        Task<TaskResponse> CreateAsync(TaskRequest request);
        Task<TaskResponse> UpdateAsync(TaskRequest request);
        Task DeleteAsync(int id);
    }
}
