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
    public class TeacherAddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TeacherAddresses
        public ActionResult Index()
        {
            var teacherAddresses = db.TeacherAddresses.Include(t => t.Teacher);
            return View(teacherAddresses.ToList());
        }

        // GET: TeacherAddresses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAddress teacherAddress = db.TeacherAddresses.Find(id);
            if (teacherAddress == null)
            {
                return HttpNotFound();
            }
            return View(teacherAddress);
        }

        // GET: TeacherAddresses/Create
        [Authorize(Roles = "Teacher,GuestT")]
        public ActionResult Create()
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            if(teacher.enrollments.Any())
            {
                TempData["assigned"] = "true";
            }
            else
                TempData["assigned"] = "false";
            return View();
        }

        // POST: TeacherAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Teacher,GuestT")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,street,suburb,city,code")] TeacherAddress teacherAddress)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(userId);
                teacherAddress.street = teacherAddress.street.Trim();
                teacherAddress.suburb = teacherAddress.suburb.Trim();
                teacherAddress.city = teacherAddress.city.Trim();
                teacherAddress.Teacher = teacher;
                db.TeacherAddresses.Add(teacherAddress);
                db.SaveChanges();
                if (!teacher.enrollments.Any())
                {
                    return RedirectToAction("AddClasses","Classes");
                }
                else
                    return RedirectToAction("Details", "Teachers");
            }

            ViewBag.Id = new SelectList(db.Teachers, "Id", "title", teacherAddress.Id);
            return View(teacherAddress);
        }

        // GET: TeacherAddresses/Edit/5
        [Authorize(Roles = "Teacher")]
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAddress teacherAddress = db.TeacherAddresses.Find(id);
            if (teacherAddress == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Teachers, "Id", "title", teacherAddress.Id);
            return View(teacherAddress);
        }

        // POST: TeacherAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,street,suburb,city,code")] TeacherAddress teacherAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacherAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Teachers");
            }
            ViewBag.Id = new SelectList(db.Teachers, "Id", "title", teacherAddress.Id);
            return View(teacherAddress);
        }

        // GET: TeacherAddresses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TeacherAddress teacherAddress = db.TeacherAddresses.Find(id);
            if (teacherAddress == null)
            {
                return HttpNotFound();
            }
            return View(teacherAddress);
        }

        // POST: TeacherAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TeacherAddress teacherAddress = db.TeacherAddresses.Find(id);
            db.TeacherAddresses.Remove(teacherAddress);
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
