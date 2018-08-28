using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class ClassTask
    {
        public ClassTask()
        {
            Interactions = new List<Interaction>();
            TaskSubmissions = new List<TaskSubmission>();
        }
        public long Id { get; set; }

        [Required(ErrorMessage = "Please enter task's Header")]
        [MaxLength(100,ErrorMessage ="Please keep task's title short, not more than 100 letters")]
        public string heading { get; set; }

        [Required(ErrorMessage = "Please briefly describe the task")]
        [DataType(DataType.MultilineText)]
        public string content { get; set; }

        [Required(ErrorMessage = "Please enter task due date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dueDate { get; set; }

        [Required(ErrorMessage = "Please enter submission time")]
        [DataType(DataType.Time), DisplayFormat(DataFormatString = "{0:HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime dueTime { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime postDate { get; set; }

        [Required(ErrorMessage = "Please choose Submitting Option")]
        public string submittingOption { get; set; }

        [NotMapped]
        [ValidateFile(ErrorMessage = "Please ensure your file is a Document not more than 5MB.")]
        public HttpPostedFileBase file { get; set; }

        [NotMapped]
        [Required(ErrorMessage = "Please select class here.")]
        public string enrollId { get; set; }

        public virtual Teacher Teacher { get; set; }
        public virtual Enrollment Enrollment { get; set; }
        public virtual ClassTaskFile ClassTaskFile { get; set; }
        public virtual ICollection<Interaction> Interactions { get; set; }
        public virtual ICollection<TaskSubmission> TaskSubmissions { get; set; }
    }
}