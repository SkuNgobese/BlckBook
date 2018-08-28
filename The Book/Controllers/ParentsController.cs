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
    public class ParentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Parents
        public ActionResult Index()
        {
            return View(db.Parents.ToList());
        }

        [Authorize(Roles = "Manager")]
        public ActionResult Assign()
        {
            string userId = User.Identity.GetUserId();
            var manager = db.Managers.Find(userId);
            var students = manager.school.Students.ToList();
            var model = new StudntsSelectionViewModel();
            foreach (var student in students)
            {
                var editorViewModel = new SelectStudntEditorViewModel()
                {
                    Id = student.Id,
                    Name = student.ApplicationUser.fullName,
                    idNo = student.idNo,
                    grade = student.enrollment.grade.ToString() + student.enrollment.group,
                    Selected = false
                };
                model.Students.Add(editorViewModel);
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Manager")]
        public ActionResult AssignSelected(StudntsSelectionViewModel model, long Id)
        {
            var manager = db.Managers.Find(User.Identity.GetUserId().ToList());
            var parent = manager.school.Parents.ToList().Find(p => p.Id == Id);
            var selectedIds = model.getSelectedIds();

            var selectedStudents = (from x in manager.school.Students
                                    where selectedIds.Contains(x.Id)
                                    select x);

            foreach (var student in selectedStudents)
            {
                student.parent = parent;
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Parents/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // GET: Parents/Create
        [Authorize(Roles = "Manager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,title,fName,mName,lName,contactNo,workTel,emailAddress")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                var manager = db.Managers.Find(User.Identity.GetUserId().ToString());
                parent.School = manager.school;
                db.Parents.Add(parent);
                db.SaveChanges();
                return RedirectToAction("Assign", new { Id = parent.Id });
            }
            
            return View(parent);
        }

        // GET: Parents/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,fName,mName,lName,contactNo,workTel,emailAddress")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(parent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Parent parent = db.Parents.Find(id);
            if (parent == null)
            {
                return HttpNotFound();
            }
            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Parent parent = db.Parents.Find(id);
            db.Parents.Remove(parent);
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
