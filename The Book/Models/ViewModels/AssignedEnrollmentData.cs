using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Book.Models.ViewModels
{
    public class AssignedEnrollmentData
    {
        public long EnrollmentId { get; set; }

        public int grade { get; set; }

        public string group { get; set; }

        public bool Assigned { get; set; }
    }
}