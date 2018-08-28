using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models.ViewModels
{
    public class ParentEmailVM
    {
        [Required(ErrorMessage = "Please enter Subject line")]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }
    }
}