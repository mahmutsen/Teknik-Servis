using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BusinessLayer;
using TeknikServis.Entities;
using TeknikServis.WebApp.Filters;
using TeknikServis.WebApp.Models;
using TeknikServis.WebApp.ViewModels;

namespace TeknikServis.WebApp.Controllers
{
    [Auth]
    [AuthManager]
    [Exc]
    public class ServiceController : Controller
    {
        private ServiceManager serviceManager = new ServiceManager();
        private CategoryManager categoryManager = new CategoryManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var services = serviceManager.ListQueryable().Include(s => s.Category).OrderByDescending(x=>x.Id);
            return View(services.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Service service = serviceManager.Find(x=>x.Id==id);
            if (service == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(service);
        }        

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Service service)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                serviceManager.Insert(service);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", service.CategoryId);
            return View(service);
        }

        // GET: Service/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Service service = serviceManager.Find(x => x.Id == id);
            if (service == null)
            {
                return View("Error", errorNotifyObj);
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", service.CategoryId);
            return View(service);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Service service)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Service db_service = serviceManager.Find(x => x.Id == service.Id);
                db_service.Title = service.Title;
                db_service.Tel = service.Tel;
                db_service.Fax = service.Fax;
                db_service.Email = service.Email;
                db_service.City = service.City;
                db_service.Adress = service.Adress;
                db_service.Text = service.Text;
                db_service.CategoryId = service.CategoryId;

                serviceManager.Update(db_service);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", service.CategoryId);
            return View(service);
        }

        // GET: Service/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Service service = serviceManager.Find(x => x.Id == id);
            if (service == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(service);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Service service = serviceManager.Find(x => x.Id == id);
            serviceManager.Delete(service);

            return RedirectToAction("Index");
        }

    }
}
