using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TeknikServis.BusinessLayer;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.Entities;
using TeknikServis.Entities.Messages;
using TeknikServis.Entities.ViewModelObjects;
using TeknikServis.WebApp.Filters;
using TeknikServis.WebApp.Models;
using TeknikServis.WebApp.ViewModels;

namespace TeknikServis.WebApp.Controllers
{
    [Exc]
    public class HomeController : Controller
    {
        private CategoryManager categoryManager = new CategoryManager();
        private ServiceManager serviceManager = new ServiceManager();
        private WorkerManager workerManager = new WorkerManager();
        private CustomerProductManager customerProductManager = new CustomerProductManager();
        private BankAccountManager bankAccountManager = new BankAccountManager();
        private FeeManager feeManager = new FeeManager();
        private ErrorViewModel errorNotifyObj = new ErrorViewModel();

        // GET: Home
        public ActionResult Index()
        {
            BusinessLayer.Test test = new BusinessLayer.Test();
            return View();

            //return View(sm.GetAllService().OrderByDescending(x => x.ModifiedOn).ToList();
            //return View(sm.GetAllServiceQueryable().OrderByDescending(x => x.ModifiedOn).ToList()); //Bu sorgu farklı olarak oderbydescending ile birlikte sql sorgusu olarak çalıştırılacak
        }

        public ActionResult Services()
        {
            return View(serviceManager.ListQueryable().OrderByDescending(x => x.ModifiedOn).ToList());
        }

        public ActionResult GetServiceDetail(int? id)
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
            return PartialView("_PartialServiceDetail", service);
        }

        public ActionResult ByCategory(int? id)
        {
            if (id == null)
            {
                //return View("Error", errorNotifyObj);
            }

            Category cat = categoryManager.Find(x => x.Id == id.Value); // int? durumundan dolayı .value dedik

            if (cat == null)
            {
                //return View("Error", errorNotifyObj);
            }

            return View("Services", cat.Services.OrderByDescending(x => x.City).ToList());
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Accounts()
        {
            var result = bankAccountManager.ListQueryable().Include("Service").OrderBy(x => x.ServiceId);
            return View(result.ToList());
        }
        [Auth]
        public ActionResult ShowProfile()
        {

            BusinessLayerResult<Worker> res = workerManager.GetWorkerById(CurrentSession.Personel.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }
        [Auth]
        public ActionResult EditProfile()
        {

            BusinessLayerResult<Worker> res = workerManager.GetWorkerById(CurrentSession.Personel.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors
                };

                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        [Auth]
        [HttpPost]
        public ActionResult EditProfile(Worker model, HttpPostedFileBase ProfileImage)
        {
            ModelState.Remove("ModifiedUserName");
            ModelState.Remove("Username");

            if (ModelState.IsValid)
            {
                if (ProfileImage != null &&
                    (ProfileImage.ContentType == "image/jpeg" ||
                    ProfileImage.ContentType == "image/jpg" ||
                    ProfileImage.ContentType == "image/png"))
                {
                    string filename = $"user_{model.Id}.{ProfileImage.ContentType.Split('/')[1]}";

                    ProfileImage.SaveAs(Server.MapPath($"~/Images/{filename}"));
                    model.PersonelImage = filename;
                }

                BusinessLayerResult<Worker> res = workerManager.UpdateProfile(model);

                if (res.Errors.Count > 0)
                {
                    ErrorViewModel errorNotifyObj = new ErrorViewModel()
                    {
                        Title = "Hata Oluştu",
                        Items = res.Errors,
                        RedirectingUrl = "/Home/EditProfile"
                    };
                    return View("Error", errorNotifyObj);
                }

                CurrentSession.Set<Worker>("login", res.Result); //profil güncellendiğinden sesion güncellendi

                return RedirectToAction("ShowProfile");
            }

            return View(model);
        }

        public ActionResult Login()
        {
            var model = new LoginViewModel();
            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Worker> res = workerManager.LoginWorker(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
                    return View(model); //WorkerManager ın LoginWorker metodunda belirlediğimiz kurallara uygun değilse 
                }

                CurrentSession.Set<Worker>("login", res.Result); //Session da kullanıcı bilgi saklama

                if (CurrentSession.Personel.IsAdmin)
                {
                    return RedirectToAction("AllProducts", "Product");
                }
                else if (CurrentSession.Personel.IsManager)
                {
                    return RedirectToAction("Index", "Category");
                }
                else if (!(CurrentSession.Personel.IsAdmin&&CurrentSession.Personel.IsManager))
                {
                    return RedirectToAction("PersonelIndex", "Product");
                }


            }
            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");
            return View(model); //Model valid değilse
        }

        public ActionResult Logout()
        {
            CurrentSession.Remove("login");
            return RedirectToAction("Index");
        }

        //Service ListBoxunu doldur
        //private void PopulateServiceChoices(RegisterViewModel model)
        //{
        //    model.ServiceChoices = serviceManager.List().Select(m => new SelectListItem
        //    {
        //        Value = m.Id.ToString(),
        //        Text = m.Title
        //    });

        //}

        public ActionResult CustomerProductRegister(int? id)
        {
            var model = new RegisterViewModel();
            model.ServiceId=(int)id;
            //PopulateServiceChoices(model);
            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title");

            return View(model);
        }

        [HttpPost]
        public ActionResult CustomerProductRegister(RegisterViewModel model)
        {

            if (ModelState.IsValid)
            {

                BusinessLayerResult<Product> res = customerProductManager.RegisterCustomerProduct(model);
                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message)); // BusinessLayerResultdan dönen nesnenin varsa her bir error prop. ı için, modelstate e error mesajı ekle
                    //PopulateServiceChoices(model);
                    ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", model.ServiceId);
                    return View(model); //Hatalar varsa view a geri dönder
                }
                OkViewModel notifyObj = new OkViewModel()
                {
                    Title = "Kayıt Başarılı",
                    RedirectingUrl = "/Home/CustomerQuery",
                    RedirectingTimeOut = 5000
                };
                //notifyObj.Items.Add(res.Result.Owner.FormNo.ToString());
                notifyObj.Items.Add("Cihaz takip numarası e-posta adresinize gönderilmiştir. Cihazınız tarafımıza ulaşıp incelendikten sonra, ücret çıkması durumunda bilgilendirelecek ve onay vermeniz halinde hizmetimiz başlatılacaktır.");

                CacheHelper.RemoveProductsFromCache();//Product Controller oluşturunca ordaki insert edit delete işlemleri içinde yap!!
                return View("Ok", notifyObj);
            }

            //Aktivasyon e-poastası gönderimi Personel Kayıtda da yapılabilir.
            //PopulateServiceChoices(model);
            ViewBag.ServiceId = new SelectList(serviceManager.List(), "Id", "Title", model.ServiceId);

            return View(model);
        }


        //public ActionResult WorkerActivate(Guid activate_id)
        //{
        //    //Personel aktivasyonu sağlanır
        //    return View();
        //}

        public ActionResult CustomerQuery()
        {
            if (CurrentSession.Product == null)
            {
                var model = new QueryViewModel();
                return View();
            }
            return RedirectToAction("ShowInfo");
        }

        [HttpPost]
        public ActionResult CustomerQuery(QueryViewModel model)
        {
            if (ModelState.IsValid)
            {
                BusinessLayerResult<Product> res = customerProductManager.LoginCustomerProduct(model);

                if (res.Errors.Count > 0)
                {
                    res.Errors.ForEach(x => ModelState.AddModelError("", x.Message));
                    return View(model);
                }

                CurrentSession.Set<Product>("info", res.Result); //Session da kullanıcı bilgi saklama
                return RedirectToAction("ShowInfo");

            }
            return View(model); //Model valid değilse
        }

        [AuthCus]
        public ActionResult ShowInfo()
        {
            BusinessLayerResult<Product> res = customerProductManager.CustomerProductById(CurrentSession.Product.Id);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Hata Oluştu",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/CustomerQuery"
                };

                return View("Error", errorNotifyObj);
            }
            return View(res.Result);
        }

        //[AuthCus]
        //public ActionResult SuspendCustomer()
        //{
        //    BusinessLayerResult<Product> res = customerProductManager.SuspendCustomerProduct(CurrentSession.Product.CustomerId);

        //    if (res.Errors.Count>0)
        //    {
        //        ErrorViewModel errorNotifyObj = new ErrorViewModel()
        //        {
        //            Title = "Oturum Bulunamadı",
        //            Items = res.Errors,
        //            RedirectingUrl= "/Home/CustomerQuery"
        //        };

        //        return View("Error", errorNotifyObj);
        //    }
        //    customerProductManager.Update(res.Result);
        //    CacheHelper.RemoveProductsFromCache();
        //    return RedirectToAction("ShowInfo");
        //}

        [AuthCus]
        public ActionResult SuspendCustomer()
        {
            BusinessLayerResult<Fee> res = feeManager.FeeDenied(CurrentSession.Product);

            if (res.Errors.Count > 0)
            {
                ErrorViewModel errorNotifyObj = new ErrorViewModel()
                {
                    Title = "Oturum Bulunamadı",
                    Items = res.Errors,
                    RedirectingUrl = "/Home/CustomerQuery"
                };

                return View("Error", errorNotifyObj);
            }
            feeManager.Update(res.Result);
            CacheHelper.RemoveProductsFromCache();
            return RedirectToAction("ShowInfo");
        }

        public ActionResult CustomerLogout()
        {
            CurrentSession.Remove("info");
            return RedirectToAction("Index");
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        public ActionResult deneme()
        {
            BusinessLayer.Test test = new BusinessLayer.Test();
            return View();
        }

        public ActionResult HasError()
        {
            return View();
        }
    }
}