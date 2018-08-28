using DHTMLX.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace The_Book.Models
{
    public class SchoolEvent
    {
        public SchoolEvent()
        {

        }
        [DHXJson(Alias = "id")]
        public long Id { get; set; }

        [DHXJson(Alias = "text")]
        public string Description { get; set; }

        [DHXJson(Alias = "start_date")]
        public DateTime StartDate { get; set; }

        [DHXJson(Alias = "end_date")]
        public DateTime EndDate { get; set; }

        public virtual School School { get; set; }
    }
}