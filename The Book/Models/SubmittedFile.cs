using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class SubmittedFile
    {
        public SubmittedFile()
        {

        }
        [Key, ForeignKey("TaskSubmission")]
        public long Id { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }

        public virtual TaskSubmission TaskSubmission { get; set; }
    }
}