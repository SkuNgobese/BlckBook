using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Student
    {
        public Student()
        {
            TaskSubmissions = new List<TaskSubmission>();
        }
        [Key, ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [DisplayName("Student No.")]
        public string studNo { get; set; }

        [DisplayName("ID/Passport No.")]
        public string idNo { get; set; }

        [DisplayName("Gender")]
        public string gender { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [StringLength(10, ErrorMessage = "Contact number must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [DataType(DataType.PhoneNumber)]
        public string contNo { get; set; }

        [StringLength(10, ErrorMessage = "Telephone number must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Home Tel.")]
        [DataType(DataType.PhoneNumber)]
        public string homeTel { get; set; }

        public bool active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime _date { get; set; }

        public virtual StudentAddress studentAddress { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual Enrollment enrollment { get; set; }
        public virtual School school { get; set; }
        public virtual Parent parent { get; set; }
        public virtual ICollection<TaskSubmission> TaskSubmissions { get; set; }
        public virtual DeactivatedStudent deactivatedStudent { get; set; }
    }
    public class SAStudentViewModel
    {
        [DisplayName("Student No.")]
        [System.Web.Mvc.Remote("StudentNoExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Student number cannot contain letters or special characters")]
        public string studNo { get; set; }

        [RSAIDNumber(ErrorMessage = "A valid RSA ID Number is required")]
        [System.Web.Mvc.Remote("StudentIDExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [DisplayName("ID No.")]
        public string idNo { get; set; }

        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string contNo { get; set; }

        [Required(ErrorMessage = "Please enter home contact number")]
        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Home Tel.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string homeTel { get; set; }

        [Required(ErrorMessage = "Please enter your Grade")]
        [Range(8, 12, ErrorMessage = "Grade must be between 8 and 12")]
        [DisplayName("Grade")]
        public int grade { get; set; }

        [Required(ErrorMessage = "Please enter your class group")]
        [StringLength(1, ErrorMessage = "Class group can be A,B,C etc.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Special characters, spaces & numbers not allowed")]
        [DisplayName("Class Group")]
        public string group { get; set; }
    }
    public class NonSAStudentViewModel
    {
        [DisplayName("Student No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Student number cannot contain letters or special characters")]
        public string studNo { get; set; }

        [Required(ErrorMessage = "A valid {0} is required.")]
        [System.Web.Mvc.Remote("StudentPassportExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [DisplayName("Passport No.")]
        [RegularExpression(@"/^[A-PR-WY][1-9]\d\s?\d{4}[1-9]$/i", ErrorMessage = "A valid {0} is required")]
        public string passportNo { get; set; }

        [StringLength(10, ErrorMessage = "{0} must be 10 digits long.", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string contNo { get; set; }

        [Required(ErrorMessage = "Please enter home contact number")]
        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Home Tel.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string homeTel { get; set; }

        [Required(ErrorMessage = "Please enter your Grade")]
        [Range(8, 12, ErrorMessage = "Grade must be between 8 and 12")]
        [DisplayName("Grade")]
        public int grade { get; set; }

        [Required(ErrorMessage = "Please enter your Class Group")]
        [StringLength(1, ErrorMessage = "Class group can be A,B,C etc.", MinimumLength = 1)]
        [RegularExpression(@"^[a-zA-Z]*$", ErrorMessage = "Special characters, spaces or numbers are not allowed")]
        [DisplayName("Class Group")]
        public string group { get; set; }
    }
    public class SelectStudentEditorViewModel
    {
        public bool Selected { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string idNo { get; set; }
        public string studNo { get; set; }
    }
    public class SelectStudntEditorViewModel
    {
        public bool Selected { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string idNo { get; set; }
        public string grade { get; set; }
    }
    public class StudentsSelectionViewModel
    {
        public List<SelectStudentEditorViewModel> Students { get; set; }
        public StudentsSelectionViewModel()
        {
            this.Students = new List<SelectStudentEditorViewModel>();
        }
        public IEnumerable<string> getSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected teachers:
            return (from p in this.Students where p.Selected select p.Id).ToList();
        }
    }
    public class StudntsSelectionViewModel
    {
        public List<SelectStudntEditorViewModel> Students { get; set; }
        public StudntsSelectionViewModel()
        {
            this.Students = new List<SelectStudntEditorViewModel>();
        }
        public IEnumerable<string> getSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected teachers:
            return (from p in this.Students where p.Selected select p.Id).ToList();
        }
    }
}