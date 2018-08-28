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
    public class TeachersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public TeachersController()
        {
        }
        public TeachersController(ApplicationUserManager userManager)
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

        // GET: Teachers
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Index()
        {
            var teachers = new List<Teacher>();
            string userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var r = userManager.GetRoles(userId);
            if (r[0].ToString() == "Admin")
            {
                teachers = (from i in db.Teachers.ToList()
                            orderby i.ApplicationUser.fullName ascending
                            select i).ToList();                
            }
            if (r[0].ToString() == "Manager")
            {
                var man = db.Managers.Find(userId);
                teachers = (from i in man.school.Teachers.ToList()
                            orderby i._date descending, i.ApplicationUser.fullName ascending
                            where i.active == true
                            select i).ToList();

            }
            return View(teachers);
        }

        [Authorize(Roles ="Admin,Manager")]
        public ActionResult Info(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Infor(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        public ActionResult Activate()
        {
            string userId = User.Identity.GetUserId();
            var man = db.Managers.Find(userId);
            var teachers = new List<Teacher>();            
            teachers = (from i in man.school.Teachers.ToList()
                        orderby i.ApplicationUser.fullName ascending
                        where i.active == false
                        select i).ToList();
            var model = new TeachersSelectionViewModel();
            foreach (var teacher in teachers)
            {
                var editorViewModel = new SelectTeacherEditorViewModel()
                {
                    Id = teacher.Id,
                    Name = teacher.ApplicationUser.fullName,
                    idNo = teacher.idNo,
                    Selected = false
                };
                model.Teachers.Add(editorViewModel);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ApproveSelected(TeachersSelectionViewModel model)
        {
            string userId = User.Identity.GetUserId();
            var man = db.Managers.Find(userId);

            var selectedIds = model.getSelectedIds();

            var selectedTeachers = (from x in man.school.Teachers
                                    where selectedIds.Contains(x.Id)
                                    select x);

            foreach (var teacher in selectedTeachers)
            {
                var user = db.Users.Find(teacher.Id);
                await this.UserManager.RemoveFromRoleAsync(user.Id, "GuestT");
                await this.UserManager.AddToRoleAsync(user.Id, "Teacher");
                teacher.active = true;
                teacher._date = DateTime.Now;
                if(user.Teacher.deactivatedTeacher != null)
                {
                    db.DeactivatedTeachers.Remove(user.Teacher.deactivatedTeacher);
                }
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Teachers/Deactivate
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Deactivate(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }
        [HttpPost, ActionName("Deactivate")]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeactivateConfirmed(Teacher teacher,string id)
        {
            Teacher tcher = db.Teachers.Find(id);
            DeactivatedTeacher deactivatedTeacher = new DeactivatedTeacher();
            await this.UserManager.RemoveFromRoleAsync(tcher.Id, "Teacher");
            await this.UserManager.AddToRoleAsync(tcher.Id, "GuestT");
            tcher.active = false;
            tcher._date = DateTime.Now;
            deactivatedTeacher.Teacher = tcher;
            deactivatedTeacher._date = DateTime.Now;
            deactivatedTeacher.reason = teacher.deactivatedTeacher.reason;
            db.DeactivatedTeachers.Add(deactivatedTeacher);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Teachers/Details/5
        [Authorize]
        public ActionResult Details()
        {            
            string id = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }
        
        // GET: Teachers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,title,empNo,idNo,mobileNo,telNo")] Teacher teacher)
        {
            if (ModelState.IsValid)
            {
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(teacher);
        }

        // GET: Teachers/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        // POST: Teachers/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Teacher teacher = db.Teachers.Find(id);
            db.Teachers.Remove(teacher);
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
