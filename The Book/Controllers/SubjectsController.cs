using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class SubjectsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Subjects
        public ActionResult Index()
        {
            return View(db.Subjects.ToList());
        }

        // GET: Subjects/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // GET: Subjects/Create for Grade 10-12
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Managers.Find(userId);
            var streamList = new List<SelectListItem>();
            var streamQuery = from e in user.school.Streams
                              where e.name != "Grade 8 & 9"
                              select e;
            foreach (var m in streamQuery)
            {
                streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
            }
            ViewBag.streamlist = streamList;

            return View();
        }

        // POST: Subjects/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,name,streamId")]bool another, Subject subject)
        {
            string userId = User.Identity.GetUserId();
            var user = db.Managers.Find(userId);
            if (!ModelState.IsValid)
            {
                var streamList = new List<SelectListItem>();
                var streamQuery = from e in user.school.Streams
                                  where e.name != "Grade 8 & 9"
                                  select e;
                foreach (var m in streamQuery)
                {
                    streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
                }
                ViewBag.streamlist = streamList;

                return View(subject);
            }
            if (ModelState.IsValid)
            {
                var StrId = Convert.ToInt64(subject.streamId);
                var stream = user.school.Streams.ToList().Find(p => p.Id == StrId);
                var exist = stream.subjects.ToList().Find(p => p.name.ToLower() == subject.name.ToLower());
                if(exist != null)
                {
                    ModelState.AddModelError("", subject.name + " already exists under " + exist.stream.name + " Stream.");
                    var streamList = new List<SelectListItem>();
                    var streamQuery = from e in user.school.Streams
                                      where e.name != "Grade 8 & 9"
                                      select e;
                    foreach (var m in streamQuery)
                    {
                        streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
                    }
                    ViewBag.streamlist = streamList;

                    return View(subject);
                }
                if (stream.enrollments.Any())
                {
                    foreach (var i in stream.enrollments)
                    {
                        var subj = new EnrollmentSubject
                        {
                            name = subject.name,
                            Enrollment = i
                        };
                        db.EnrollmentSubjects.Add(subj);
                    }
                }
                subject.stream = stream;
                db.Subjects.Add(subject);
                db.SaveChanges();
                if(another == true)
                {
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Create", "Enrollments");
            }
            return View(subject);
        }
        //Get Subjects/Add  for Grade 8-9
        [Authorize(Roles = "Manager")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Add([Bind(Exclude = "streamId")]SubjectViewModel subjectViewModel, bool another)
        {
            if(ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var user = db.Managers.Find(userId);
                var stream = user.school.Streams.ToList().Find(p => p.name == "Grade 8 & 9");
                var exist = stream.subjects.ToList().Find(p => p.name.ToLower() == subjectViewModel.name.ToLower());
                if (exist != null)
                {
                    ModelState.AddModelError("", subjectViewModel.name + " already exists.");
                    return View(subjectViewModel);
                }
                var subject = new Subject { name = subjectViewModel.name, stream = stream, streamId = "0" };
                
                if (stream.enrollments.Any())
                {
                    foreach (var i in stream.enrollments)
                    {
                        var subj = new EnrollmentSubject
                        {
                            name = subject.name,
                            Enrollment = i
                        };
                        db.EnrollmentSubjects.Add(subj);
                    }
                }
                db.Subjects.Add(subject);
                db.SaveChanges();
                if(another == true)
                {
                    return RedirectToAction("Add", "Subjects");
                }
                return RedirectToAction("Add", "Enrollments");
            }
            return View(subjectViewModel);
        }
        // GET: Subjects/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,name")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subject).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subject);
        }

        // GET: Subjects/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subject subject = db.Subjects.Find(id);
            if (subject == null)
            {
                return HttpNotFound();
            }
            return View(subject);
        }

        // POST: Subjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subject subject = db.Subjects.Find(id);
            db.Subjects.Remove(subject);
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
