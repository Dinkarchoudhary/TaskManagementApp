using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;
using System;
using System.Linq;
using TaskManagementApp.Data;
using System.Xml.Linq;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace TaskManagementApp.Tests.Repositories
{
    [TestFixture]
    public class TaskRepositoryTests
    {
        private TaskContext _context;
        private TaskRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<TaskContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;

            _context = new TaskContext(options);
            _repository = new TaskRepository(_context);

            // Seed initial data
            //_context.Columns.Add(new Column { ColumnId = 1, Name = "ToDo" });
            _context.Columns.Add(new Column { ColumnId = 1, Name = "ToDo", Tasks = new List<Models.Task>() });
            _context.SaveChanges();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void AddTask_ShouldAddTaskToDatabase()
        {
            var task = new Models.Task {
                    Name= "Test Task",
                    Description= "Initialize the Git repository for project tracking.",
                    Deadline= DateTime.Now,
                    ColumnId= 1,
                    IsFavorited= false,
                    ImagePath = "/desktop/SetupImg.png" 
            };
            _repository.AddTask(task);
            var taskInDb = _context.Tasks.FirstOrDefault(t => t.Name == "Test Task");
            Assert.That(taskInDb, Is.Not.Null);
        }

        [Test]
        public void UpdateTask_ShouldModifyTask()
        {
            var task = new Models.Task {
                Name = "Test Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();

            task.Name = "Updated Task";
            _repository.UpdateTask(task);

            var updatedTask = _context.Tasks.FirstOrDefault(t => t.Name == "Updated Task");
            Assert.That(updatedTask, Is.Not.Null);
        }

        [Test]
        public void DeleteTask_ShouldRemoveTaskFromDatabase()
        {
            var task = new Models.Task {
                Name = "Test Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();

            _repository.DeleteTask(task.TaskId);

            var deletedTask = _context.Tasks.FirstOrDefault(t => t.TaskId == task.TaskId);
            Assert.That(deletedTask, Is.Null);
        }

        [Test]
        public void GetAllTasks_ShouldReturnTasksOrderedByColumnIdAndIsFavoritedAndName()
        {
            var task1 = new Models.Task {
                Name = "Task B",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            var task2 = new Models.Task {
                Name = "Task A",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = true,
                ImagePath = "/desktop/SetupImg.png"
            };
            var task3 = new Models.Task {
                Name = "Task C",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 2,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _context.Tasks.AddRange(task1, task2, task3);
            _context.SaveChanges();

            var tasks = _repository.GetAllTasks().ToList();

            Assert.That(tasks[0], Is.EqualTo(task2));
            Assert.That(tasks[1], Is.EqualTo(task1));
            Assert.That(tasks[2], Is.EqualTo(task3));
        }

        [Test]
        public void GetTaskById_ShouldReturnCorrectTask()
        {
            var task = new Models.Task {
                Name = "Find Me",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();

            var retrievedTask = _repository.GetTaskById(task.TaskId);

            Assert.That(retrievedTask, Is.Not.Null);
            Assert.That(retrievedTask.Name, Is.EqualTo("Find Me"));
        }

        [Test]
        public void GetTaskDetailsById_ShouldReturnTaskWithColumnDetails()
        {
            //var column = new Column { ColumnId = 1, Name = "In Progress", Tasks = new List<Models.Task>() };
            var task = new Models.Task {
                Name = "Detailed Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            //_context.Columns.Add(column);
            _context.Tasks.Add(task);
            _context.SaveChanges();

            var taskDetails = _repository.GetTaskDetailsById(task.TaskId);

            Assert.That(taskDetails, Is.Not.Null);
            Assert.That(taskDetails.ColumnName, Is.EqualTo("ToDo"));
        }

        [Test]
        public void MoveTaskToColumn_ShouldUpdateTaskColumnId()
        {
            var task = new Models.Task {
                Name = "Movable Task",
                Description = "Initialize the Git repository for project tracking.",
                Deadline = DateTime.Now,
                ColumnId = 1,
                IsFavorited = false,
                ImagePath = "/desktop/SetupImg.png"
            };
            _context.Tasks.Add(task);
            _context.SaveChanges();

            _repository.MoveTaskToColumn(task.TaskId, 2);
            _context.SaveChanges();

            var updatedTask = _context.Tasks.Find(task.TaskId);
            Assert.That(updatedTask.ColumnId, Is.EqualTo(2));
        }

        [Test]
        public void GetAllColumns_ShouldReturnAllColumns()
        {
            //_context.Columns.Add(new Column { ColumnId = 1, Name = "Column1", Tasks = new List<Models.Task>() });
            //_context.SaveChanges();

            var columns = _repository.GetAllColumns();
            Assert.That(columns.Count(), Is.EqualTo(1));
        }

        [Test]
        public void AddColumn_ShouldAddColumnToDatabase()
        {
            var column = new Column { Name = "New Column" };
            _repository.AddColumn(column);

            var columnInDb = _context.Columns.FirstOrDefault(c => c.Name == "New Column");
            Assert.That(columnInDb, Is.Not.Null);
        }
    }
}
