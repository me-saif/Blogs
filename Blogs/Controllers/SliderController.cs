using Blogs.Models;
using Blogs.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;

namespace Blogs.Controllers
{
    public class SliderController : Controller
    {
        //private readonly ISliderRepository _employeeRepository;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly BlogDbContext _context;

        public SliderController(/*ISliderRepository employeeRepository,*/
                              IWebHostEnvironment hostingEnvironment,
                              BlogDbContext context)
        {
            //_employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Sliders.ToList());
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSliderVM model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photo property on the incoming model object is not null, then the user
                // has selected an image to upload.
                if (model.Thumbnail != null)
                {
                    // The image must be uploaded to the images folder in wwwroot
                    // To get the path of the wwwroot folder we are using the inject
                    // HostingEnvironment service provided by ASP.NET Core
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "sliders");
                    // To make sure the file name is unique we are appending a new
                    // GUID value and and an underscore to the file name
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Thumbnail.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                    // Use CopyTo() method provided by IFormFile interface to
                    // copy the file to wwwroot/images folder
                    model.Thumbnail.CopyTo(new FileStream(filePath, FileMode.Create));
                }

                Slider newSlider = new Slider
                {
                    ShortDescription = model.ShortDescription,
                    Link = model.Link,
                    Status = model.Status,
                    // Store the file name in PhotoPath property of the employee object
                    // which gets saved to the Employees database table
                    ThumbnailUrl = uniqueFileName
                };

                _context.Add(newSlider);
                _context.SaveChanges();
                return RedirectToAction("Index", "Categories");
            }

            return View();
        }
    }
}