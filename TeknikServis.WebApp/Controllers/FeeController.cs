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
using TeknikServis.Entities.ViewModelObjects;
using TeknikServis.WebApp.Filters;
using TeknikServis.WebApp.Models;
using TeknikServis.WebApp.ViewModels;

namespace TeknikServis.WebApp.Controllers
{
    [Auth]
    [AuthAdmin]
    [Exc]
    public class FeeController : Controller
    {
        private FeeManager feeManager = new FeeManager();
        private ProductManager productManager = new ProductManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var fees = feeManager.ListQueryable().Include(f => f.Product).Where(
                x => x.Product.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(x => x.ModifiedOn);
            return View(fees.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Fee fee = feeManager.Find(x => x.ProductId == id);
            if (fee == null)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "İçerik Bulunamadı",
                    RedirectingUrl = "/Fee/index"
                };

                return View("Error", errorNotifyObj);
            }
            return View(fee);
        }

        public ActionResult Create(int? id)
        {

            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }

            var model = new Fee();
            model.ProductId = (int)id;

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fee model)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Fee> res = feeManager.Insert(model);
                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
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
            Fee fee = feeManager.Find(x=>x.ProductId==id);
            if (fee == null)
            {
                throw new Exception("Ücret bilgisi bulunamadı");
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(fee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fee fee, string returnUrl)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BusinessLayerResult<Fee> res = feeManager.FullUpdate(fee);

                if (res.Errors.Count>0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    return View(fee);
                }
                //return RedirectToAction("AllProducts","Product");
                return Redirect(returnUrl);
            }
            return View(fee);
        }

        public ActionResult LimitedEdit(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Fee fee = feeManager.Find(x => x.ProductId == id);
            if (fee == null)
            {
                throw new Exception("Ücret bilgisi bulunamadı");
            }

            return View(fee);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LimitedEdit(Fee fee)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Fee db_fee = feeManager.Find(x => x.ProductId == fee.ProductId);
                db_fee.IsDenied = fee.IsDenied;
                db_fee.IsPaid = fee.IsPaid;
                feeManager.Update(db_fee);
                return RedirectToAction("Index");
            }
            return View(fee);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Id parametresi null olamaz");
            }
            Fee fee = feeManager.Find(x => x.ProductId == id);
            if (fee == null)
            {
                throw new Exception("Ücret bilgisi bulunamadı");
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(fee);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Fee fee = feeManager.Find(x => x.ProductId == id);
            feeManager.Delete(fee);

            //return RedirectToAction("Index");
            return Redirect(returnUrl);
        }

    }
}
