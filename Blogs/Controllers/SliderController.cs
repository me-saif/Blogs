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
            var result = _context.Sliders.ToList().OrderByDescending(x => x.Id).Where(x => x.Status == "published");
            return View(result);
        }

        public IActionResult List()
        {
            var result = _context.Sliders.ToList().OrderByDescending(x => x.Id).Where(x => x.Status == "published");
            return View(result);
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

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Slider slider = _context.Sliders.Find(id);
            EditSliderVM sliderEditViewModel = new EditSliderVM
            {
                Id = slider.Id,
                Status = slider.Status,
                ShortDescription = slider.ShortDescription,
                Link = slider.Link,
                ExistingThumbnail = slider.ThumbnailUrl
            };
            return View(sliderEditViewModel);
        }

        [HttpPost]
        public IActionResult Edit(EditSliderVM model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                // Retrieve the employee being edited from the database
                Slider slider = _context.Sliders.Find(model.Id);
                // Update the employee object with the data in the model object
                slider.ShortDescription = model.ShortDescription;
                slider.Status = model.Status;
                slider.Link = model.Link;

                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Thumbnail != null)
                {
                    // If a new photo is uploaded, the existing photo must be
                    // deleted. So check if there is an existing photo and delete
                    if (model.ExistingThumbnail != null)
                    {
                        string filePath = Path.Combine(_hostingEnvironment.WebRootPath,
                            "sliders", model.ExistingThumbnail);
                        System.IO.File.Delete(filePath);
                    }
                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    slider.ThumbnailUrl = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table

                _context.Sliders.Attach(slider).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.SaveChanges();

                //Slider updatedSlider = _context.Sliders.Update(slider);

                return RedirectToAction("index");
            }

            return View(model);
        }

        private string ProcessUploadedFile(CreateSliderVM model)
        {
            string uniqueFileName = null;

            if (model.Thumbnail != null)
            {
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "sliders");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Thumbnail.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Thumbnail.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
    }
}