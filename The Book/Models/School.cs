using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class School
    {
        public School()
        {
            Students = new List<Student>();
            Teachers = new List<Teacher>();
            Streams = new List<Stream>();
            Libraries = new List<Library>();
            SchoolEvents = new List<SchoolEvent>();
            Parents = new List<Parent>();
        }
        public long Id { get; set; }

        [Display(Name = "School Emblem")]
        public byte[] schoolPhoto { get; set; }

        [NotMapped]
        [ValidatePicture(ErrorMessage = "Make sure 'School Emblem' is a picture and not more than 1MB size")]
        public HttpPostedFileBase poImgFile { get; set; }

        [Required(ErrorMessage = "School name cannot be empty!")]
        [RegularExpression(@"^[a-zA-Z ]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Name")]
        public string name { get; set; }

        [StringLength(10, ErrorMessage = "Telephone number must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Telephone No.")]
        [DataType(DataType.PhoneNumber)]
        public string TelNo { get; set; }

        [StringLength(10, ErrorMessage = "Telephone number must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Fax No.")]
        [DataType(DataType.PhoneNumber)]
        public string FaxNo { get; set; }

        [EmailAddress]
        [System.Web.Mvc.Remote("IsAlreadySigned", "Account", HttpMethod = "POST", ErrorMessage = "Email already exists in database")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        public virtual SchoolAddress schoolAddress { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<Stream> Streams { get; set; }
        public virtual ICollection<SchoolEvent> SchoolEvents { get; set; }
        public virtual ICollection<Parent> Parents { get; set; } 
    }
}