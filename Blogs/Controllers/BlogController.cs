using Blogs.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blogs.Controllers
{
    public class BlogController : Controller
    {
        private readonly BlogDbContext _context;

        public BlogController(BlogDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            ViewBag.categoryList = _context.Categories.Where(x => x.Delete_Status == true).ToList();
            var result = "";
            return View();
        }
    }
}