using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using The_Book.Models;
using DHTMLX.Scheduler;
using DHTMLX.Scheduler.Data;
using DHTMLX.Common;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace The_Book.Controllers
{
    public class SchoolEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize(Roles = "Manager")]
        public ActionResult Manage()
        {
            var scheduler = new DHXScheduler(this);
            scheduler.Skin = DHXScheduler.Skins.Glossy;

            scheduler.Config.first_hour = 6;
            scheduler.Config.last_hour = 20;

            scheduler.EnableDynamicLoading(SchedulerDataLoader.DynamicalLoadingMode.Month);

            scheduler.LoadData = true;
            scheduler.EnableDataprocessor = true;

            return View(scheduler);
        }
        public ContentResult Data(DateTime from, DateTime to)
        {
            string userId = User.Identity.GetUserId();
            var user = db.Managers.Find(userId);
            var schlEvs = user.school.SchoolEvents.ToList().FindAll(e => e.StartDate < to && e.EndDate >= from).ToList();
            return new SchedulerAjaxData(schlEvs);
        }
        public ActionResult Save(int? id, FormCollection actionValues)
        {
            var action = new DataAction(actionValues);
            string userId = User.Identity.GetUserId();
            var user = db.Managers.Find(userId);
            try
            {
                var changedEvent = DHXEventsHelper.Bind<SchoolEvent>(actionValues);
                switch (action.Type)
                {
                    case DataActionTypes.Insert:
                        changedEvent.School = user.school;
                        db.SchoolEvents.Add(changedEvent);
                        break;
                    case DataActionTypes.Delete:
                        db.Entry(changedEvent).State = EntityState.Deleted;
                        break;
                    default:// "update"
                        changedEvent.School = user.school;
                        db.Entry(changedEvent).State = EntityState.Modified;
                        break;
                }
                db.SaveChanges();
                action.TargetId = changedEvent.Id;
            }
            catch (Exception)
            {
                action.Type = DataActionTypes.Error;
            }

            return (new AjaxSaveResponse(action));
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