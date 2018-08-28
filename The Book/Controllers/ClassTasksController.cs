using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class ClassTasksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ClassTasks
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);
            
            var classtasks = new List<ClassTask>();
            if(s[0].ToString() == "Student")
            {
                var user = db.Students.Find(userId);
                classtasks = (from i in user.enrollment.ClassTasks
                              orderby i.dueDate descending
                              where i.postDate.Year == DateTime.Today.Year
                              select i).ToList();
            }
            if (s[0].ToString() == "Teacher")
            {
                var user = db.Teachers.Find(userId);
                classtasks = (from i in user.ClassTasks
                              orderby i.dueDate descending
                              where i.postDate.Year == DateTime.Today.Year
                              select i).ToList();
            }
            TempData["classtasks"] = classtasks;

            return View();
        }
        [Authorize(Roles = "Student,Teacher")]
        public ActionResult Timeline()
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);
            var classtasks = new List<ClassTask>();
            if (s[0].ToString() == "Student")
            {
                var user = db.Students.Find(userId);
                classtasks = (from i in user.enrollment.ClassTasks
                              orderby i.postDate descending
                              where i.postDate.Year == DateTime.Today.Year
                              select i).ToList();
            }
            if (s[0].ToString() == "Teacher")
            {
                var user = db.Teachers.Find(userId);
                classtasks = (from i in user.ClassTasks
                              orderby i.postDate descending
                              where i.postDate.Year == DateTime.Today.Year
                              select i).ToList();
            }
            TempData["classtasks"] = classtasks;
            
            return View();
        }
        public FileContentResult teacherPhoto(string id)
        {
            var user = db.Users.Find(id);
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
        public FileContentResult userPhoto(string id)
        {
            var user = db.Users.Find(id);
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
        // GET: ClassTasks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTasks.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            return View(classTask);
        }

        // GET: ClassTasks/Create
        [Authorize(Roles = "Teacher")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var enrollList = new List<SelectListItem>();
            var teacher = db.Teachers.Find(userId);
            var enrollsQuery = (from e in teacher.enrollments
                                orderby e.grade, e.@group ascending
                                select e);
            foreach (var m in enrollsQuery)
            {
                enrollList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade + m.group });
            }
            ViewBag.streamlist = enrollList;
            TempData["classesNum"] = teacher.enrollments.Count();
            return View();
        }

        // POST: ClassTasks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ClassTask classTask)
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            var currTime = DateTime.Now;
            if (!ModelState.IsValid)
            {
                var enrollList = new List<SelectListItem>();
                var enrollsQuery = (from e in teacher.enrollments
                                    orderby e.grade, e.@group ascending
                                    select e);
                foreach (var m in enrollsQuery)
                {
                    enrollList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade + m.group });
                }

                ViewBag.streamlist = enrollList;
                TempData["classesNum"] = teacher.enrollments.Count();
                return View(classTask);
            }
            if (classTask.dueDate < DateTime.Today)
            {
                ModelState.AddModelError("dueDate", "Due date cannot be past date.");
                var enrollList = new List<SelectListItem>();
                var enrollsQuery = (from e in teacher.enrollments
                                    orderby e.grade, e.@group ascending
                                    select e);
                foreach (var m in enrollsQuery)
                {
                    enrollList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade + m.group });
                }

                ViewBag.streamlist = enrollList;
                TempData["classesNum"] = teacher.enrollments.Count();
                return View(classTask);
            }
            var x = DateTime.Compare(DateTime.Now, classTask.dueTime);
            if (classTask.dueDate == DateTime.Today && x > 0)
            {
                ModelState.AddModelError("dueTime", "Time cannot be past time.");
                var enrollList = new List<SelectListItem>();
                var enrollsQuery = (from e in teacher.enrollments
                                    orderby e.grade, e.@group ascending
                                    select e);
                foreach (var m in enrollsQuery)
                {
                    enrollList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade + m.group });
                }

                ViewBag.streamlist = enrollList;
                TempData["classesNum"] = teacher.enrollments.Count();
                return View(classTask);
            }
            if (ModelState.IsValid)
            {
                if(classTask.file != null)
                {
                    var fileName = Path.GetFileName(classTask.file.FileName);
                    ClassTaskFile classTaskFile = new ClassTaskFile()
                    {
                        FileName = fileName,
                        Extension = Path.GetExtension(fileName),
                        Id = classTask.Id,
                        ClassTask = classTask
                    };
                    classTask.ClassTaskFile = classTaskFile;                    
                }

                classTask.Teacher = teacher;
                long enrId = Convert.ToInt64(classTask.enrollId);
                var Enroll = teacher.enrollments.ToList().Find(p => p.Id == enrId);
                classTask.postDate = DateTime.Now;
                classTask.Enrollment = Enroll;
                db.ClassTasks.Add(classTask);
                db.SaveChanges();
                if (classTask.file != null)
                {
                    var fileName = Path.GetFileName(classTask.file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/ClassTaskFiles/"), classTask.ClassTaskFile.Id + fileName);
                    classTask.file.SaveAs(path);
                }
                return RedirectToAction("Index");
            }

            return View(classTask);
        }

        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/ClassTaskFiles/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        public FileResult DownloadFile(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/SubmittedFiles/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Submit()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Student")]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(TaskSubmission taskSubmission, long id)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var student = db.Students.Find(userId);
                var classTask = student.enrollment.ClassTasks.ToList().Find(p => p.Id == id);
                var submission = classTask.TaskSubmissions.ToList().Find(p => p.Student.Id == student.Id);
                if (submission != null)
                {
                    ModelState.AddModelError("", "You have already submitted this task.");
                    return View(taskSubmission);
                }
                if (classTask.submittingOption == "BlckBook" && (classTask.dueDate.Date == DateTime.Today && DateTime.Today.TimeOfDay > classTask.dueTime.TimeOfDay) || (DateTime.Today > classTask.dueDate.Date))
                {
                    ModelState.AddModelError("", "You cannot submit online anymore, please see your teacher.");
                    return View(taskSubmission);
                }
                if (taskSubmission.file != null)
                {
                    var fileName = Path.GetFileName(taskSubmission.file.FileName);
                    SubmittedFile submittedFile = new SubmittedFile()
                    {
                        FileName = fileName,
                        Extension = Path.GetExtension(fileName),
                        Id = taskSubmission.Id,
                        TaskSubmission = taskSubmission
                    };
                    taskSubmission.SubmittedFile = submittedFile;
                }
                
                taskSubmission.Student = student;
                taskSubmission.ClassTask = classTask;
                taskSubmission.submissionDate = DateTime.Now;
                db.TaskSubmissions.Add(taskSubmission);
                db.SaveChanges();
                if (taskSubmission.file != null)
                {
                    var fileName = Path.GetFileName(taskSubmission.file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/SubmittedFiles/"), taskSubmission.SubmittedFile.Id + fileName);
                    taskSubmission.file.SaveAs(path);
                }
                return RedirectToAction("Index");
            }
            return View(taskSubmission);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Submitted(long id)
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            var classTask = teacher.ClassTasks.ToList().Find(p => p.Id == id);
            var submissions = classTask.TaskSubmissions.ToList();
            
            TempData["name"] = classTask.heading;
            return View(submissions);
        }

        // GET: ClassTasks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTasks.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            return View(classTask);
        }

        // POST: ClassTasks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,heading,body")] ClassTask classTask)
        {
            if (ModelState.IsValid)
            {
                db.Entry(classTask).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(classTask);
        }

        // GET: ClassTasks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ClassTask classTask = db.ClassTasks.Find(id);
            if (classTask == null)
            {
                return HttpNotFound();
            }
            return View(classTask);
        }

        // POST: ClassTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ClassTask classTask = db.ClassTasks.Find(id);
            db.ClassTasks.Remove(classTask);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
