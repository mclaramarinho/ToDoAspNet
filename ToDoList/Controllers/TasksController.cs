using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using static ToDoList.Models.TaskItem;

namespace ToDoList.Controllers
{
    public class TasksController : Controller
    {
        private static List<TaskItem> _tasks = new List<TaskItem>();

        public IActionResult Index(string? status)
        {
            List<TaskItem> tasks = _tasks.ToList();
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
            return View(_tasks.ToList());
        }

        [HttpPost, ActionName("Index")]
        public IActionResult ChangeStatus(Guid taskid, string newStatus, string filter, string ? redirectTo)
        {
            redirectTo ??= "Index";

            if(redirectTo != "TableView" && redirectTo != "Index") {
                return NotFound();
            }

            if (newStatus == "TD") {
                _tasks.First(t => t.TaskID == taskid).CreateTask();
            } else if (newStatus == "DO")
            {
                _tasks.First(t => t.TaskID == taskid).DoTask();
            } else if (newStatus == "FI")
            {
                _tasks.First(t => t.TaskID == taskid).EndTask();
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
            
            _tasks.Add(task);

            TempData["ToasterType"] = "success";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(Guid id)
        {
            if (_taskExists(id))
            {
                return View(_tasks.First(t=>t.TaskID==id));
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
                TaskItem t = _tasks.First(i => i.TaskID == id);
                t.TaskTitle = task.TaskTitle;
                t.TaskDescription = task.TaskDescription;
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
                TaskItem t = _tasks.First(i => i.TaskID == id);
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
                _tasks.Remove(_tasks.First(t => t.TaskID == id));
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return NotFound();
            }
        }

        
        
        private bool _taskExists(Guid id)
        {
            return _tasks.Any(t => t.TaskID == id);
        }

    }
}
