using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Tasks.Controllers;
using Tasks.Domain.AutoMapper;
using Tasks.Domain.DTO.Request;
using Tasks.Domain.DTO.Response;
using Tasks.Domain.Entities;
using Tasks.Domain.Interfaces.Data.Service;

namespace Tasks.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TasksControllerTests
    {
        private Mock<ITaskService> _mockTaskService;
        private Mock<ILogger<TaskController>> _mockLogger;
        private IMapper _mockMapper;

        [SetUp]
        public void SetUp()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockLogger = new Mock<ILogger<TaskController>>();

            MapperConfiguration mapperConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.AddProfile(new DomainToResponseMappingProfile());
        });

            _mockMapper = new Mapper(mapperConfig);
        }

        [Test]
        public async Task TaskController_TryingToGetTaskAsync_Retrieve_Success_With_Ok()
        {
            //Arrange
            int taskId = 1;

            var taskResponse = new TaskResponse()
            {
                Id = 1,
                Description = "Test 1",
                Date = DateTime.Today,
                Status = true,
            };

            _mockTaskService.Setup(s => s.GetTaskAsync(taskId)).ReturnsAsync(taskResponse);
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Get(taskId) as OkObjectResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.OK);
            _mockTaskService.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }

        [Test]
        public async Task TaskController_TryingToGetTaskAsync_Retrieve_Success_With_NoContent()
        {
            //Arrange
            int taskId = 1;

            _mockTaskService.Setup(s => s.GetTaskAsync(taskId)).ReturnsAsync((TaskResponse)null!);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Get(taskId) as NoContentResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.NoContent);
            _mockTaskService.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }

        [Test]
        public async Task TaskController_GetAllTasksAsync_Retrieve_Success_With_OkResult()
        {
            //Arrange
            IList<TaskResponse> listResponse = new List<TaskResponse>()
            {
                new TaskResponse()
                {
                    Date = DateTime.Today,
                    Description = "Test 1",
                    Status = true,
                    Id = 1
                }
            };

            _mockTaskService.Setup(s => s.GetAllTasksAsync()).ReturnsAsync(listResponse);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.GetAll() as OkObjectResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.OK);
            _mockTaskService.Verify(x => x.GetAllTasksAsync(), Times.Once);
        }

        [Test]
        public async Task TaskController_GetAllTasksAsync_Retrieve_Success_With_NoContent()
        {
            //Arrange
            _mockTaskService.Setup(s => s.GetAllTasksAsync()).ReturnsAsync((IList<TaskResponse>)null!);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.GetAll() as NoContentResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.NoContent);
            _mockTaskService.Verify(x => x.GetAllTasksAsync(), Times.Once);
        }

        [Test]
        public async Task TaskController_CreateAsync_Retrieve_Success_With_OkResult()
        {
            //Arrange
            var taskRequest = new TaskRequest()
            {
                Date = DateTime.Now,
                Description = "Test",
                Status = true
            };
            var taskResponse = new TaskResponse()
            {
                Date = DateTime.Today,
                Description = "Test 1",
                Status = true,
                Id = 1
            };

            _mockTaskService.Setup(s => s.CreateAsync(taskRequest)).ReturnsAsync(taskResponse);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Create(taskRequest) as CreatedResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.Created);
            _mockTaskService.Verify(x => x.CreateAsync(It.IsAny<TaskRequest>()), Times.Once);
        }

        [Test]
        public async Task TaskController_CreateAsync_With_Invalid_Parameters_Retrieve_UnSuccess_Returning_BadRequest()
        {
            //Arrange
            _mockTaskService.Setup(s => s.CreateAsync(It.IsAny<TaskRequest>())).ReturnsAsync((TaskResponse)null!);
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Create(It.IsAny<TaskRequest>()) as BadRequestResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.BadRequest);
            _mockTaskService.Verify(x => x.CreateAsync(It.IsAny<TaskRequest>()), Times.Never);
        }

        [Test]
        public async Task TaskController_CreateAsync_With_Valid_Parameters_Returning_BadRequest()
        {
            //Arrange
            var taskRequest = new TaskRequest()
            {
                Date = DateTime.Now,
                Description = "Test",
                Status = true
            };

            _mockTaskService.Setup(s => s.CreateAsync(taskRequest)).ReturnsAsync((TaskResponse)null!);
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Create(taskRequest) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual("Error trying to create a task", expected?.Value);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.BadRequest);
            _mockTaskService.Verify(x => x.CreateAsync(It.IsAny<TaskRequest>()), Times.Once);
        }

        [Test]
        public async Task TaskController_UpdateAsync_Retrieve_Success_With_OkResult()
        {
            //Arrange
            var taskRequest = new TaskRequest()
            {
                Date = DateTime.Now,
                Description = "Test",
                Status = true
            };
            var taskResponse = new TaskResponse()
            {
                Date = DateTime.Today,
                Description = "Test 1",
                Status = true,
                Id = 1
            };

            _mockTaskService.Setup(s => s.UpdateAsync(taskRequest)).ReturnsAsync(taskResponse);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Update(taskRequest) as CreatedResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.Created);
            _mockTaskService.Verify(x => x.UpdateAsync(It.IsAny<TaskRequest>()), Times.Once);
        }

        [Test]
        public async Task TaskController_UpdateAsync_With_Invalid_Parameters_Retrieve_UnSuccess_Returning_BadRequest()
        {
            //Arrange
            _mockTaskService.Setup(s => s.UpdateAsync(It.IsAny<TaskRequest>())).ReturnsAsync((TaskResponse)null!);
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Update(It.IsAny<TaskRequest>()) as BadRequestResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.BadRequest);
            _mockTaskService.Verify(x => x.UpdateAsync(It.IsAny<TaskRequest>()), Times.Never);
        }

        [Test]
        public async Task TaskController_UpdateAsync_With_Valid_Parameters_Retrieve_UnSuccess_Returning_BadRequest()
        {
            //Arrange
            var taskRequest = new TaskRequest()
            {
                Date = DateTime.Now,
                Description = "Test",
                Status = true
            };
            
            _mockTaskService.Setup(s => s.UpdateAsync(taskRequest)).ReturnsAsync((TaskResponse)null!);
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Update(taskRequest) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.BadRequest);
            _mockTaskService.Verify(x => x.UpdateAsync(It.IsAny<TaskRequest>()), Times.Once);
        }

        [Test]
        public async Task TaskController_DeleteAsync_Retrieve_Success_With_OkResult()
        {
            //Arrange
            var id = 1;

            _mockTaskService.Setup(s => s.DeleteAsync(id));

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Delete(id) as OkResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.OK);
            _mockTaskService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once);
        }

        [TestCase(0)]
        [TestCase(-1)]
        public async Task TaskController_DeleteAsync_Retrieve_Success_With_OkResult(int id)
        {
            //Arrange
            _mockTaskService.Setup(s => s.DeleteAsync(id));
            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Delete(id) as BadRequestResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected?.StatusCode, (int)HttpStatusCode.BadRequest);
            _mockTaskService.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Never);
        }
    }
}
