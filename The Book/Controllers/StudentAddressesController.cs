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
    public class StudentAddressesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: StudentAddresses
        public ActionResult Index()
        {
            var studentAddresses = db.StudentAddresses.Include(s => s.Student);
            return View(studentAddresses.ToList());
        }

        // GET: StudentAddresses/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAddress studentAddress = db.StudentAddresses.Find(id);
            if (studentAddress == null)
            {
                return HttpNotFound();
            }
            return View(studentAddress);
        }

        // GET: StudentAddresses/Create
        [Authorize(Roles = "Student,GuestS")]
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Students, "Id", "studNo");
            return View();
        }

        // POST: StudentAddresses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Student,GuestS")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,street,suburb,city,code")] StudentAddress studentAddress)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                var student = db.Students.Find(userId);
                studentAddress.street = studentAddress.street.Trim();
                studentAddress.suburb = studentAddress.suburb.Trim();
                studentAddress.city = studentAddress.city.Trim();
                studentAddress.Student = student;
                db.StudentAddresses.Add(studentAddress);
                db.SaveChanges();
                return RedirectToAction("Details", "Students");
            }

            ViewBag.Id = new SelectList(db.Students, "Id", "studNo", studentAddress.Id);
            return View(studentAddress);
        }

        // GET: StudentAddresses/Edit/5
        [Authorize(Roles = "Student,GuestS")]
        public ActionResult Edit()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAddress studentAddress = db.StudentAddresses.Find(id);
            if (studentAddress == null)
            {
                return HttpNotFound();
            }
            return View(studentAddress);
        }

        // POST: StudentAddresses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,street,suburb,city,code")] StudentAddress studentAddress)
        {
            if (ModelState.IsValid)
            {
                db.Entry(studentAddress).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Students");
            }
            return View(studentAddress);
        }

        // GET: StudentAddresses/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            StudentAddress studentAddress = db.StudentAddresses.Find(id);
            if (studentAddress == null)
            {
                return HttpNotFound();
            }
            return View(studentAddress);
        }

        // POST: StudentAddresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            StudentAddress studentAddress = db.StudentAddresses.Find(id);
            db.StudentAddresses.Remove(studentAddress);
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
