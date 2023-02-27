using Tasks.Domain.Interfaces.Data;
using Tasks.Domain.Interfaces.Data.Repository;
using Tasks.Domain.Interfaces.Data.Service;

namespace Tasks.Services
{
    public class TaskService : ITaskService
    {
        private readonly IDatabase _database;
        private readonly ITaskRepository _taskRepository;

        public TaskService(IDatabase database, ITaskRepository taskRepository)
        {
            _database = database;
            _taskRepository = taskRepository;
        }

    }
}
