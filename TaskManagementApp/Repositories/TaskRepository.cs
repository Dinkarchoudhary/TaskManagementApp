using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TaskManagementApp.Data;
using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskContext _context;

        public TaskRepository(TaskContext context)
        {
            _context = context;
        }

        //public IEnumerable<Models.Task> GetAllTasks() => _context.Tasks.ToList();
        public IEnumerable<Models.Task> GetAllTasks()
        {
            return _context.Tasks
                .OrderBy(t => t.ColumnId)
                .ThenByDescending(t => t.IsFavorited)
                .ThenBy(t => t.Name);
        }

        public Models.Task GetTaskById(int id) => _context.Tasks.Find(id);

        public TaskColumnDTO GetTaskDetailsById(int id)
        {
            var result = (from task in _context.Tasks
                          join column in _context.Columns on task.ColumnId equals column.ColumnId
                          where task.TaskId == id
                          select new TaskColumnDTO
                          {
                              TaskId = task.TaskId,
                              Name = task.Name,
                              Description = task.Description,
                              Deadline = task.Deadline,
                              IsFavorited = task.IsFavorited,
                              ImagePath = task.ImagePath,
                              ColumnId = task.ColumnId,
                              ColumnName = column.Name
                          }).FirstOrDefault();
            return result;
        }

        public void AddTask(Models.Task task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();
        }

        public void UpdateTask(Models.Task task)
        {
            _context.Tasks.Update(task);
            _context.SaveChanges();
        }

        public void DeleteTask(int id)
        {
            var task = _context.Tasks.Find(id);
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Column> GetAllColumns() => _context.Columns.Include(i=>i.Tasks).ToList();

        public void AddColumn(Column column)
        {
            _context.Columns.Add(column);
            _context.SaveChanges();
        }

        public void MoveTaskToColumn(int taskId, int columnId)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == taskId);
            if (task != null)
            {
                task.ColumnId = columnId;
            }
        }
    }
}
