using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class Blog : BaseEntity
    {
        public int Id { get; set; }

        [StringLength(200)]
        public string Title { get; set; }

        public int CategoryId { get; set; }
        public List<Category> Categories { get; set; }

        public string Tags { get; set; }

        [StringLength(100)]
        public string Status { get; set; }

        public string ShortDescription { get; set; }
        public string Content { get; set; }
    }
}