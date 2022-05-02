using System;
using System.ComponentModel.DataAnnotations;

namespace Blogs.Models
{
    public class BaseEntity
    {
        [StringLength(100)]
        public string CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{MM/dd/yyyy hh:mm tt}")]
        public DateTime CreatedDate { get; set; }

        [StringLength(100)]
        public string UpdatedBy { get; set; }

        [DisplayFormat(DataFormatString = "{MM/dd/yyyy hh:mm tt}")]
        public DateTime UpdatedDate { get; set; }
    }
}