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
    [Exc]
    [Auth]
    [AuthAdmin]
    public class CargoController : Controller
    {
        private CargoManager cargoManager = new CargoManager();
        private ProductManager productManager = new ProductManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var cargos = cargoManager.ListQueryable().Include(f => f.Product).Where(
                x => x.Product.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(x=>x.ModifiedOn);
            return View(cargos.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Cargo cargo = cargoManager.Find(x => x.ProductId == id);
            if (cargo == null)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "İçerik Bulunamadı",
                    RedirectingUrl = "/Cargo/index"
                };

                return View("Error", errorNotifyObj);
            }
            return View(cargo);
        }

        public ActionResult Create(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
                //return View("Error", errorNotifyObj);
            }

            Cargo model = new Cargo();
            model.ProductId = (int)id;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Cargo model)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Cargo> res = cargoManager.Insert(model);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("",x.Message));
                    return View(model);
                }

                return RedirectToAction("AllProducts", "Product");
            }

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Cargo cargo = cargoManager.Find(x => x.ProductId== id);
            if (cargo == null)
            {
                throw new Exception("Kargo bilgisi bulunamadı");
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cargo cargo,string returnUrl)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Cargo> res = cargoManager.Update(cargo,null);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(cargo);
                }
                //return RedirectToAction("AllProducts","Product");
                return Redirect(returnUrl);
            }

            return View(cargo);
        }
        
        public ActionResult Deneme(int ?id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Cargo cargo = cargoManager.Find(x => x.ProductId == id);
            if (cargo == null)
            {
                throw new Exception("Kargo bilgisi bulunamadı");
            }

            return PartialView(cargo);
        }
        public ActionResult LimitedEdit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Cargo cargo = cargoManager.Find(x => x.ProductId == id);
            if (cargo == null)
            {
                throw new Exception("Kargo bilgisi bulunamadı");
            }

            return View(cargo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LimitedEdit(Cargo cargo)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Cargo db_cargo = cargoManager.Find(x => x.ProductId == cargo.ProductId);
                db_cargo.IsArrived = cargo.IsArrived;
                cargoManager.Update(db_cargo);
                return RedirectToAction("Index");
            }
            return View(cargo);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Cargo cargo = cargoManager.Find(x => x.ProductId == id);
            if (cargo == null)
            {
                throw new Exception("Kargo bilgisi bulunamadı");
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(cargo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id,string returnUrl)
        {
            Cargo cargo = cargoManager.Find(x => x.ProductId == id);
            cargoManager.Delete(cargo);

            //return RedirectToAction("Index");
            return Redirect(returnUrl);
        }

    }
}
