using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Enrollment
    {
        public Enrollment()
        {
            EnrollmentSubjects = new List<EnrollmentSubject>();
            Assessments = new List<Assessment>();
            Libraries = new List<Library>();
            this.Teachers = new HashSet<Teacher>();
            Students = new List<Student>();
            ClassTasks = new List<ClassTask>();
        }
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter Grade.")]
        [DisplayName("Grade")]
        public int grade { get; set; }

        [Required(ErrorMessage = "Please enter class group.")]
        [StringLength(1, ErrorMessage = "Class group can be A,B,C,D,E,F,G or H etc...", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Class Group")]
        public string group { get; set; }

        public virtual Stream stream { get; set; }
        public virtual ICollection<EnrollmentSubject> EnrollmentSubjects { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Teacher> Teachers { get; set; }
        public virtual ICollection<ClassTask> ClassTasks { get; set; }
    }
    public class Enrollment8to9ViewModel
    {
        [Required(ErrorMessage = "Please enter Grade")]
        [Range(8, 9, ErrorMessage = "Grade must be between 8 and 9.")]
        [DisplayName("Grade")]
        public int grade { get; set; }

        [Required(ErrorMessage = "Please enter class group.")]
        [StringLength(1, ErrorMessage = "Class group can be A,B,C,D,E,F,G or H etc...", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Class Group")]
        public string group { get; set; }
    }
    public class Enrollment10to12ViewModel
    {
        [Required(ErrorMessage = "Please choose the stream this class is under.")]
        public string streamId { get; set; }

        [Required(ErrorMessage = "Please enter Grade.")]
        [Range(10, 12, ErrorMessage = "Grade must be between 10 and 12.")]
        [DisplayName("Grade")]
        public int grade { get; set; }

        [Required(ErrorMessage = "Please enter class group.")]
        [StringLength(1, ErrorMessage = "Class group can be A,B,C,D,E,F,G or H etc...", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]* {0,1}[a-zA-Z]*$", ErrorMessage = "Special characters or numbers are not allowed!")]
        [DisplayName("Class Group")]
        public string group { get; set; }
    }
}