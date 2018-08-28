using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Parent
    {
        public Parent()
        {
            Students = new List<Student>();
        }
        public long Id { get; set; }

        [Required(ErrorMessage = "Please choose Title")]
        public string title { get; set; }

        [Required(ErrorMessage = "Your First Name cannot be empty")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [Display(Name = "First Name")]
        public string fName { get; set; }

        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [Display(Name = "Middle Name")]
        public string mName { get; set; }

        [Required(ErrorMessage = "Your Last Name cannot be empty")]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Spaces, special characters & numbers not allowed on {0}")]
        [Display(Name = "Last Name")]
        public string lName { get; set; }

        [Required(ErrorMessage = "Please provide your Cell Number")]
        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string contactNo { get; set; }
        
        [StringLength(10, ErrorMessage = "Telephone number must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Work Tel.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string workTel { get; set; }

        [EmailAddress]
        [Display(Name = "Email Address")]
        [DataType(DataType.EmailAddress)]
        public string emailAddress { get; set; }

        public virtual School School { get; set; }
        public virtual ICollection<Student> Students { get; set; }
    }
}