using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class Interaction
    {
        public Interaction()
        {
            
        }
        public long Id { get; set; }

        [Required(ErrorMessage ="What is your question?")]
        public string content { get; set; }

        [DataType(DataType.DateTime), DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime postDate { get; set; }

        public virtual ClassTask ClassTask { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}