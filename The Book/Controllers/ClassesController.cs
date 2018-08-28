using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;
using The_Book.Models.ViewModels;

namespace The_Book.Controllers
{
    public class ClassesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        private ICollection<AssignedEnrollmentData> PopulateEnrollmentData()
        {
            string userId = User.Identity.GetUserId();
            var user = db.Teachers.Find(userId);
            var enrollments = (from i in db.Enrollments.ToList()
                               orderby i.grade, i.@group ascending
                               where i.stream.school.Id == user.school.Id
                               select i).ToList();
            var assignedEnrollments = new List<AssignedEnrollmentData>();
            foreach (var item in enrollments)
            {
                assignedEnrollments.Add(new AssignedEnrollmentData
                {
                    EnrollmentId = item.Id,
                    grade = item.grade,
                    group = item.group,
                    Assigned = false
                });
            }
            return assignedEnrollments;
        }

        private ICollection<AssignedEnrollmentData> PopulateUpdateEnrollmentData(Teacher teacher)
        {
            var assignedEnrollments = new List<AssignedEnrollmentData>();
            foreach (var x in teacher.enrollments)
            {
                assignedEnrollments.Add(new AssignedEnrollmentData
                {
                    EnrollmentId = x.Id,
                    grade = x.grade,
                    group = x.group,
                    Assigned = true
                });
            }
            var Enrollments = (from i in db.Enrollments.ToList()
                               orderby i.grade, i.@group ascending
                               where i.stream.school.Id == teacher.school.Id
                               select i).Except(teacher.enrollments).ToList();
            foreach (var item in Enrollments)
            {
                assignedEnrollments.Add(new AssignedEnrollmentData
                {
                    EnrollmentId = item.Id,
                    grade = item.grade,
                    group = item.group,
                    Assigned = false
                });                
            }
            return assignedEnrollments;
        }

        private void AddEnrollments(Teacher teacher, IEnumerable<AssignedEnrollmentData> assignedEnrollments)
        {
            if (assignedEnrollments != null)
            {
                foreach (var assignedEnrollment in assignedEnrollments)
                {
                    if (assignedEnrollment.Assigned)
                    {
                        var enrollment = new Enrollment { Id = assignedEnrollment.EnrollmentId };
                        teacher.enrollments.Add(enrollment);
                        db.Enrollments.Attach(enrollment);
                    }
                }
            }
        }

        [Authorize(Roles = "GuestT")]
        public ActionResult Teacher()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "GuestT")]
        [ValidateAntiForgeryToken]
        public ActionResult Teacher(SATeacherViewModel teacherViewModel)
        {
            if (ModelState.IsValid)
            {
                int year = Convert.ToInt16(teacherViewModel.idNo.Substring(0, 2));
                int month = Convert.ToInt16(teacherViewModel.idNo.Substring(2, 2));
                int day = Convert.ToInt16(teacherViewModel.idNo.Substring(4, 2));
                var dobstring = year + "/" + month + "/" + day;
                var dob = DateTime.Parse(dobstring);
                var gender = "Male";
                int gNo = Convert.ToInt16(teacherViewModel.idNo.Substring(6, 1));
                if (gNo < 5)
                {
                    gender = "Female";
                }
                string userId = User.Identity.GetUserId();
                var teach = db.Teachers.Find(userId);
                var teacher = new Teacher { Id = teach.Id, ApplicationUser = teach.ApplicationUser, title = teacherViewModel.title, empNo = teacherViewModel.empNo, idNo = teacherViewModel.idNo, dob = dob, _date = DateTime.Now, gender = gender, mobileNo = teacherViewModel.mobileNo, telNo = teacherViewModel.telNo };
                teacher.school = teach.school;
                db.Teachers.Add(teacher);
                db.SaveChanges();
                if (teacher.teacherAddress == null)
                {
                    return RedirectToAction("Create", "TeacherAddresses");
                }
                return RedirectToAction("AddClasses");
            }
            return View(teacherViewModel);
        }

        [Authorize(Roles = "Teacher,GuestT")]
        public ActionResult AddClasses()
        {
            var teacherEnrollmentViewModel = new TeacherEnrollmentViewModel { enrollments = PopulateEnrollmentData() };
            
            return View(teacherEnrollmentViewModel);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher,GuestT")]
        [ValidateAntiForgeryToken]
        public ActionResult AddClasses(TeacherEnrollmentViewModel teacherEnrollmentViewModel)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId();
                var teacher = db.Teachers.Find(id);
                AddEnrollments(teacher, teacherEnrollmentViewModel.enrollments);
                db.Entry(teacher).State = EntityState.Modified;
                if (!teacher.enrollments.Any())
                {
                    ModelState.AddModelError("", "Please select atleast one Class you are taking in your school.");
                    var teacherEnrollViewModel = new TeacherEnrollmentViewModel { enrollments = PopulateEnrollmentData() };

                    return View(teacherEnrollViewModel);
                }
                db.SaveChanges();
                return RedirectToAction("AddSubjects");
            }
            return View(teacherEnrollmentViewModel);
        }

        [Authorize(Roles = "Teacher,GuestT")]
        public ActionResult AddSubjects()
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            TempData["enrolllist"] = teacher.enrollments.ToList();

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher,GuestT")]
        [ValidateAntiForgeryToken]
        public ActionResult AddSubjects(TeacherSubjectViewModel teacherSubjectViewModel)
        {
            if (ModelState.IsValid)
            {
                string id = User.Identity.GetUserId(); 
                var teacher = db.Teachers.Find(id);
                var classesnum = teacher.enrollments.Count();
                var listitems = teacherSubjectViewModel.subjectIds.ToList().Where(p => p != "").Count();
                if (!classesnum.Equals(listitems))
                {
                    ModelState.AddModelError("", "Please choose your Subject for each Class you are taking.");
                    TempData["enrolllist"] = teacher.enrollments.ToList();
                    return View(teacherSubjectViewModel);
                }
                var subjects = new List<EnrollmentSubject>();
                foreach (var i in teacherSubjectViewModel.subjectIds)
                {
                    var subjId = Convert.ToInt64(i);
                    var subject = db.EnrollmentSubjects.Find(subjId);
                    subjects.Add(subject);
                }
                teacher.EnrollmentSubjects = subjects;
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Teachers");
            }
            return View(teacherSubjectViewModel);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult UpdateClasses()
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            if(teacher.enrollments.Any())
            {
                var teacherEnrollmentViewModel = new TeacherEnrollmentViewModel { enrollments = PopulateUpdateEnrollmentData(teacher) };
                return View(teacherEnrollmentViewModel);
            }
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateClasses(TeacherEnrollmentViewModel model)
        {
            string id = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(id);
            if (ModelState.IsValid)
            {
                if (model.enrollments.Any())
                {
                    var enrollments = new List<Enrollment>();
                    foreach (var i in model.enrollments)
                    {
                        if(i.Assigned)
                        {
                            var enroll = db.Enrollments.ToList().Find(p => p.Id == i.EnrollmentId);
                            enrollments.Add(enroll);
                        }
                    }
                    var Enrollments = (from i in enrollments
                                       select i).Except(teacher.enrollments).ToList();
                    foreach (var i in Enrollments)
                    {
                        var enrollment = enrollments.ToList().Find(p => p.Id == i.Id);
                        teacher.enrollments.Add(enrollment);
                    }
                    var dlt = teacher.enrollments.ToList().FindAll(p => !enrollments.Contains(p));
                    foreach (var i in dlt)
                    {
                        var subj = i.EnrollmentSubjects.ToList().Find(p => p.Teacher == (teacher));
                        teacher.EnrollmentSubjects.Remove(subj);
                        teacher.enrollments.Remove(i);
                    }
                }
                db.Entry(teacher).State = EntityState.Modified;
                if (!teacher.enrollments.Any())
                {
                    ModelState.AddModelError("", "You cannot uncheck all classes, A teacher must have atleast one class.");
                    var teacherEnrollmentViewModel = new TeacherEnrollmentViewModel { enrollments = PopulateUpdateEnrollmentData(teacher) };

                    return View(teacherEnrollmentViewModel);
                }
                db.SaveChanges();
                foreach (var i in teacher.enrollments)
                {
                    var subj = i.EnrollmentSubjects.ToList().Find(p => p.Teacher == (teacher));
                    if (subj == null)
                    {
                        return RedirectToAction("UpdateSubjects");
                    }
                }
                return RedirectToAction("Details", "Teachers");
            }
            return View(model);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult UpdateSubjects()
        {
            string userId = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(userId);
            var enrollments = new List<Enrollment>();
            foreach (var i in teacher.enrollments)
            {
                var subj = i.EnrollmentSubjects.ToList().Find(p => p.Teacher == (teacher));
                if(subj == null)
                {
                    enrollments.Add(i);
                }
            }
            TempData["enrolllist"] = enrollments;

            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateSubjects(TeacherSubjectViewModel teacherSubjectViewModel)
        {
            string id = User.Identity.GetUserId();
            var teacher = db.Teachers.Find(id);
            var enrollments = new List<Enrollment>();
            foreach (var i in teacher.enrollments)
            {
                var subj = i.EnrollmentSubjects.ToList().Find(p => p.Teacher == (teacher));
                if (subj == null)
                {
                    enrollments.Add(i);
                }
            }
            if(!ModelState.IsValid)
            {
                TempData["enrolllist"] = enrollments;
                return View(teacherSubjectViewModel);
            }
            if (ModelState.IsValid)
            {
                var listitems = teacherSubjectViewModel.subjectIds.ToList().Where(p => p != "").Count();
                if (!enrollments.Count().Equals(listitems))
                {
                    ModelState.AddModelError("", "Please choose your Subject for each new Class you are taking.");
                    TempData["enrolllist"] = enrollments;
                    return View(teacherSubjectViewModel);
                }
                foreach (var i in teacherSubjectViewModel.subjectIds)
                {
                    var subjId = Convert.ToInt64(i);
                    var subject = db.EnrollmentSubjects.Find(subjId);
                    teacher.EnrollmentSubjects.Add(subject);
                }
                db.Entry(teacher).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", "Teachers");
            }
            return View(teacherSubjectViewModel);
        }

        [Authorize(Roles = "GuestS")]
        public ActionResult Student()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "GuestS")]
        [ValidateAntiForgeryToken]
        public ActionResult Student(SAStudentViewModel studentViewModel)
        {            
            if (ModelState.IsValid)
            {
                int year = Convert.ToInt16(studentViewModel.idNo.Substring(0, 2));
                int month = Convert.ToInt16(studentViewModel.idNo.Substring(2, 2));
                int day = Convert.ToInt16(studentViewModel.idNo.Substring(4, 2));
                var dobstring = year + "/" + month + "/" + day;
                var dob = DateTime.Parse(dobstring);
                var gender = "Male";
                int gNo = Convert.ToInt16(studentViewModel.idNo.Substring(6, 1));
                if (gNo < 5)
                {
                    gender = "Female";
                }

                string userId = User.Identity.GetUserId();
                var stude = db.Students.Find(userId);
                var enroll = new Enrollment();
                foreach (var strm in stude.school.Streams)
                {
                    enroll = strm.enrollments.ToList().Find(x => x.grade == studentViewModel.grade && x.group == studentViewModel.group.ToUpper());
                    if(enroll != null)
                    {
                        break;
                    }
                }
                if (enroll == null)
                {
                    ModelState.AddModelError("", "Your School do not have Grade (" + studentViewModel.grade + studentViewModel.group + ") registered");
                    return View(studentViewModel);
                }

                var student = new Student { Id = stude.Id, ApplicationUser = stude.ApplicationUser, studNo = studentViewModel.studNo, idNo = studentViewModel.idNo, contNo = studentViewModel.contNo, homeTel = studentViewModel.homeTel, dob = dob, _date = DateTime.Now, gender = gender, enrollment = enroll, school = stude.school };
                db.Students.Add(student);
                db.SaveChanges();
                if (student.studentAddress == null)
                {
                    return RedirectToAction("Create", "StudentAddresses");
                }
                return RedirectToAction("Details", "Students");
            }
            return View(studentViewModel);
        }

        [AllowAnonymous]
        public JsonResult TeacherIDExists(string idNo)
        {
            var isExist = db.Teachers.ToList().Find(p => p.idNo == idNo);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult EmployeeNoExists(string empNo)
        {
            var isExist = db.Teachers.ToList().Find(p => p.empNo == empNo);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult StudentIDExists(string idNo)
        {
            var isExist = db.Students.ToList().Find(p => p.idNo == idNo);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult StudentNoExists(string studNo)
        {
            var isExist = db.Students.ToList().Find(p => p.studNo == studNo);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult TeacherPassportExists(string passport)
        {
            var isExist = db.Teachers.ToList().Find(p => p.idNo == passport);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        public JsonResult StudentPassportExists(string passport)
        {
            var isExist = db.Students.ToList().Find(p => p.idNo == passport);
            return Json(isExist == null, JsonRequestBehavior.AllowGet);
        }
    }
}