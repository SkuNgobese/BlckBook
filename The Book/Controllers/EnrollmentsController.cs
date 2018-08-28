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
    public class EnrollmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Enrollments
        public ActionResult Index()
        {
            return View(db.Enrollments.ToList());
        }

        // GET: Enrollments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // GET: Enrollments/Create
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

        // POST: Enrollments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,grade,group,streamId")]bool another, Enrollment10to12ViewModel enrollment10to12ViewModel)
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

                return View(enrollment10to12ViewModel);
            }
            if (ModelState.IsValid)
            {
                var StrId = Convert.ToInt64(enrollment10to12ViewModel.streamId);
                var stream = user.school.Streams.ToList().Find(p => p.Id == StrId);
                enrollment10to12ViewModel.group = enrollment10to12ViewModel.group.ToUpper();
                var exist = stream.enrollments.ToList().Find(p => p.grade == enrollment10to12ViewModel.grade && p.group == enrollment10to12ViewModel.group);
                if(exist != null)
                {
                    ModelState.AddModelError("", "Grade " + enrollment10to12ViewModel.grade + enrollment10to12ViewModel.group + " already exists.");
                    var streamList = new List<SelectListItem>();
                    var streamQuery = from e in user.school.Streams
                                      where e.name != "Grade 8 & 9"
                                      select e;
                    foreach (var m in streamQuery)
                    {
                        streamList.Add(new SelectListItem { Value = (m.Id).ToString(), Text = m.name });
                    }
                    ViewBag.streamlist = streamList;

                    return View(enrollment10to12ViewModel);
                }
                var enrollment = new Enrollment { grade = enrollment10to12ViewModel.grade, group = enrollment10to12ViewModel.group, stream = stream };
                foreach (var i in stream.subjects)
                {
                    var subject = new EnrollmentSubject
                    {
                        name = i.name,
                        Enrollment = enrollment
                    };
                    db.EnrollmentSubjects.Add(subject);
                }
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                if(another == true)
                {
                    return RedirectToAction("Create");
                }
                return RedirectToAction("Index", "Home");
            }

            return View(enrollment10to12ViewModel);
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Add()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Add(bool another, Enrollment8to9ViewModel enrollment8to9ViewModel)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var user = db.Managers.Find(userId);
                var stream = user.school.Streams.ToList().Find(p => p.name == "Grade 8 & 9");
                enrollment8to9ViewModel.group = enrollment8to9ViewModel.group.ToUpper();
                var enroll = stream.enrollments.ToList().Find(p => p.grade == enrollment8to9ViewModel.grade && p.group == enrollment8to9ViewModel.group);
                if (enroll != null)
                {
                    ModelState.AddModelError("", "Grade " + enrollment8to9ViewModel.grade + enrollment8to9ViewModel.group + " already exists.");
                    return View(enrollment8to9ViewModel);
                }
                var enrollment = new Enrollment { grade = enrollment8to9ViewModel.grade, group = enrollment8to9ViewModel.group, stream = stream };
                foreach (var i in stream.subjects)
                {
                    var subject = new EnrollmentSubject
                    {
                        name = i.name,
                        Enrollment = enrollment
                    };
                    db.EnrollmentSubjects.Add(subject);
                }
                db.Enrollments.Add(enrollment);
                db.SaveChanges();
                if (another == true)
                {
                    return RedirectToAction("Add", "Enrollments");
                }
                return RedirectToAction("Index", "Home");
            }
            return View(enrollment8to9ViewModel);
        }
        // GET: Enrollments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,grade,group")] Enrollment enrollment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enrollment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(enrollment);
        }

        // GET: Enrollments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Enrollment enrollment = db.Enrollments.Find(id);
            if (enrollment == null)
            {
                return HttpNotFound();
            }
            return View(enrollment);
        }

        // POST: Enrollments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Enrollment enrollment = db.Enrollments.Find(id);
            db.Enrollments.Remove(enrollment);
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
