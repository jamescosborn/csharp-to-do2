using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Models;
using System;

namespace ToDoList.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public ActionResult Index()
        {
            return View(null);
        }

        [HttpPost("/"), ActionName("Index")]
        public ActionResult IndexPost()
        {
            Task newTask = new Task (Request.Form["new-task"]);
            newTask.Save();
            List<string> allTasks = Task.GetAll();
            return View(allTasks);
        }

        [Route("/task/list")]
        public ActionResult TaskList()
        {
            List<string> allTasks = Task.GetAll();
            return View(allTasks);
        }

        [HttpPost("/task/create")]
        public ActionResult CreateTask()
        {
            Task newTask = new Task (Request.Form["new-task"]);
            newTask.Save();
            return View(newTask);
        }

        [HttpPost("/task/list/clear")]
        public ActionResult TaskListClear()
        {
            Task.ClearAll();
            return View();
        }
    }
}