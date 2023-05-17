using AutoMapper;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Tasks.Domain.AutoMapper;
using Tasks.Domain.Entities;
using Tasks.Domain.Interfaces.Data;
using Tasks.Domain.Interfaces.Data.Repository;
using Tasks.Domain.Interfaces.Data.Service;
using Tasks.Domain.Interfaces.Messaging;
using Tasks.Services;

namespace Tasks.Tests
{
    [ExcludeFromCodeCoverage]
    [TestFixture]
    public class TaskTests
    {
        private Mock<ITaskService> _mockTaskService;
        private Mock<ITaskRepository> _mockTaskRepository;
        private Mock<IDatabase> _mockIDatabase;
        private IMapper _mockMapper;
        private Mock<IBusPublisher> _mockBusPublisher;

        [SetUp]
        public void SetUp()
        {
            _mockTaskService = new Mock<ITaskService>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockIDatabase = new Mock<IDatabase>();
            _mockBusPublisher = new Mock<IBusPublisher>();

            MapperConfiguration mapperConfig = new MapperConfiguration(
        cfg =>
        {
            cfg.AddProfile(new DomainToResponseMappingProfile());
        });

            _mockMapper = new Mapper(mapperConfig);
        }


        [Test]
        public async System.Threading.Tasks.Task TaskService_TryingToGetTaskAsync_Retrieve_Success()
        {
            //Arrange
            int taskId = 1;

            var taskEntity = new TaskEntity()
            {
                Id = 1,
                Description = "Test 1",
                Date = System.DateTime.Today,
                Status = true,
                CreatedAt = System.DateTime.Today
            };

            _mockTaskRepository.Setup(c => c.GetTaskAsync(taskId)).Returns(System.Threading.Tasks.Task.FromResult(taskEntity));

            var taskService = new TaskService(_mockIDatabase.Object, _mockTaskRepository.Object, _mockMapper, _mockBusPublisher.Object);

            //Act
            var expected = await taskService.GetTaskAsync(taskId);

            //Assert
            Assert.NotNull(expected);
            _mockTaskRepository.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }

        [Test]
        public async System.Threading.Tasks.Task TaskService_TryingToGetTaskAsync_Retrieve_NullValue()
        {
            //Arrange
            int taskId = 1;

            _mockTaskRepository.Setup(c => c.GetTaskAsync(taskId)).Returns(System.Threading.Tasks.Task.FromResult(It.IsAny<TaskEntity>()));

            var taskService = new TaskService(_mockIDatabase.Object, _mockTaskRepository.Object, _mockMapper, _mockBusPublisher.Object);

            //Act
            var expected = await taskService.GetTaskAsync(taskId);

            //Assert
            Assert.Null(expected);
            _mockTaskRepository.Verify(x => x.GetTaskAsync(taskId), Times.Once);
        }

        [Test]
        public async System.Threading.Tasks.Task TaskService_TryingToGetAllTaskAsync_Retrieve_Success()
        {
            //Arrange
            IEnumerable<TaskEntity> listTaskEntity = new List<TaskEntity>()
            {
                new TaskEntity
                {
                    Id = 1,
                    Date = DateTime.Today,
                    Status = true,
                    Description = "Test 1"
                },
                new TaskEntity
                {
                    Id = 2,
                    Date = DateTime.Today.AddDays(1),
                    Status = true,
                    Description= "Test2"
                }
            };
                
            _mockTaskRepository.Setup(c => c.GetAllTasksAsync()).Returns(System.Threading.Tasks.Task.FromResult(listTaskEntity));

            var taskService = new TaskService(_mockIDatabase.Object, _mockTaskRepository.Object, _mockMapper, _mockBusPublisher.Object);

            //Act
            var expected = await taskService.GetAllTasksAsync();

            //Assert
            Assert.NotNull(expected);
            _mockTaskRepository.Verify(x => x.GetAllTasksAsync(), Times.Once);
        }

        [Test]
        public async System.Threading.Tasks.Task TaskService_TryingToGetAllTaskAsync_Retrieve_NullValue()
        {
            //Arrange
            _mockTaskRepository.Setup(c => c.GetAllTasksAsync()).Returns(System.Threading.Tasks.Task.FromResult(It.IsAny<IEnumerable<TaskEntity>>()));

            var taskService = new TaskService(_mockIDatabase.Object, _mockTaskRepository.Object, _mockMapper, _mockBusPublisher.Object);

            //Act
            var expected = await taskService.GetAllTasksAsync();

            //Assert
            Assert.Null(expected);
            _mockTaskRepository.Verify(x => x.GetAllTasksAsync(), Times.Once);
        }
    }
}
