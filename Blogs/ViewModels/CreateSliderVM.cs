using Microsoft.AspNetCore.Http;

namespace Blogs.ViewModels
{
    public class CreateSliderVM
    {
        public IFormFile Thumbnail { get; set; }

        public string ShortDescription { get; set; }

        public string Status { get; set; }

        public string Link { get; set; }
    }
}