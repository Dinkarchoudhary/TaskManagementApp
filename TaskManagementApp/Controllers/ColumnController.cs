using Microsoft.AspNetCore.Mvc;
using TaskManagementApp.Models;
using TaskManagementApp.Repositories;

namespace TaskManagementApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColumnController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public ColumnController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Column>> GetColumns()
        {
            return Ok(_taskRepository.GetAllColumns());
        }

        [HttpPost]
        public ActionResult CreateColumn([FromBody] Column column)
        {
            _taskRepository.AddColumn(column);
            return CreatedAtAction(nameof(GetColumns), column);
        }
    }
}
