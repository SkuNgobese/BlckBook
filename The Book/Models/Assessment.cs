using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Assessment
    {
        public Assessment()
        {
            AssessmentMarks = new List<AssessmentMark>();
        }
        public long Id { get; set; }

        [Required(ErrorMessage ="Please enter Assessment.")]
        [Display(Name = "Assessment")]
        [RegularExpression(@"^[a-zA-Z0-9 ]*$", ErrorMessage = "You cannot use special characters.")]
        public string name { get; set; }

        [Required(ErrorMessage = "What is the Total Mark for this Assessment?")]
        [Display(Name = "Total Mark")]
        [Range(1, 300)]
        public double totalMark { get; set; }

        [Required(ErrorMessage = "What is CASS Contribution for this Assessment?")]
        [Display(Name = "CASS Contribution")]
        [Range(1, 25)]
        public double cassContribution { get; set; }

        public DateTime date { get; set; }

        public virtual Enrollment Enrollment { get; set; }
        public virtual EnrollmentSubject EnrollmentSubject { get; set; }
        public virtual ICollection<AssessmentMark> AssessmentMarks { get; set; }
    }
}