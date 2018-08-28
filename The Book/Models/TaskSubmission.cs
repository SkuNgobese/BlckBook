using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class TaskSubmission
    {
        public TaskSubmission()
        {

        }
        public long Id { get; set; }

        [NotMapped]
        [ValidateFile(ErrorMessage = "Please select a Document not more than 5MB.")]
        public HttpPostedFileBase file { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime submissionDate { get; set; }

        public virtual Student Student { get; set; }
        public virtual ClassTask ClassTask { get; set; }
        public virtual SubmittedFile SubmittedFile { get; set; }
    }
}