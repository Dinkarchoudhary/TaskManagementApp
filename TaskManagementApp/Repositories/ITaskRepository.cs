using TaskManagementApp.Models;

namespace TaskManagementApp.Repositories
{
    public interface ITaskRepository
    {
        IEnumerable<Models.Task> GetAllTasks();
        Models.Task GetTaskById(int id);
        TaskColumnDTO GetTaskDetailsById(int id);
        void AddTask(Models.Task task);
        void UpdateTask(Models.Task task);
        void DeleteTask(int id);
        IEnumerable<Column> GetAllColumns();
        void AddColumn(Column column);
        void MoveTaskToColumn(int taskId, int columnId);
    }
}
