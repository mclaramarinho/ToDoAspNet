using ToDoList.Data;
using ToDoList.Models;
namespace ToDoList.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem? GetTask(Guid id);

        Task<bool> CreateTask(TaskItem task, int uid);

        Task<bool> UpdateTask(Guid id, TaskItem task);
        Task<bool> DeleteTask(Guid id);
        bool TaskExists(Guid id);
        Task<bool> UpdateStatus(Guid id, string newStatus);
    }

    public class TaskService : ITaskService
    {
        private readonly ApplicationDbContext _db;

        public TaskService(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<TaskItem> GetAllTasks()
        {

            return _db.TaskItems.ToList();
        }

        public TaskItem? GetTask(Guid id)
        {
            if (!TaskExists(id))
            {
                throw new Exception("Task does not exist!");
            }
            return _db.TaskItems.FirstOrDefault(t => t.TaskID == id);
        }

        public Task<bool> CreateTask(TaskItem task, int uid)
        {
            var promise = new TaskCompletionSource<bool>();
            try {
                User user = _db.Users.First(u => u.UserID == uid);
                task.CreateTask(user);
                _db.TaskItems.Add(task);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            } catch (Exception ex) {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public Task<bool> UpdateTask(Guid id, TaskItem task)
        {
            var promise = new TaskCompletionSource<bool>();

            try
            {
                TaskItem t = _db.TaskItems.First(i => i.TaskID == id);
                t.TaskTitle = task.TaskTitle;
                t.TaskDescription = task.TaskDescription;

                _db.TaskItems.Update(t);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            }catch(Exception ex)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public Task<bool> UpdateStatus(Guid id, string newStatus)
        {
            var promise = new TaskCompletionSource<bool>();
            try
            {
                if(newStatus != "TD" && newStatus != "DO" && newStatus != "FI")
                {
                    throw new Exception("Invalid status");
                }
                if (!TaskExists(id))
                {
                    throw new Exception("Task not found");
                }

                TaskItem t = _db.TaskItems.FirstOrDefault(t => t.TaskID == id);

                switch (newStatus)
                {
                    case "TD":
                        t.ReopenTask();
                        break;
                    case "DO":
                        t.DoTask();
                        break;
                    case "FI":
                        t.EndTask();
                        break;
                }
                _db.TaskItems.Update(t);
                _db.SaveChanges();

                promise.SetResult(true);

                return promise.Task;

            }catch (Exception ex)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public Task<bool> DeleteTask(Guid id)
        {
            var promise = new TaskCompletionSource<bool>();
            try
            {
                if (!TaskExists(id))
                {
                    throw new Exception("Task does not exist!");
                }
                TaskItem t = _db.TaskItems.FirstOrDefault(t => t.TaskID == id);
                _db.TaskItems.Remove(t);
                _db.SaveChanges();
                promise.SetResult(true);
                return promise.Task;
            }catch(Exception ex)
            {
                promise.SetResult(false);
                return promise.Task;
            }
        }

        public bool TaskExists(Guid id)
        {
            return _db.TaskItems.Any(t => t.TaskID == id);
        }
    }
}
