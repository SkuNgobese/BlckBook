using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Library
    {
        public Library()
        {

        }
        public long Id { get; set; }

        [Required(ErrorMessage = "Please briefly describe the document to be uploaded.")]
        [DisplayName("Description")]
        public string title { get; set; }

        [DisplayName("Date")]
        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:dd-mmm-yyyy-}", ApplyFormatInEditMode = true)]
        public DateTime _date { get; set; }

        [NotMapped]
        [ValidateFile(ErrorMessage ="Please choose a document not more than 5MB.")]
        public HttpPostedFileBase file { get; set; }

        [NotMapped]
        public string streamId { get; set; }

        [NotMapped]
        public string enrollId { get; set; }

        public virtual School School { get; set; }
        public virtual Stream Stream { get; set; }
        public virtual Enrollment Enrollment { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual StudyMaterial StudyMaterial { get; set; }
    }
}