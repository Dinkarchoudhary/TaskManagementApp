using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TaskManagementApp.Controllers;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Tests.Controllers
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITaskRepository> _mockRepository;
        private TaskController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<ITaskRepository>();
            _controller = new TaskController(_mockRepository.Object);
        }

        [Test]
        public void GetTasks_ReturnsOkResult_WithTasks()
        {
            // Arrange
            var tasks = new List<Models.Task>
            {
                new Models.Task { TaskId = 1,
                    Name = "Task 1",
                    Description = "Initialize the Git repository for project tracking.",
                    Deadline = DateTime.Now,
                    ColumnId = 1,
                    IsFavorited = false,
                    ImagePath = "/desktop/SetupImg.png" },
                new Models.Task { TaskId = 2,
                    Name = "Task 2",
                    Description = "Initialize the Git repository for project tracking.",
                    Deadline = DateTime.Now,
                    ColumnId = 1,
                    IsFavorited = false,
                    ImagePath = "/desktop/SetupImg.png" }
            };
            _mockRepository.Setup(repo => repo.GetAllTasks()).Returns(tasks);

            // Act
            var result = _controller.GetTasks();

            // Assert
            Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());

            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(tasks));
        }

        [Test]
        public void GetTask_ExistingId_ReturnsOkResult_WithTask()
        {
            // Arrange
            var task = new Models.Task {
                TaskId = 1,
                Name = "Task 1",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns(task);

            // Act
            var result = _controller.GetTask(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(task));
        }

        [Test]
        public void GetTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns((Models.Task)null);

            // Act
            var result = _controller.GetTask(1);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void GetTaskDetails_ExistingId_ReturnsOkResult_WithTaskDetails()
        {
            // Arrange
            var task = new TaskColumnDTO {
                TaskId = 1,
                Name = "Task 1",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png",
                ColumnName = "In Progress"
            };
            _mockRepository.Setup(repo => repo.GetTaskDetailsById(1)).Returns(task);

            // Act
            var result = _controller.GetTaskDetails(1);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(task));
        }

        [Test]
        public void GetTaskDetails_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTaskDetailsById(1)).Returns((Models.TaskColumnDTO)null);

            // Act
            var result = _controller.GetTaskDetails(1);

            // Assert
            Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void CreateTask_ValidTask_ReturnsCreatedAtAction()
        {
            // Arrange
            var newTask = new Models.Task {
                TaskId = 1,
                Name = "New Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _mockRepository.Setup(repo => repo.AddTask(newTask)).Verifiable();

            // Act
            var result = _controller.CreateTask(newTask);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.Value, Is.EqualTo(newTask));
            _mockRepository.Verify(repo => repo.AddTask(newTask), Times.Once);
        }

        [Test]
        public void UpdateTask_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new Models.Task { 
                TaskId = 1,
                Name = "Existing Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            var updatedTask = new Models.Task {
                Name = "Updated Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns(existingTask);
            _mockRepository.Setup(repo => repo.UpdateTask(existingTask)).Verifiable();

            // Act
            var result = _controller.UpdateTask(1, updatedTask);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mockRepository.Verify(repo => repo.UpdateTask(existingTask), Times.Once);
        }

        [Test]
        public void UpdateTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var updatedTask = new Models.Task {
                Name = "Updated Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns((Models.Task)null);

            // Act
            var result = _controller.UpdateTask(1, updatedTask);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void DeleteTask_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new Models.Task {
                TaskId = 1,
                Name = "Task to Delete",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns(existingTask);
            _mockRepository.Setup(repo => repo.DeleteTask(1)).Verifiable();

            // Act
            var result = _controller.DeleteTask(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mockRepository.Verify(repo => repo.DeleteTask(1), Times.Once);
        }

        [Test]
        public void DeleteTask_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns((Models.Task)null);

            // Act
            var result = _controller.DeleteTask(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }

        [Test]
        public void MoveTaskToColumn_ExistingTaskId_ReturnsNoContent()
        {
            // Arrange
            var existingTask = new Models.Task {
                TaskId = 1,
                Name = "Task to Move",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            int targetColumnId = 2;
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns(existingTask);
            _mockRepository.Setup(repo => repo.MoveTaskToColumn(1, targetColumnId)).Verifiable();

            // Act
            var result = _controller.MoveTaskToColumn(1, targetColumnId);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
            _mockRepository.Verify(repo => repo.MoveTaskToColumn(1, targetColumnId), Times.Once);
        }

        [Test]
        public void MoveTaskToColumn_NonExistingTaskId_ReturnsNotFound()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTaskById(1)).Returns((Models.Task)null);

            // Act
            var result = _controller.MoveTaskToColumn(1, 2);

            // Assert
            Assert.That(result, Is.InstanceOf<NotFoundResult>());
        }
    }
}
