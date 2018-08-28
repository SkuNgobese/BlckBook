using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class AssessmentMark
    {
        public AssessmentMark()
        {

        }
        public long Id { get; set; }

        [Required(ErrorMessage ="Mark ?")]
        public double mark { get; set; }

        public virtual Assessment Assessment { get; set; }
        public virtual Student Student { get; set; }
    }
}