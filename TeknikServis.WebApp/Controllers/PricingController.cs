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
    //[AuthAdmin]
    [AuthManager]
    [Exc]
    public class PricingController : Controller
    {
        private PricingManager pricingManager = new PricingManager();
        private CategoryManager categoryManager = new CategoryManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var pricings = pricingManager.ListQueryable().Include(p => p.Category).OrderByDescending(x=>x.ModifiedOn);
            return View(pricings.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Pricing pricing = pricingManager.Find(x=>x.Id==id);
            if (pricing == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(pricing);
        }

        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pricing pricing)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                pricingManager.Insert(pricing);
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", pricing.CategoryId);
            return View(pricing);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Pricing pricing = pricingManager.Find(x => x.Id == id);

            if (pricing == null)
            {
                return View("Error", errorNotifyObj);
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", pricing.CategoryId);
            return View(pricing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Pricing pricing)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Pricing db_pricing= pricingManager.Find(x => x.Id == pricing.Id);

                db_pricing.CategoryId = pricing.CategoryId;
                db_pricing.DefectType = pricing.DefectType;
                db_pricing.Price = pricing.Price;
                pricingManager.Update(db_pricing);

                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(categoryManager.List(), "Id", "Title", pricing.CategoryId);
            return View(pricing);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Pricing pricing = pricingManager.Find(x => x.Id == id);
            if (pricing == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(pricing);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Pricing pricing = pricingManager.Find(x => x.Id == id);

            pricingManager.Delete(pricing);

            return RedirectToAction("Index");
        }

    }
}
