using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class LibrariesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Libraries
        [Authorize(Roles = "Manager,Teacher,Student")]
        public ActionResult Index(string search)
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);
            var library = new List<Library>();
            var recent = new List<Library>();
            if (s[0].ToString() == "Student")
            {
                var user = db.Students.Find(userId);
                if(search !=null)
                {
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               where i.Enrollment == user.enrollment && i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper()) ||
                               i.Stream == user.enrollment.stream && i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper()) ||
                               i.Stream == null && i.Enrollment == null && i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper())
                               select i).ToList();
                }
                else
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               where i.Enrollment == user.enrollment ||
                               i.Stream == user.enrollment.stream ||
                               i.Stream == null && i.Enrollment == null
                               select i).ToList();

                recent = (from i in user.school.Libraries
                          orderby i._date descending
                          where i.Enrollment == user.enrollment && i._date.Month == DateTime.Today.Month || 
                          i.Stream == user.enrollment.stream && i._date.Month == DateTime.Today.Month ||
                          i.Stream == null && i.Enrollment == null && i._date.Month == DateTime.Today.Month
                          select i).ToList();
            }
            if (s[0].ToString() == "Teacher")
            {
                var user = db.Teachers.Find(userId);                
                if(search != null)
                {
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               where i.Teacher == user && i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper()) ||
                               i.Stream == null && i.Teacher == null && i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper())
                               select i).ToList();
                }
                else
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               where i.Teacher == user ||
                               i.Stream == null && i.Teacher == null
                               select i).ToList();

                recent = (from i in user.school.Libraries
                          orderby i._date descending
                          where i.Teacher == user && i._date.Month == DateTime.Today.Month ||
                          i.Stream == null && i.Teacher == null && i._date.Month == DateTime.Today.Month
                          select i).ToList();
            }
            if (s[0].ToString() == "Manager")
            {
                var user = db.Managers.Find(userId);
                if(search != null)
                {
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               where i.title.ToUpper().Contains(search.ToUpper()) || i.StudyMaterial.FileName.ToUpper().Contains(search.ToUpper())
                               select i).ToList();
                }
                else
                    library = (from i in user.school.Libraries
                               orderby i._date descending
                               select i).ToList();

                recent = (from i in user.school.Libraries
                          orderby i._date descending
                          where i._date.Month == DateTime.Today.Month
                          select i).ToList();
            }
            TempData["library"] = library;
            TempData["recent"] = recent;
            return View();
        }
        public FileResult Download(String p, String d)
        {
            return File(Path.Combine(Server.MapPath("~/App_Data/StudyMaterials/"), p), System.Net.Mime.MediaTypeNames.Application.Octet, d);
        }
        // GET: Libraries/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // GET: Libraries/Create
        [Authorize(Roles = "Teacher,Manager")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);
            if (s[0].ToString() == "Teacher")
            {
                var enrollList = new List<SelectListItem>();
                var teacher = db.Teachers.Find(userId);
                var enrollsQuery = (from e in teacher.enrollments
                                   orderby e.grade,e.@group ascending
                                   select e);
                foreach (var m in enrollsQuery)
                {
                    enrollList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade+m.group });
                }
                ViewBag.streamlist = enrollList;
            }
            if (s[0].ToString() == "Manager")
            {
                var user = db.Managers.Find(userId);
                var streamList = new List<SelectListItem>();
                var streamQuery = (from e in user.school.Streams
                                   orderby e.name ascending
                                   where e.name != "Grade 8 & 9"
                                   select e);
                foreach (var m in streamQuery)
                {
                    streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
                }
                ViewBag.streamlist = streamList;
            }
            return View();
        }

        // POST: Libraries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Teacher,Manager")]
        public ActionResult Create(Library library)
        {
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var s = userManager.GetRoles(userId);            
            if(!ModelState.IsValid)
            {
                if (s[0].ToString() == "Teacher")
                {
                    var streamList = new List<SelectListItem>();
                    var teacher = db.Teachers.Find(userId);
                    var enrollsQuery = (from e in teacher.enrollments
                                        orderby e.grade, e.@group ascending
                                        select e);
                    foreach (var m in enrollsQuery)
                    {
                        streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.grade + m.group });
                    }
                    ViewBag.streamlist = streamList;
                }
                if (s[0].ToString() == "Manager")
                {
                    var user = db.Managers.Find(userId);
                    var streamList = new List<SelectListItem>();
                    var streamQuery = (from e in user.school.Streams
                                       orderby e.name ascending
                                       where e.name != "Grade 8 & 9"
                                       select e);
                    foreach (var m in streamQuery)
                    {
                        streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
                    }
                    ViewBag.streamlist = streamList;
                }
                return View(library);
            }
            if (ModelState.IsValid)
            {
                if (s[0].ToString() == "Manager")
                {
                    var manager = db.Managers.Find(userId);
                    library.School = manager.school;
                    if (library.streamId != null)
                    {
                        var strId = Convert.ToInt64(library.streamId);
                        var stream = manager.school.Streams.ToList().Find(p => p.Id == strId);
                        library.Stream = stream;
                    }
                }
                if (s[0].ToString() == "Teacher")
                {
                    var teacher = db.Teachers.Find(userId);
                    library.School = teacher.school;
                    library.Teacher = teacher;
                    var enrId = Convert.ToInt64(library.enrollId);
                    var Enroll = teacher.enrollments.ToList().Find(p => p.Id == enrId);
                    library.Enrollment = Enroll;
                }
                var fileName = Path.GetFileName(library.file.FileName);
                StudyMaterial studymaterial = new StudyMaterial()
                {
                    FileName = fileName,
                    Extension = Path.GetExtension(fileName),
                    Id = library.Id,
                    Library = library
                };
                library.StudyMaterial = studymaterial;
                
                library._date = DateTime.Now;
                db.Libraries.Add(library);
                db.SaveChanges();
                var path = Path.Combine(Server.MapPath("~/App_Data/StudyMaterials/"), library.StudyMaterial.Id+fileName);
                library.file.SaveAs(path);
                return RedirectToAction("Index");
            }

            return View(library);
        }

        // GET: Libraries/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.StudyMaterials, "Id", "FileName", library.Id);
            return View(library);
        }

        // POST: Libraries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title")] Library library)
        {
            if (ModelState.IsValid)
            {
                db.Entry(library).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.StudyMaterials, "Id", "FileName", library.Id);
            return View(library);
        }

        // GET: Libraries/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Library library = db.Libraries.Find(id);
            if (library == null)
            {
                return HttpNotFound();
            }
            return View(library);
        }

        // POST: Libraries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Library library = db.Libraries.Find(id);
            db.Libraries.Remove(library);
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
