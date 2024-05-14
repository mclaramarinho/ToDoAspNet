using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using ToDoList.Services;
namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _tasks;
        private readonly IUsersServices _users;
        private readonly IAccessControlService _auth;

        public TasksController(ITaskService tasks, IUsersServices users, IAccessControlService auth)
        {
            _tasks = tasks;
            _users = users;
            _auth = auth;
        }

        public IActionResult Index(string? status)
        {
            if (!_auth.VerifyIfUserLoggedIn())
            {
                return RedirectToAction("Login", "Users");
            }
            List<TaskItem> tasks = _tasks.GetAllTasks();

            tasks = tasks.FindAll(i => i.UserID == _auth.LoggedUser);
            if(status != null)
            {
                tasks = tasks.FindAll(i => i.Status == status);

                ViewBag.URLFilter = status;
            }

            return View(tasks);
        }

        public IActionResult TableView()
        {
            return View(_tasks.GetAllTasks());
        }

        [HttpPost, ActionName("Index")]
        public async Task<IActionResult> ChangeStatus(Guid taskid, string newStatus, string filter, string? redirectTo)
        {
            redirectTo ??= "Index";

            if (redirectTo != "TableView" && redirectTo != "Index")
            {
                return NotFound();
            }

            bool updated = await _tasks.UpdateStatus(taskid, newStatus);

            if (!updated)
            {
                return BadRequest();
            }
            return filter == null ? RedirectToAction(redirectTo) : RedirectToAction(redirectTo, routeValues: new { status = filter });
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("TaskTitle,TaskDescription")] TaskItem task)
        {
            if (!_auth.VerifyIfUserLoggedIn())
            {
                return RedirectToAction("Login", "Users");
            }
            bool created = await _tasks.CreateTask(task, (int)_auth.LoggedUser);

            TempData["ToasterType"] = !created ? "error" : "success";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            return _tasks.TaskExists(id) ? View(_tasks.GetTask(id)) : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Guid id, [Bind("TaskID, EndDate, StartDate, Status, UserID, TaskTitle,TaskDescription")] TaskItem task)
        {
            if (_tasks.TaskExists(id))
            {
                bool updated = await _tasks.UpdateTask(id, task);

                return updated ? RedirectToAction(nameof(Index)) : StatusCode(500);
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Delete(Guid? id)
        {

            if (id == null) { return RedirectToAction(nameof(Index)); }

            try
            {
                TaskItem t = _tasks.GetTask((Guid)id) ?? throw new Exception("Returned null");

                return View(t);
            }catch (Exception ex)
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(Guid id)
        {
            bool deleted = await _tasks.DeleteTask(id);
            return deleted ? RedirectToAction(nameof(Index)) : NotFound();
        }
    }
}
