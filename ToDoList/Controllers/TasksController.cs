using Microsoft.AspNetCore.Mvc;
using ToDoList.Data;
using ToDoList.Models;
using static ToDoList.Models.TaskItem;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TasksController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string? status)
        {
            List<TaskItem> tasks = _db.TaskItems.ToList();
            if(status != null)
            {
                Console.WriteLine(status);
                tasks = tasks.FindAll(i => i.Status == status);

                ViewBag.URLFilter = status;
            }
            return View(tasks);
        }

        public IActionResult TableView()
        {
            return View(_db.TaskItems.ToList());
        }

        [HttpPost, ActionName("Index")]
        public IActionResult ChangeStatus(Guid taskid, string newStatus, string filter, string ? redirectTo)
        {
            redirectTo ??= "Index";

            if(redirectTo != "TableView" && redirectTo != "Index") {
                return NotFound();
            }

            TaskItem t = _db.TaskItems.First(t => t.TaskID == taskid);

            if (newStatus == "TD") {
                t.ReopenTask(_db);
            } else if (newStatus == "DO")
            {
                t.DoTask(_db);
            } else if (newStatus == "FI")
            {
                t.EndTask(_db);
            }

            if (filter == null)
            {
                return RedirectToAction(redirectTo);
            }
            else
            {
                return RedirectToAction(redirectTo, routeValues: new {status=filter});
            }
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([Bind("TaskTitle,TaskDescription")] TaskItem task)
        {
            task.CreateTask();

            _db.TaskItems.Add(task);
            _db.SaveChanges();

            TempData["ToasterType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            if (_taskExists(id))
            {
                return View(_db.TaskItems.First(t=>t.TaskID==id));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Edit(Guid id, [Bind("TaskID,TaskTitle,TaskDescription,StartDate,EndDate,Status")] TaskItem task)
        {
            if (_taskExists(id))
            {
                TaskItem t = _db.TaskItems.First(i => i.TaskID == id);
                t.TaskTitle = task.TaskTitle;
                t.TaskDescription = task.TaskDescription;

                _db.TaskItems.Update(t);
                _db.SaveChanges();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }


        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return RedirectToAction(nameof(Index));
            }

            if (_taskExists((Guid)id))
            {
                TaskItem t = _db.TaskItems.First(i => i.TaskID == id);
                return View(t);
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult ConfirmDelete(Guid id)
        {
            Console.WriteLine(id);

            if(_taskExists(id))
            {
                _db.TaskItems.Remove(_db.TaskItems.First(t => t.TaskID == id));
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        
        
        private bool _taskExists(Guid id)
        {
            return _db.TaskItems.Any(t => t.TaskID == id);
        }

    }
}
