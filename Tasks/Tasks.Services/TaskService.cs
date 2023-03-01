using AutoMapper;
using Serilog;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.DTO.Response;
using Tasks.Domain.Events;
using Tasks.Domain.Interfaces.Data;
using Tasks.Domain.Interfaces.Data.Repository;
using Tasks.Domain.Interfaces.Data.Service;
using Tasks.Domain.Messaging;

namespace Tasks.Services
{
    public class TaskService : ITaskService
    {
        private readonly IDatabase _database;
        private readonly ITaskRepository _taskRepository;
        private readonly IMapper _mapper;
        private readonly IBusPublisher _busPublisher;

        public TaskService(IDatabase database, ITaskRepository taskRepository, IMapper mapper, IBusPublisher busPublisher)
        {
            _database = database;
            _taskRepository = taskRepository;
            _mapper = mapper;
            _busPublisher = busPublisher;
        }

        public async Task<TaskResponse> GetTaskAsync(int taskId)
        {
            var repository = await _taskRepository.GetTaskAsync(taskId);
            if (repository is null)
            {
                Log.Error($"The Task {taskId} does not exists");
                return null;
            }

            var taskResponse = _mapper.Map<TaskResponse>(repository);

            return taskResponse;
        }

        public async Task<IEnumerable<TaskResponse>> GetAllTasksAsync()
        {
            var repository = await _taskRepository.GetAllTasksAsync();
            if (repository is null)
            {
                Log.Error($"The Tasks does not exists");
                return null;
            }

            var taskResponse = _mapper.Map<IEnumerable<TaskResponse>>(repository);

            return taskResponse;
        }

        public async Task<int> CreateAsync(TaskRequest task)
        {
            int id = 0;
            await _database.ExecuteInTransaction(async (connection, transaction) =>
            {
                var response = await _taskRepository.CreateAsync(task, connection, transaction);
                id = response.Id;
            });
            
            if (id >= 0)
            {
                await _busPublisher.PublishAsync(
                    new TaskCreatedEvent
                    {
                        Email = "fake@email.com",
                        Date = task.Date,
                        Description = $"Task has been created with Description: {task.Description}"
                    });
            }
            
            return id;
        }
    }
}
