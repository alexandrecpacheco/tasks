using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Threading.Tasks;
using Tasks.Controllers;
using Tasks.Domain.AutoMapper;
using Tasks.Domain.DTO.Response;
using Tasks.Domain.Entities;
using Tasks.Domain.Interfaces.Data.Service;

namespace Tasks.Tests.Controllers
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TasksControlerTests
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
            Assert.IsTrue(expected?.StatusCode.Equals(HttpStatusCode.OK));
            _mockTaskService.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }

        [Test]
        public async Task TaskController_TryingToGetTaskAsync_Retrieve_Success_With_NoContent()
        {
            //Arrange
            int taskId = 1;

            var taskEntity = new TaskEntity()
            {
                Id = 1,
                Description = "Test 1",
                Date = DateTime.Today,
                Status = true,
                CreatedAt = DateTime.Today
            };

            _mockTaskService.Setup(s => s.GetTaskAsync(taskId)).ReturnsAsync((TaskResponse)null!);

            var controller = new TaskController(_mockTaskService.Object, _mockLogger.Object);

            //Act
            var expected = await controller.Get(taskId) as NoContentResult;

            //Assert
            Assert.NotNull(expected);
            Assert.AreEqual(expected.StatusCode, (int)HttpStatusCode.NoContent);
            _mockTaskService.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }
    }
}
