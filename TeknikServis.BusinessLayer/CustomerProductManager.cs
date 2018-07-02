using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.BusinessLayer.Abstract;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.Common.Helpers;
using TeknikServis.DataAccessLayer.EntityFramework;
using TeknikServis.Entities;
using TeknikServis.Entities.Messages;
using TeknikServis.Entities.ViewModelObjects;

namespace TeknikServis.BusinessLayer
{
    public class CustomerProductManager : ManagerBase<Product>
    {
        //private Repository<Customer> repo_customer = new Repository<Customer>();
        //private Repository<Product> repo_product = new Repository<Product>();

        public BusinessLayerResult<Product> RegisterCustomerProduct(RegisterViewModel data)
        {
            //ürün imei  kontrolü
            //kayıt işlemi
            //Aktivasyon e-postası Gönderimi
            Product product = Find(x => x.Imei == data.Imei);

            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();
            if (product != null)
            {

                res.AddError(ErrorMessageCode.DeviceAlreadyExist, "Bu Cihaz sistemde kayıtlı");

            }
            else
            {
                //ServiceManager sm = new ServiceManager();

                int dbResult = Insert(new Product()
                {

                    Imei = data.Imei,
                    Problems = data.Problems,
                    //Service=new ServiceManager().GetAllService().Where(x=>data.ServiceId.Contains(x.Id)).FirstOrDefault()
                    //Service=sm.List().Where(x=>x.Id==data.ServiceId).FirstOrDefault(),
                    ServiceId = data.ServiceId,
                    Warranty = false,
                    IsRepaired=false,
                    AtService=false,
                    Owner = new Customer()
                    {
                        Name = data.Name,
                        Surname = data.Surname,
                        Email = data.Email,
                        Tel = data.Tel,
                        City = data.City,
                        Adress = data.Adress,
                        FormNo = Guid.NewGuid(),
                        IsActive = false,
                        ModifiedUserName = "system",
                        CreatedOn = DateTime.Now,
                        ModifiedOn = DateTime.Now
                    }
                });
                //Customer customer = repo_customer.Find(x => x.Tel == data.Tel && x.Email == data.Email);
                //customer.Products.Add(product);   //Aşağıdaki kod hata verirse alternatif olarak

                if (dbResult > 0)// Burda hata é!!!!!!!
                {
                    res.Result = Find(x => x.Imei == data.Imei);

                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string queryUri = $"{siteUri}/Home/CustomerQuery";
                    string body = $"Merhaba {res.Result.Owner.Tel} no lu müşterimiz <br><br>Cihaz durum takibinizi <a href='{queryUri}' target='_blank'>adresinden</a> <br><br><b>{res.Result.Owner.FormNo}</b> Takip Numarası ile gerçekleştirebilirsiniz.";

                    MailHelper.SendMail(body, res.Result.Owner.Email, "Cihaz Takip Numarası");
                    //layerResult.Result = repo_customer.Find(x => x.Products.Find(y => y.Imei == data.Imei).Imei == data.Imei);
                    //layerResult.Result = repo_customer.Find(x => x.Products.);
                    //TODO
                    //layerResult.Result.ActivateFormGuid
                }
            }

            return res;
        }

        public BusinessLayerResult<Product> LoginCustomerProduct(QueryViewModel data)
        {

            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();
            res.Result = Find(x => x.Imei == data.İmeiNo || x.Owner.FormNo.ToString() == data.FormNo);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.DeviceCouldNotFound, "Cihaz Bulunamadı");
                //if (!res.Result.Owner.IsActive) //Personel aktif edilmemişse
                //{
                //    res.AddError(ErrorMessageCode.DeviceCouldNotFound, "Cihaz Bulunamadı");
                //}
            }

            return res;
            
        }

        public BusinessLayerResult<Product> CustomerProductById(int id)
        {
            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.DeviceCouldNotFound, "Cihaz Bulunamadı");
            }
            return res;
        }


        public BusinessLayerResult<Product> SuspendCustomerProduct(int id)
        {
            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            res.Result = Find(x => x.CustomerId == id);

            if (res.Result==null)
            {
                res.AddError(ErrorMessageCode.SessionCouldNotFound, "Oturum bulunamadı.");
            }
            else
            {
                res.Result.Owner.IsActive = false;
            }

            return res;
        }
    }
}
