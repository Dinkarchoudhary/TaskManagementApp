using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TaskController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Models.Task>> GetTasks()
        {
            return Ok(_taskRepository.GetAllTasks());
        }

        [HttpGet("{id}")]
        public ActionResult<Models.Task> GetTask(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpGet("details/{id}")]
        public ActionResult<Models.Task> GetTaskDetails(int id)
        {
            var task = _taskRepository.GetTaskDetailsById(id);
            if (task == null) return NotFound();
            return Ok(task);
        }

        [HttpPost]
        public ActionResult CreateTask([FromBody] Models.Task task)
        {
            _taskRepository.AddTask(task);
            return CreatedAtAction(nameof(GetTask), new { id = task.TaskId }, task);
        }

        [HttpPut("{id}")]
        public ActionResult UpdateTask(int id, [FromBody] Models.Task updatedTask)
        {
            var existingTask = _taskRepository.GetTaskById(id);
            if (existingTask == null) return NotFound();

            existingTask.Name = updatedTask.Name;
            existingTask.Description = updatedTask.Description;
            existingTask.Deadline = updatedTask.Deadline;
            existingTask.IsFavorited = updatedTask.IsFavorited;
            existingTask.ColumnId = updatedTask.ColumnId;

            _taskRepository.UpdateTask(existingTask);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTask(int id)
        {
            var task = _taskRepository.GetTaskById(id);
            if (task == null) return NotFound();

            _taskRepository.DeleteTask(id);
            return NoContent();
        }

        [HttpPut("move/{taskId}")]
        public ActionResult MoveTaskToColumn(int taskId, [FromBody] int columnId)
        {
            var existingTask = _taskRepository.GetTaskById(taskId);
            if (existingTask == null) return NotFound();

            _taskRepository.MoveTaskToColumn(taskId, columnId);
            return NoContent();
        }
    }
}
