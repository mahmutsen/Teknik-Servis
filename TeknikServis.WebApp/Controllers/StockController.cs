using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BusinessLayer;
using TeknikServis.Entities;
using TeknikServis.WebApp.Models;
using TeknikServis.WebApp.ViewModels;

namespace TeknikServis.WebApp.Controllers
{
    public class StockController : Controller
    {
        private StockManager sm = new StockManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        // GET: Stock
        public ActionResult Index()
        {
            var result = sm.ListQueryable().Where(x => x.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(x => x.Name);
            return View(result.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Stock stock)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                stock.ServiceId = CurrentSession.Personel.ServiceId;

                if (sm.Insert(stock)>0)
                {
                    RedirectToAction("Index");
                }
                return View(stock);
            }
            return View(stock);
        }

        public ActionResult StockUpdate(int? id)
        {
            if (id==null)
            {
                return View("Error", errorNotifyObj);
            }
            Stock stock = sm.Find(x => x.Id == id);
            if (stock==null)
            {
                return View("Error", errorNotifyObj);
            }

            return View(stock);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult StockUpdate(Stock stock)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Stock db_stock = sm.Find(x => x.Id == stock.Id);
                db_stock.ServiceId = stock.ServiceId;
                db_stock.Name = stock.Name;
                db_stock.Quantity = stock.Quantity;
                db_stock.BPrice = stock.BPrice;

                if (sm.Update(db_stock)>0)
                {
                    return RedirectToAction("Index");
                }
                return View(stock);
            }
            return View(stock);
        }

        [HttpPost]
        public ActionResult Update(int stockid, string dir)
        {
            int res = 0;
            Stock stock = sm.Find(x => x.Id == stockid);

            if (stock != null && dir == "up")
            {
                stock.Quantity += 1;
                res = sm.Update(stock);
            }
            else if (stock != null && dir == "dwn" && stock.Quantity > 0)
            {
                stock.Quantity -= 1;
                res = sm.Update(stock);
            }

            if (res>0)
            {
                return Json(new { hasError = false, errorMessage = string.Empty, result = stock.Quantity });
            }

            return Json(new { hasError = true, errorMessage = "Stok güncellenemedi.", result = stock.Quantity });
        }

        public ActionResult Insert()
        {

            return View();
        }
    }
}