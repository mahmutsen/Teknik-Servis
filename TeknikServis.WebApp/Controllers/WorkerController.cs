using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BusinessLayer;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.Entities;
using TeknikServis.WebApp.Filters;
using TeknikServis.WebApp.Models;
using TeknikServis.WebApp.ViewModels;

namespace TeknikServis.WebApp.Controllers
{
    [AuthAdminOrManager]
    [Auth]
    [Exc]
    public class WorkerController : Controller
    {
        private WorkerManager workerManager = new WorkerManager();
        private ServiceManager serviceManager = new ServiceManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var result = workerManager.ListQueryable().Where(x => x.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(x=>x.ModifiedOn);
            return View(result.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Worker worker = workerManager.Find(x => x.Id == id.Value);
            if (worker == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(worker);
        }

        public ActionResult Create()
        {
            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
            //return View(CurrentSession.Personel); // ModelState.IsValid den sonra woker.serviceId verdiğimiz sistem şaşırırsa buna geri dön!
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Worker worker)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                //worker.ServiceId = CurrentSession.Personel.ServiceId;
                BusinessLayerResult<Worker> res = workerManager.Insert(worker);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", worker.ServiceId);
                    return View(worker);
                }
                return RedirectToAction("Index");
            }

            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", worker.ServiceId);
            return View(worker);
        }


        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Worker worker = workerManager.Find(x => x.Id == id.Value);
            if (worker == null)
            {
                return View("Error", errorNotifyObj);
            }
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", worker.ServiceId);
            return View(worker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Worker worker)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                worker.ServiceId = CurrentSession.Personel.ServiceId;
                BusinessLayerResult<Worker> res = workerManager.Update(worker);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", worker.ServiceId);
                    return View(worker);
                }
                return RedirectToAction("Index");
            }
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", worker.ServiceId);
            return View(worker);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Worker worker = workerManager.Find(x => x.Id == id.Value);
            if (worker == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(worker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Worker worker = workerManager.Find(x => x.Id == id);
            workerManager.Delete(worker);
            return RedirectToAction("Index");
        }

    }
}
