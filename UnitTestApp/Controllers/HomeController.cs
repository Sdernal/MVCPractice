using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UnitTestApp.Models;

namespace UnitTestApp.Controllers
{
    public class HomeController : Controller
    {
        IRepository repo;
        public HomeController(IRepository r)
        {
            repo = r;
        }
        public HomeController()
        {
            repo = null; 
        }
        public IActionResult Index()
        {
            ViewData["Message"] = "Hello world!";
            return View("Index");
        }
        public IActionResult About()
        {
            return View(repo.GetAll());
        }

        public IActionResult GetPhone(int? id)
        {
            if (!id.HasValue)
                return BadRequest();
            Phone phone = repo.Get(id.Value);
            if (phone == null)
                return NotFound();
            return View(phone);
        }

        public IActionResult AddPhone(Phone phone)
        {
            if (ModelState.IsValid)
            {
                repo.Create(phone);
                return RedirectToAction("About");
            }
            return View(phone);
        }
    }
}
