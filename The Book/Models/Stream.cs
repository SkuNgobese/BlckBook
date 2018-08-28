using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Stream
    {
        public Stream()
        {
            Libraries = new List<Library>();
            enrollments = new List<Enrollment>();
            subjects = new List<Subject>();
        }
        public long Id { get; set; }
        
        [Required]
        [DisplayName("Name")]
        public string name { get; set; }

        public virtual School school { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<Enrollment> enrollments { get; set; }
        public virtual ICollection<Subject> subjects { get; set; }
    }
    public class StreamViewModel
    {
        [Required(ErrorMessage = "Please enter stream name!")]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        public string name { get; set; }
    }
}