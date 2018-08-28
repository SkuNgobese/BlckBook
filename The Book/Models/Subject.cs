using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Subject
    {
        public Subject()
        {

        }
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter subject name!")]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Subject Name")]
        public string name { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please choose the stream the subject belongs to!")]
        public string streamId { get; set; }

        public virtual Stream stream { get; set; }
    }
    public class SubjectViewModel
    {
        [Required(ErrorMessage = "Please enter subject name!")]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Subject Name")]
        public string name { get; set; }
    }
}