using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class StudentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public StudentsController()
        {
        }
        public StudentsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Students
        [Authorize(Roles = "Admin,Manager,Teacher")]
        public ActionResult Index()
        {
            var students = new List<Student>();
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var r = userManager.GetRoles(userId);
            if (r[0].ToString() == "Admin")
            {
                students = (from i in db.Students.ToList()
                            orderby i.ApplicationUser.fullName ascending
                            select i).ToList();
            }
            if (r[0].ToString() == "Manager")
            {
                var man = db.Managers.Find(userId);
                students = (from i in man.school.Students
                            orderby i.ApplicationUser.fullName ascending
                            where i.active == true && i.enrollment != null
                            select i).ToList();
            }
            if (r[0].ToString() == "Teacher")
            {
                var teacher = db.Teachers.Find(userId);

                students = teacher.school.Students.ToList().FindAll(p => teacher.enrollments.Contains(p.enrollment) && p.active == true);                
            }
            return View(students);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        public ActionResult Info(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Admin,Manager,Teacher")]
        public ActionResult infor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: Students/Details/5
        [Authorize]
        public ActionResult Details()
        {
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Activate()
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            var students = teacher.school.Students.ToList().FindAll(p => teacher.enrollments.Contains(p.enrollment) && p.active == false);
            var model = new StudentsSelectionViewModel();
            foreach (var student in students)
            {
                var editorViewModel = new SelectStudentEditorViewModel()
                {
                    Id = student.Id,
                    Name = student.ApplicationUser.fullName,
                    studNo = student.studNo,
                    idNo = student.idNo,
                    Selected = false
                };
                model.Students.Add(editorViewModel);
            }
            return View(model);
        }
        [Authorize(Roles = "Teacher")]
        [HttpPost]
        public async Task<ActionResult> ApproveSelected(StudentsSelectionViewModel model)
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            var selectedIds = model.getSelectedIds();

            var selectedStudents = (from x in teacher.school.Students
                                    where selectedIds.Contains(x.Id)
                                    select x);

            foreach (var student in selectedStudents)
            {
                var user = db.Users.Find(student.Id);
                await this.UserManager.RemoveFromRoleAsync(user.Id, "GuestS");
                await this.UserManager.AddToRoleAsync(user.Id, "Student");
                student.active = true;
                student._date = DateTime.Now;
                if(user.Student.deactivatedStudent != null)
                {
                    db.DeactivatedStudents.Remove(user.Student.deactivatedStudent);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Teachers/Deactivate
        [Authorize(Roles = "Manager,Teacher")]
        public ActionResult Deactivate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        [HttpPost, ActionName("Deactivate")]
        [Authorize(Roles = "Manager,Teacher")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(Student student, string id)
        {
            Student stdent = db.Students.Find(id);
            Teacher teacher = db.Teachers.Find(User.Identity.GetUserId().ToString());
            DeactivatedStudent deactivatedStudent = new DeactivatedStudent();
            await this.UserManager.RemoveFromRoleAsync(stdent.Id, "Student");
            await this.UserManager.AddToRoleAsync(stdent.Id, "GuestS");
            stdent.active = false;
            stdent._date = DateTime.Now;
            deactivatedStudent.Student = stdent;
            deactivatedStudent._date = DateTime.Now;
            deactivatedStudent.reason = student.deactivatedStudent.reason;
            deactivatedStudent.Teacher = teacher;
            db.DeactivatedStudents.Add(deactivatedStudent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Students/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,studNo,idNo,gender,dob,contNo")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: Students/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = db.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Student student = db.Students.Find(id);
            db.Students.Remove(student);
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
