using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class CASSController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CASS
        [Authorize(Roles = "Teacher")]
        public ActionResult Index()
        {
            string id = User.Identity.GetUserId();
            Teacher teacher = db.Teachers.Find(id);
            if (teacher == null)
            {
                return HttpNotFound();
            }
            return View(teacher);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult AddAssessment()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult AddAssessment(Assessment assessment, long id, bool another)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                Teacher teacher = db.Teachers.Find(userId);
                var subject = teacher.EnrollmentSubjects.ToList().Find(p => p.Id == id);
                if(subject.Assessments.Any())
                {
                    var casscontr = 0.0;
                    foreach(var i in subject.Assessments)
                    {
                        casscontr += i.cassContribution;
                    }
                    if((casscontr + assessment.cassContribution) > 25)
                    {
                        ModelState.AddModelError("cassContribution", "Consider lowering Cass Contribution. In total, all Subject Assessments must make 25%.");
                        return View(assessment);
                    }
                }
                assessment.EnrollmentSubject = subject;
                assessment.Enrollment = subject.Enrollment;
                assessment.date = DateTime.Today;
                db.Assessments.Add(assessment);
                db.SaveChanges();
                if (another == true)
                {
                    return RedirectToAction("AddAssessment");
                }
                return RedirectToAction("Index");
            }
            return View(assessment);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Capture(long? id)
        {
            var cass = db.Assessments.Find(id);
            TempData["cass"] = cass;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Capture(AssessmentMark assessmentMark, long? id, string studId)
        {
            var cass = db.Assessments.Find(id);
            if (!ModelState.IsValid)
            {
                TempData["cass"] = cass;
                return View(assessmentMark);
            }
            if (ModelState.IsValid)
            {
                if(assessmentMark.mark > cass.totalMark)
                {
                    ModelState.AddModelError("mark", "Mark can not exceed Assessment Total Mark!");
                    TempData["cass"] = cass;
                    return View(assessmentMark);
                }
                var student = cass.Enrollment.Students.ToList().Find(p => p.Id == studId);
                assessmentMark.Student = student;
                assessmentMark.Assessment = cass;
                db.AssessmentMarks.Add(assessmentMark);
                db.SaveChanges();
                return RedirectToAction("Capture");
            }
            return View(assessmentMark);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Edit(long? id, string studId)
        {
            var cass = db.Assessments.Find(id);
            TempData["cass"] = cass;
            var assessmentMark = (from i in cass.AssessmentMarks
                                  where i.Student.Id == studId
                                  select i).First();
            return View(assessmentMark);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AssessmentMark assessmentMark, long? id, string studId)
        {
            var cass = db.Assessments.Find(id);
            var assessMark = (from i in cass.AssessmentMarks
                              where i.Student.Id == studId
                              select i).First();
            if (!ModelState.IsValid)
            {
                TempData["cass"] = cass;                
                return View(assessMark);
            }
            if (ModelState.IsValid)
            {
                if (assessmentMark.mark > cass.totalMark)
                {
                    ModelState.AddModelError("mark", "Mark can not exceed Assessment Total Mark!");
                    TempData["cass"] = cass;
                    return View(assessMark);
                }
                assessMark.mark = assessmentMark.mark;
                db.Entry(assessMark).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Capture",new { id = cass.Id });
            }
            return View(assessmentMark);
        }

        [Authorize(Roles = "Teacher")]
        public ActionResult Change(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var assessment = db.Assessments.Find(id);
            if (assessment == null)
            {
                return HttpNotFound();
            }
            return View(assessment);
        }
        [HttpPost]
        [Authorize(Roles = "Teacher")]
        [ValidateAntiForgeryToken]
        public ActionResult Change(Assessment assessment, long? id, long subjId)
        {
            if (ModelState.IsValid)
            {
                string userId = User.Identity.GetUserId();
                Teacher teacher = db.Teachers.Find(userId);
                var subject = teacher.EnrollmentSubjects.ToList().Find(p => p.Id == subjId);
                var asses = subject.Assessments.ToList().Find(p => p.Id == id);
                var casscontr = 0.0;
                foreach (var i in subject.Assessments)
                {
                    if(!i.Equals(asses))
                    {
                        casscontr += i.cassContribution;
                    }
                }
                if ((casscontr + assessment.cassContribution) > 25)
                {
                    ModelState.AddModelError("cassContribution", "Consider lowering Cass Contribution. In total, all Subject Assessments must make 25%.");
                    return View(assessment);
                }
                asses.name = assessment.name;
                asses.totalMark = assessment.totalMark;
                asses.cassContribution = assessment.cassContribution;
                asses.date = DateTime.Today;
                db.Entry(asses).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(assessment);
        }

        [Authorize(Roles = "Student")]
        public ActionResult Report()
        {
            string userId = User.Identity.GetUserId();
            var student = db.Students.Find(userId);
            return View(student);
        }
    }
}