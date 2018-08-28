using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class StudentAddress
    {
        [Key, ForeignKey("Student")]
        public string Id { get; set; }

        [Required(ErrorMessage = "Please enter House number and Street!")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "Street address cannot contain special characters")]
        [Display(Name = "Street")]
        public string street { get; set; }

        [Required(ErrorMessage = "Please enter Suburb/Location!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Suburb cannot contain numbers or special characters")]
        [Display(Name = "Suburb")]
        public string suburb { get; set; }

        [Required(ErrorMessage = "Please enter City/Town!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "City cannot contain numbers or special characters")]
        [Display(Name = "City")]
        public string city { get; set; }

        [Required(ErrorMessage = "Please enter Postal Code!")]
        [StringLength(4, ErrorMessage = "Postal Code is 4 digits long", MinimumLength = 4)]
        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Code cannot contain letter or special characters.")]
        [Display(Name = "Code")]
        public string code { get; set; }

        public virtual Student Student { get; set; }
    }
}