using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Threading.Tasks;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.Interfaces.Data.Service;

namespace Tasks.Controllers
{
    [Route("api/task")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService, ILogger<TaskController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        /// <summary>
        /// Get a task from database
        /// </summary>
        /// <param name="taskId">Id</param>
        /// <returns>Returns a Task from database</returns>
        /// <response code="204">Returns no data</response>
        /// <response code="200">Returns a task from database</response>
        /// <response code="400">Error bad request</response>
        [HttpGet("{taskId}")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Response of a specific task by Id")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Does not exists task with this Id")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request failed trying to get information")]
        public async Task<IActionResult> Get([Required] int taskId)
        {
            _logger.LogInformation("Starting Get Task");
            var result = await _taskService.GetTaskAsync(taskId);

            if (result is null) return NoContent();

            _logger.LogInformation($"The task {taskId} has been finished!");
            return Ok(result);
        }

        /// <summary>
        /// Get a list of tasks
        /// </summary>
        /// <returns>Returns a collection of TaskResponse</returns>
        /// <response code="204">Returns no data</response>
        /// <response code="200">Returns a list of tasks</response>
        /// <response code="400">Error of bad request</response>
        [HttpGet("get-all-tasks")]
        [SwaggerResponse((int)HttpStatusCode.OK, "Response of all tasks")]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "Does not exists any task")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request failed trying to get information")]
        public async Task<IActionResult> GetAll()
        {
            _logger.LogInformation("Starting Get All Tasks");
            var result = await _taskService.GetAllTasksAsync();

            if (result is null) return NoContent();

            _logger.LogInformation($"The tasks has been retrieved!");
            return Ok(result);
        }

        /// <summary>
        /// Create a new task
        /// </summary>
        /// <param name="task">tasks parameters</param>
        /// <returns>Returns the API has been created with id</returns>
        [HttpPost]
        [SwaggerResponse((int)HttpStatusCode.Created, "The task has been created")]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, "Request failed trying to get information")]

        public async Task<IActionResult> Create([Required][FromBody] TaskRequest task)
        {
            _logger.LogInformation("Creating task");
            if (task is null) return BadRequest();

            var result = await _taskService.CreateAsync(task);

            if (result.Description is null)
                return BadRequest("Error trying to create a task");

            _logger.LogInformation($"Task has been created, description: {result.Description}");
            return Created("/", result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([Required] [FromBody] TaskRequest task)
        {
            _logger.LogInformation("Updating task");

            var result = await _taskService.UpdateAsync(task);

            if (result.Description is null)
                return BadRequest("Error trying to update a task");

            _logger.LogInformation($"Task has been created, description: {result.Description}");
            return Created("/", result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Deleting a task id: {id}");

            await _taskService.DeleteAsync(id);

            return Ok();
        }
    }
}
