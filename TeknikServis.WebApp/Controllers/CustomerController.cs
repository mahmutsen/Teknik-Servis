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
    [AuthAdmin]
    [Exc]
    public class CustomerController : Controller
    {
        private CustomerManager customerManager = new CustomerManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            //var customers = customerManager.ListQueryable().Include(p=>p.Products).OrderByDescending(x => x.ModifiedOn);
            var customers = customerManager.ListQueryable().Include(p => p.Products).Where(
                x => x.Products.Any(y => y.ServiceId == CurrentSession.Personel.ServiceId) || x.Products.Count == 0).OrderByDescending(x => x.ModifiedOn);
            return View(customers.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Customer customer = customerManager.Find(x=>x.Id==id);
            if (customer == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(customer);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Customer customer)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                customer.IsActive=true;
                customer.FormNo = Guid.NewGuid();

                customerManager.Insert(customer);
                return RedirectToAction("Index");
            }

            return View(customer);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Customer customer = customerManager.Find(x => x.Id == id);
            if (customer == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Customer customer)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                Customer db_customer = customerManager.Find(x => x.Id == customer.Id);
                db_customer.Name = customer.Name;
                db_customer.Surname = customer.Surname;
                db_customer.Tel = customer.Tel;
                db_customer.Email = customer.Email;
                db_customer.District = customer.District;
                db_customer.City = customer.City;
                db_customer.Adress = customer.Adress;
                db_customer.IsActive = customer.IsActive;
                //db_customer.Fee = customer.Fee;

                customerManager.Update(customer);
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            Customer customer = customerManager.Find(x => x.Id == id);
            if (customer == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(customer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = customerManager.Find(x => x.Id == id);
            customerManager.Delete(customer);
            return RedirectToAction("Index");
        }

    }
}
