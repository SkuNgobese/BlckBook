using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class SchoolsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Schools
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var schools = (from i in db.Schools.ToList()
                           orderby i.name ascending
                           select i).ToList();
            
            return View(schools);
        }

        // GET: Schools/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // GET: Schools/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schools/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public ActionResult Create(School school)
        {
            if (ModelState.IsValid)
            {
                SchoolAddress schoolAddress = new SchoolAddress();

                if (school.schoolAddress.street != null)
                {
                    school.schoolAddress.street = school.schoolAddress.street.Trim();
                }
                school.name = school.name.Trim();                
                school.schoolAddress.suburb = school.schoolAddress.suburb.Trim();
                school.schoolAddress.city = school.schoolAddress.city.Trim();

                var schl = db.Schools.ToList().Find(p => p.name.ToLower() == school.name.ToLower());
                if (schl != null)
                {
                    ModelState.AddModelError("", "This school has already been registered.");
                    return View(school);
                }

                byte[] imageData = new byte[0];
                if (school.poImgFile != null)
                {
                    using (var binary = new BinaryReader(school.poImgFile.InputStream))
                    {
                        imageData = binary.ReadBytes(school.poImgFile.ContentLength);
                    }
                }
                school.schoolPhoto = imageData;
                schoolAddress.province = school.schoolAddress.province;
                schoolAddress.street = school.schoolAddress.street;
                schoolAddress.suburb = school.schoolAddress.suburb;
                schoolAddress.city = school.schoolAddress.city;
                schoolAddress.code = school.schoolAddress.code;
                db.Schools.Add(school);
                schoolAddress.School = school;
                db.SchoolAddresses.Add(schoolAddress);
                db.SaveChanges();
                var stream = new Models.Stream { name = "Grade 8 & 9", school = school };
                db.Streams.Add(stream);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(school);
        }

        // GET: Schools/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Schools/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,schoolPhoto,name,province,street,suburb,city,code")] School school)
        {
            if (ModelState.IsValid)
            {
                db.Entry(school).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(school);
        }

        // GET: Schools/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            School school = db.Schools.Find(id);
            if (school == null)
            {
                return HttpNotFound();
            }
            return View(school);
        }

        // POST: Schools/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirmed(long id)
        {
            School school = db.Schools.Find(id);
            db.Schools.Remove(school);
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
