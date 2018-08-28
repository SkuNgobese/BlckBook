using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using The_Book.Models.ViewModels;

namespace The_Book.Models
{
    public class Teacher
    {
        public Teacher()
        {
            this.enrollments = new HashSet<Enrollment>();
            EnrollmentSubjects = new List<EnrollmentSubject>();
            Libraries = new List<Library>();
            ClassTasks = new List<ClassTask>();
        }
        [Key, ForeignKey("ApplicationUser")]
        public string Id { get; set; }

        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Employment No.")]
        public string empNo { get; set; }

        [DisplayName("ID/Passport No.")]
        public string idNo { get; set; }

        [DisplayName("Gender")]
        public string gender { get; set; }

        [DisplayName("Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime dob { get; set; }

        [DisplayName("Mobile No.")]
        [DataType(DataType.PhoneNumber)]
        public string mobileNo { get; set; }

        [DisplayName("Telephone No.")]
        [DataType(DataType.PhoneNumber)]
        public string telNo { get; set; }

        public bool active { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime _date { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual School school { get; set; }
        public virtual TeacherAddress teacherAddress { get; set; }
        public virtual ICollection<Enrollment> enrollments { get; set; }
        public virtual ICollection<EnrollmentSubject> EnrollmentSubjects { get; set; }
        public virtual ICollection<Library> Libraries { get; set; }
        public virtual ICollection<ClassTask> ClassTasks { get; set; }
        public virtual DeactivatedTeacher deactivatedTeacher { get; set; }
    }
    public class SATeacherViewModel
    {
        [Required(ErrorMessage = "Please choose Salutation")]
        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Employment No.")]
        [System.Web.Mvc.Remote("EmployeeNoExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Employment number cannot contain spaces, letters or special characters")]
        public string empNo { get; set; }

        [RSAIDNumber(ErrorMessage = "A valid RSA ID Number is required")]
        [System.Web.Mvc.Remote("TeacherIDExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [DisplayName("ID No.")]
        public string idNo { get; set; }

        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string mobileNo { get; set; }

        [StringLength(10, ErrorMessage = "{0} must be 10 digits long", MinimumLength = 10)]
        [DisplayName("Telephone No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Please enter valid {0}")]
        [DataType(DataType.PhoneNumber)]
        public string telNo { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    public class NonSATeacherViewModel
    {
        [Required(ErrorMessage = "Please choose Salutation")]
        [DisplayName("Title")]
        public string title { get; set; }

        [DisplayName("Employment No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Employment number cannot contain spaces, letters or special characters")]
        public string empNo { get; set; }

        [Required(ErrorMessage = "A valid {0} is required")]
        [System.Web.Mvc.Remote("TeacherPassportExists", "Classes", ErrorMessage = "You cannot use someone else's {0}")]
        [DisplayName("Passport No.")]
        [RegularExpression(@"/^[A-PR-WY][1-9]\d\s?\d{4}[1-9]$/i", ErrorMessage = "A valid Passport Number is required")]
        public string passportNo { get; set; }

        [StringLength(10, ErrorMessage = "Contact number must be 10 digits long.", MinimumLength = 10)]
        [DisplayName("Mobile No.")]
        [RegularExpression(@"^\d+$", ErrorMessage = "Mobile number cannot contain spaces, letters or special characters")]
        [DataType(DataType.PhoneNumber)]
        public string mobileNo { get; set; }

        [StringLength(10, ErrorMessage = "Contact number must be 10 digits long", MinimumLength = 10)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Phone number cannot contain spaces, letters or special characters")]
        [DisplayName("Telephone No.")]
        [DataType(DataType.PhoneNumber)]
        public string telNo { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
    public class TeacherEnrollmentViewModel
    {
        public virtual ICollection<AssignedEnrollmentData> enrollments { get; set; }
    }
    public class TeacherSubjectViewModel
    {
        [Required(ErrorMessage = "Please choose your Subject for this Class")]
        public List<string> subjectIds { get; set; }
    }
    public class SelectTeacherEditorViewModel
    {
        public bool Selected { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string idNo { get; set; }
    }
    public class TeachersSelectionViewModel
    {
        public List<SelectTeacherEditorViewModel> Teachers { get; set; }
        public TeachersSelectionViewModel()
        {
            this.Teachers = new List<SelectTeacherEditorViewModel>();
        }
        public IEnumerable<string> getSelectedIds()
        {
            // Return an Enumerable containing the Id's of the selected teachers:
            return (from p in this.Teachers where p.Selected select p.Id).ToList();
        }
    }
}