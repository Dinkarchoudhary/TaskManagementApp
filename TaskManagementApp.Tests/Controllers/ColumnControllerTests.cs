using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Controllers;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Tests.Controllers
{
    [TestFixture]
    public class ColumnControllerTests
    {
        private ColumnController _columnController;
        private Mock<ITaskRepository> _mockTaskRepository;

        [SetUp]
        public void Setup()
        {
            _mockTaskRepository = new Mock<ITaskRepository>();
            _columnController = new ColumnController(_mockTaskRepository.Object);
        }

        [Test]
        public void GetColumns_ReturnsOkResult_WithListOfColumns()
        {
            // Arrange
            var columns = new List<Column>
            {
                new Column { ColumnId = 1, Name = "To Do" },
                new Column { ColumnId = 2, Name = "In Progress" }
            };
            _mockTaskRepository.Setup(repo => repo.GetAllColumns()).Returns(columns);

            // Act
            var result = _columnController.GetColumns();

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.That(okResult, Is.Not.Null);
            Assert.That(okResult.StatusCode, Is.EqualTo(200));
            Assert.That(okResult.Value, Is.EqualTo(columns));
        }

        [Test]
        public void CreateColumn_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newColumn = new Column { ColumnId = 3, Name = "Done" };

            // Act
            var result = _columnController.CreateColumn(newColumn);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.That(createdResult, Is.Not.Null);
            Assert.That(createdResult.StatusCode, Is.EqualTo(201));
            Assert.That(createdResult.Value, Is.EqualTo(newColumn));
        }
    }
}

