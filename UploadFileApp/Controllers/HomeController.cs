using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UploadFileApp.Models;
using UploadFileApp.ViewModels;

namespace UploadFileApp.Controllers
{
    public class HomeController : Controller
    {
        ApplicationContext _context;
        IHostingEnvironment _appEnvironment;

        public HomeController(ApplicationContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _appEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View(_context.Files.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                string path = "/Files" + uploadedFile.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                FileModel file = new FileModel { Name = uploadedFile.FileName, Path = path };
                _context.Files.Add(file);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View(_context.People.ToList());
        }

        [HttpPost]
        public IActionResult Create(PersonViewModel pvm)
        {
            Person person = new Person { Name = pvm.Name };
            if (pvm.Avatar != null)
            {
                byte[] imageData = null;

                using (var binaryReader = new BinaryReader(pvm.Avatar.OpenReadStream()))
                {
                    imageData = binaryReader.ReadBytes((int)pvm.Avatar.Length);
                }
                person.Avatar = imageData;
            }
            _context.People.Add(person);
            _context.SaveChanges();

            return RedirectToAction("Create");
        }
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
