using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;
using The_Book.Models.ViewModels;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using System.Data.Entity;
using System.Collections;
using System.Web.Helpers;

namespace The_Book.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private string CalculateAgeToLongString(DateTime birthDay)
        {
            TimeSpan difference = DateTime.Now.Subtract(birthDay);
            DateTime currentAge = DateTime.MinValue + difference;
            int years = currentAge.Year - 1;
            int months = currentAge.Month - 1;
            int days = currentAge.Day - 1;

            return String.Format("{0} years, {1} months and {2} days.", years, months, days);
        }
        [Authorize]
        [RequireHttps]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);
            var events = new List<SchoolEvent>();
            if (s[0].ToString() == "Student")
            {
                var user = db.Students.Find(userId);
                events = (from i in user.school.SchoolEvents
                          orderby i.StartDate ascending
                          where i.EndDate > DateTime.Now
                          select i).ToList();
                TempData["classtasks"] = 0;
                TempData["overdues"] = 0;
                if (user.enrollment != null)
                {
                    var stilltime = (from i in user.enrollment.ClassTasks
                                     where i.submittingOption == "BlckBook" && (DateTime.Today < i.dueDate.Date) || (i.dueDate.Date == DateTime.Today && DateTime.Now.TimeOfDay <= i.dueTime.TimeOfDay)
                                     select i).ToList();

                    var submittedtasks = (from x in user.TaskSubmissions
                                          join y in stilltime
                                          on x.ClassTask.Id equals y.Id
                                          select x).Count();

                    var overdues = (from i in user.enrollment.ClassTasks
                                    where i.submittingOption == "BlckBook" && (DateTime.Today > i.dueDate.Date) || (i.dueDate.Date == DateTime.Today && DateTime.Now.TimeOfDay > i.dueTime.TimeOfDay)
                                    select i).ToList();

                    var submittedoverduetasks = (from x in user.TaskSubmissions
                                                 join y in overdues
                                                 on x.ClassTask.Id equals y.Id
                                                 select x).Count();

                    TempData["classtasks"] = stilltime.Count() - submittedtasks;
                    TempData["overdues"] = overdues.Count() - submittedoverduetasks;
                }
                
            }
            if (s[0].ToString() == "Teacher")
            {
                var user = db.Teachers.Find(userId);
                events = (from i in user.school.SchoolEvents
                          orderby i.StartDate ascending
                          where i.EndDate > DateTime.Now
                          select i).ToList();
                var tasks = (from i in user.ClassTasks
                             orderby i.dueDate descending
                             where i.dueDate.Date >= DateTime.Today
                             select i).ToList();
                if(tasks.Any())
                {
                    TempData["tasks"] = tasks;
                }
                TempData["tasksnumber"] = tasks.Count();
            }
            if (s[0].ToString() == "Manager")
            {
                var user = db.Managers.Find(userId);
                events = (from i in user.school.SchoolEvents
                          orderby i.StartDate ascending
                          where i.EndDate > DateTime.Now
                          select i).ToList();
            }
            
            if(events.Any())
            {
                TempData["events"] = events;
            }
            
            return View();
        }

        public ActionResult NoStuds()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Managers.Find(userId);
            var streamStudents = new List<StreamStudentsVM>();
            foreach (var stream in user.school.Streams)
            {
                StreamStudentsVM temp = new StreamStudentsVM();

                foreach (var enroll in stream.enrollments)
                {
                    temp.number += enroll.Students.Count();
                }

                temp.stream = stream.name;

                streamStudents.Add(new StreamStudentsVM()
                {
                    number = temp.number,
                    stream = temp.stream
                });
            }
            ArrayList xValue = new ArrayList();
            ArrayList yValue = new ArrayList();

            streamStudents.ForEach(x => xValue.Add(x.stream));
            streamStudents.ForEach(x => yValue.Add(x.number));

            new Chart(width: 480, height: 450, theme: ChartTheme.Blue)
            .AddTitle("No. of Registered Students")
            .AddSeries("Default", chartType: "Column", axisLabel: "Streams", xValue: xValue, yValues: yValue)
            .Write("bmp");

            return null;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public FileContentResult UserPhoto()
        {
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            if (!user.userPhoto.Any())
            {
                string fileName = HttpContext.Server.MapPath(@"~/Images/noImg.png");
                byte[] imageData = null;
                FileInfo fileInfo = new FileInfo(fileName);
                long imageFileLength = fileInfo.Length;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                BinaryReader br = new BinaryReader(fs);
                imageData = br.ReadBytes((int)imageFileLength);

                return File(imageData, "image/png");
            }
            return new FileContentResult(user.userPhoto, "image/png");
        }
    }
}