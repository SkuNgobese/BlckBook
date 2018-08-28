using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;

namespace The_Book.Controllers
{
    public class ManagersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Managers
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            var schoolmanagers = (from i in db.Managers.ToList()
                                  orderby i.school.name ascending
                                  select i);

            return View(schoolmanagers);
        }
    }
}