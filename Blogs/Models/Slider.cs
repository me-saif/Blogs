using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class Slider : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string ThumbnailUrl { get; set; }

        public string ShortDescription { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        [StringLength(100)]
        public string Link { get; set; }
    }
}