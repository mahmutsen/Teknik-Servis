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
    [Exc]
    public class ReportController : Controller
    {
        ProductManager productManager = new ProductManager();
        ReportManager reportManager = new ReportManager();
        /* CustomerProductManager CustomerProductManager = new CustomerProductManager(); *///Bu Manager ismini ProductManager Yap!.ManagerBase e product verildi generic olarak 
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult ShowProductReports(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }

            Product product = productManager.ListQueryable().Include("Reports").FirstOrDefault(x => x.Id == id);//Raporların da aynı anda çekilmesi içen 

            if (product==null)
            {
                throw new Exception("Cihaz bulunamadı");
            }

            return PartialView("_PartialReports",product.Reports);
        }

        [HttpPost]
        public ActionResult Edit(int? id, string text)
        {
            if (id == null)
            {
                throw new Exception("Güncellenecek değer bulunamadı");
            }

            Report report= reportManager.Find(x => x.Id == id);

            if (report==null)
            {
                throw new Exception("Güncellenecek rapor bulunamadı");
            }

            report.Text = text;

            if (reportManager.Update(report) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create(Report report,int? productid)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");
            if (ModelState.IsValid)
            {
                if (productid == null)
                {
                    throw new Exception("Güncellenecek değer bulunamadı");
                }

                Product product = productManager.Find(x => x.Id == productid);

                if (product == null)
                {
                    throw new Exception("Cihaz bulunamadı");
                }

                report.Product = product;
                report.Worker = CurrentSession.Personel;

                if (reportManager.Insert(report) > 0)
                {
                    return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                }

            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                throw new Exception("Güncellenecek değer bulunamadı");
            }

            Report report = reportManager.Find(x => x.Id == id);

            if (report == null)
            {
                throw new Exception("Güncellenecek rapor bulunamadı");
            }

            if (reportManager.Delete(report) > 0)
            {
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { result = false }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            var reports = reportManager.ListQueryable().Include("Product").Include("Worker").Where(
                x => x.Worker.Id == CurrentSession.Personel.Id).OrderByDescending(
                x=>x.ModifiedOn); // product entitiy Joint yapıldı sorguya

            return View(reports.ToList()); //ListQuarable sorgusu burada çalıştırılıyor bütün olarak
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Report report =reportManager.Find(x=>x.Id==id);
            if (report == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(report);
        }

        //public ActionResult Create()
        //{
        //    ViewBag.ProductId = new SelectList(CacheHelper.GetProductsFromCache(), "Id", "Imei");

        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create(Report report)
        //{
        //    ModelState.Remove("CreatedOn");
        //    ModelState.Remove("ModifiedOn");
        //    ModelState.Remove("ModifiedUserName");

        //    if (ModelState.IsValid)
        //    {
        //        report.Worker = CurrentSession.Personel;
        //        reportManager.Insert(report);
        //        return RedirectToAction("Index");
        //    }

        //    ViewBag.ProductId = new SelectList(CacheHelper.GetProductsFromCache(), "Id", "Imei",report.ProductId);

        //    //ViewBag.ProductId = new SelectList(CustomerProductManager.List(), "Id", "Imei", report.ProductId);
        //    return View(report);
        //}

        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error", errorNotifyObj);
        //    }
        //    Report report = reportManager.Find(x=>x.Id==id);
        //    if (report == null)
        //    {
        //        return View("Error", errorNotifyObj);
        //    }

        //    ViewBag.ProductId = new SelectList(CacheHelper.GetProductsFromCache(), "Id", "Imei", report.ProductId);

        //    return View(report);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(Report report)
        //{
        //    ModelState.Remove("CreatedOn");
        //    ModelState.Remove("ModifiedOn");
        //    ModelState.Remove("ModifiedUserName");

        //    if (ModelState.IsValid)
        //    {
        //        Report db_report = reportManager.Find(x => x.Id == report.Id);

        //        db_report.ProductId = report.ProductId;
        //        db_report.Text = report.Text;

        //        reportManager.Update(db_report);

        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.ProductId = new SelectList(CacheHelper.GetProductsFromCache(), "Id", "Imei", report.ProductId);

        //    return View(report);
        //}


        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return View("Error", errorNotifyObj);
        //    }
        //    Report report = reportManager.Find(x => x.Id == id);
        //    if (report == null)
        //    {
        //        return View("Error", errorNotifyObj);
        //    }
        //    return View(report);
        //}


        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Report report = reportManager.Find(x => x.Id == id);
        //    reportManager.Delete(report);
        //    return RedirectToAction("Index");
        //}


    }
}
