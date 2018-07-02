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
    [Exc]
    public class ProductController : Controller
    {
        private ProductManager productManager = new ProductManager();
        private ServiceManager serviceManager = new ServiceManager();
        private CustomerManager customerManager = new CustomerManager();
        private WorkerManager workerManager = new WorkerManager();
        private FeeManager feeManager = new FeeManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();


        //[AuthAdmin]
        //public ActionResult Index()
        //{
        //    var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Service).Where(
        //        x => x.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(
        //        x => x.ModifiedOn);
        //    return View(products.ToList());
        //}

        public ActionResult Index()
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Personel).Where(
                x => x.WorkerId == CurrentSession.Personel.Id).OrderByDescending(
                x => x.ModifiedOn);
            return View(products.ToList());
        }

        public ActionResult PersonelIndex(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId && x.WorkerId == CurrentSession.Personel.Id);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products.OrderByDescending(p=>p.ModifiedOn));
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Product product = productManager.Find(x => x.Id == id);
            if (product == null)
            {
                return View("Error", errorNotifyObj);
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(product);
        }

        [AuthAdmin]
        public ActionResult Create()
        {
            ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) || x.Products.Count == 0).OrderByDescending(
                x => x.ModifiedOn).ToList(), "Id", "Tel");
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
            return View();
        }

        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                product.ServiceId = CurrentSession.Personel.ServiceId;
                BusinessLayerResult<Product> res = productManager.Insert(product);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));

                    ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) || x.Products.Count == 0).OrderByDescending(
                x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
                    //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", product.ServiceId);

                    return View(product);
                }

                CacheHelper.RemoveProductsFromCache();
                return RedirectToAction("Index");
            }

            ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) || x.Products.Count == 0).OrderByDescending(
                x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", product.ServiceId);

            return View(product);
        }

        [AuthAdmin]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Product product = productManager.Find(x => x.Id == id);
            if (product == null)
            {
                return View("Error", errorNotifyObj);
            }
            ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Include(x => x.Products).Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) && x.Products.Count == 0).OrderByDescending(
                x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
            ViewBag.WorkerId = new SelectList(workerManager.ListQueryable().Where(x => x.ServiceId == CurrentSession.Personel.ServiceId).ToList(), "Id", "Username", product.WorkerId);
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(product);
        }

        [AuthAdmin]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Product product, string returnUrl)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                //product.ServiceId = CurrentSession.Personel.ServiceId;
                BusinessLayerResult<Product> res = productManager.Update(product);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Include(x => x.Products).
                        Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) && x.Products.Count == 0).OrderByDescending
                        (x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
                    ViewBag.WorkerId = new SelectList(workerManager.ListQueryable().Where(x => x.ServiceId == CurrentSession.Personel.ServiceId).ToList(), "Id", "Username", product.WorkerId);
                    return View(product);
                }

                CacheHelper.RemoveProductsFromCache();
                return Redirect(returnUrl);
            }
            ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Include(x => x.Products).
                        Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) && x.Products.Count == 0).OrderByDescending
                        (x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
            ViewBag.WorkerId = new SelectList(workerManager.ListQueryable().Where(x => x.ServiceId == CurrentSession.Personel.ServiceId).ToList(), "Id", "Username", product.WorkerId);

            return View(product);
        }

        public ActionResult PersonelEdit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Product product = productManager.Find(x => x.Id == id);
            if (product == null)
            {
                return View("Error", errorNotifyObj);
            }

            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", product.ServiceId);
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersonelEdit(Product product)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                //product.ServiceId = CurrentSession.Personel.ServiceId;               

                if (productManager.PersonelUpdate(product) > 0)
                {

                    CacheHelper.RemoveProductsFromCache();
                    return RedirectToAction("PersonelIndex");
                }
                return View(product);

            }
            //ViewBag.CustomerId = new SelectList(customerManager.ListQueryable().Where(x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) || x.Products.Count == 0).OrderByDescending(
            //    x => x.ModifiedOn).ToList(), "Id", "Tel", product.CustomerId);
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", product.ServiceId);

            return View(product);
        }

        [AuthAdmin]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Product product = productManager.Find(x => x.Id == id);
            if (product == null)
            {
                return View("Error", errorNotifyObj);
            }
            ViewBag.returnUrl = Request.UrlReferrer;
            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Product product = productManager.Find(x => x.Id == id);
            productManager.Delete(product);

            CacheHelper.RemoveProductsFromCache();
            return Redirect(returnUrl);
            //return RedirectToAction("AllProducts");
        }

        [AuthAdmin]
        public ActionResult AllProducts(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Include(p => p.Cargo).Include(p => p.Fee).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products.OrderByDescending(x=>x.ModifiedOn).ThenBy(x=>x.Fee.ModifiedOn).ThenBy(x=>x.Cargo.ModifiedOn).ToList());
        }

        [AuthAdmin]
        public ActionResult FreeProducts(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId && x.Warranty == true);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products.ToList());
        }

        [AuthAdmin]
        public ActionResult PaidForProducts(string searchString)
        {
            //List<Product> product_id = productManager.List();
            //List<int> required_ids = product_id.Select(x => x.Id).ToList();
            ////listquarable ile değil list ile dene
            //var temp = (from i in feeManager.List() where required_ids.Contains(i.ProductId) select i).ToList();
            //var products = from i in temp
            //               join p in productManager.List() on i.ProductId equals p.Id
            //               orderby p.ModifiedOn descending
            //               select new ProductFee(i,p);
            var products = productManager.List().Join(feeManager.List(),
                    pro => pro.Id,
                    fee => fee.ProductId,
                    (pro, fee) => new ProductFee(pro, fee)).Where(x => x.Product.ServiceId == CurrentSession.Personel.ServiceId);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Product.Imei.Contains(searchString));
            }
            return View(products);
        }

        [AuthAdmin]
        public ActionResult OnWay(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId && x.AtService == false||x.Fee!=null);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products);
        }

        [AuthAdmin]
        public ActionResult Completed(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId && x.IsRepaired == true);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products.ToList().OrderByDescending(x=>x.IsRepaired).ThenBy(x => x.ModifiedOn));
        }

        [AuthAdmin]
        public ActionResult Denied(string searchString)
        {
            var products = productManager.ListQueryable().Include(p => p.Owner).Include(p => p.Reports).Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId && x.Fee.IsDenied==true);

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.Imei.Contains(searchString));
            }
            return View(products.ToList());
        }

    }
}
