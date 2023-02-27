using Tasks.Domain.DTO.Request;
using Tasks.Domain.DTO.Response;

namespace Tasks.Domain.Interfaces.Data.Service
{
    public interface ITaskService
    {
        Task<TaskResponse> GetTaskAsync(int taskId);
        Task<IEnumerable<TaskResponse>> GetAllTasksAsync();
        Task<int> CreateAsync(TaskRequest request);
    }
}
