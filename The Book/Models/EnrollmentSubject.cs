using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class EnrollmentSubject
    {
        public EnrollmentSubject()
        {
            Assessments = new List<Assessment>();
        }
        public long Id { get; set; }

        public string name { get; set; }

        public virtual Enrollment Enrollment { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual ICollection<Assessment> Assessments { get; set; }
    }
}