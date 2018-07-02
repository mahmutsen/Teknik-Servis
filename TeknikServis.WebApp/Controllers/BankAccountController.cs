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
    public class BankAccountController : Controller
    {
        private BankAccountManager bankAccountManager = new BankAccountManager();
        private ServiceManager serviceManager = new ServiceManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        public ActionResult Index()
        {
            var bankAccounts = bankAccountManager.ListQueryable().Include(b => b.Service).Where(x=>x.ServiceId==CurrentSession.Personel.ServiceId).OrderByDescending(x=>x.ModifiedOn);
            return View(bankAccounts.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            BankAccount bankAccount = bankAccountManager.Find(x=>x.Id==id);
            if (bankAccount == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(bankAccount);
        }

        public ActionResult Create()
        {
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BankAccount bankAccount)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                bankAccount.ServiceId = CurrentSession.Personel.ServiceId;
                bankAccountManager.Insert(bankAccount);
                return RedirectToAction("Index");
            }

            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", bankAccount.ServiceId);
            return View(bankAccount);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }
            BankAccount bankAccount = bankAccountManager.Find(x => x.Id == id);
            if (bankAccount == null)
            {
                return View("Error", errorNotifyObj);
            }
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", bankAccount.ServiceId);
            return View(bankAccount);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BankAccount bankAccount)
        {
            ModelState.Remove("CreatedOn");
            ModelState.Remove("ModifiedOn");
            ModelState.Remove("ModifiedUserName");

            if (ModelState.IsValid)
            {
                BankAccount db_bankAccount = bankAccountManager.Find(x => x.Id == bankAccount.Id);

                db_bankAccount.ServiceId = CurrentSession.Personel.ServiceId;
                db_bankAccount.IBAN = bankAccount.IBAN;
                db_bankAccount.AccountName = bankAccount.AccountName;
                db_bankAccount.Bank = bankAccount.Bank;
                db_bankAccount.AccountNo = bankAccount.AccountNo;
                bankAccountManager.Update(db_bankAccount);

                return RedirectToAction("Index");
            }
            //ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", bankAccount.ServiceId);
            return View(bankAccount);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return View("Error", errorNotifyObj);
            }

            BankAccount bankAccount = bankAccountManager.Find(x => x.Id == id);
            if (bankAccount == null)
            {
                return View("Error", errorNotifyObj);
            }
            return View(bankAccount);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BankAccount bankAccount = bankAccountManager.Find(x => x.Id == id);

            bankAccountManager.Delete(bankAccount);
            return RedirectToAction("Index");
        }

    }
}
