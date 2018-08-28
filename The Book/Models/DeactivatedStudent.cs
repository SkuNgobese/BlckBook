using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class DeactivatedStudent
    {
        [Key, ForeignKey("Student")]
        public string Id { get; set; }

        [Display(Name = "Reason")]
        [Required(ErrorMessage = "Please provide {0} for deactivating this user")]
        [StringLength(50, ErrorMessage = "{0} must be between {2} & {1} characters long", MinimumLength = 5)]
        [DataType(DataType.MultilineText)]
        public string reason { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime _date { get; set; }

        public virtual Student Student { get; set; }
        public virtual Teacher Teacher { get; set; }
    }
}