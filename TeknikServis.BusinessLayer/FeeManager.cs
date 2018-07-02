using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.BusinessLayer.Abstract;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.Common.Helpers;
using TeknikServis.Entities;
using TeknikServis.Entities.Messages;

namespace TeknikServis.BusinessLayer
{
    public class FeeManager : ManagerBase<Fee>
    {
        public new BusinessLayerResult<Fee> Insert(Fee data)
        {
            Fee fee = Find(x => x.ProductId == data.ProductId);
            BusinessLayerResult<Fee> res = new BusinessLayerResult<Fee>();
            res.Result = data;

            if (fee != null)
            {
                res.AddError(ErrorMessageCode.DeviceAlreadyExist, "Bu cihaz için daha önce ücret belirlendi");
            }
            else
            {
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.FeeCouldNotInserted, "Ücret eklenemedi");
                }
                else
                {
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string queryUri = $"{siteUri}/Home/CustomerQuery";
                    string bankUri = $"{siteUri}/Home/Accounts";
                    string body = $"Merhaba Değerli Müşterimiz <br>{res.Result.Product.Imei} no lu cihazınızın garanti kapsamı dışında olduğu saptandı ve bakım/onarım ücreti; <b>{res.Result.Value} TL </b> olarak belirlendi." +
                        $"<br><br> Ücretli hizmetimizden faydalanmak isterseniz, 7 iş günü içerisinde <b>{res.Result.Product.Service.Title} Servisinin</b> hesabına ödemeyi gerçekleştirmeniz rica olunur." +
                        $"<br><br> Servis hesap bilgilerine <b><a href='{bankUri}' target='_blank'>adresinden</a></b> ulaşabilirsiniz." +
                        $"<br><br> İptal talebinde bulunmak isterseniz <b><a href='{queryUri}' target='_blank'>cihaz takip sayfasından</a></b> ya da {res.Result.Product.Service.Tel}'i arayarak gerçekleştirebilirsiniz.   " +
                        $"<br><br><strong>7 iş günü içerisinde</strong> işlem yapılmaması(ödeme veya iptal talebi) durumunda hizmetimiz sonlandıralarak cihazınız kayıt sırasında belirtmiş olduğunuz adrese kargolanacak ve bilgilendirme postası alacaksınız.İyi günler Dileriz.";

                    MailHelper.SendMail(body, res.Result.Product.Owner.Email, "Cihaz Garanti Durumu");
                }
            }

            return res;
        }

        public  BusinessLayerResult<Fee> FullUpdate(Fee data)
        {
            Fee db_fee = Find(x => x.ProductId == data.ProductId);
            BusinessLayerResult<Fee> res = new BusinessLayerResult<Fee>();

            res.Result = data;

            if (db_fee==null)
            {
                res.AddError(ErrorMessageCode.FeeDoesNotExist, "Güncellemek istediğiniz ücret bilgisi bulunamadı");
                return res;
            }
            else
            {
                res.Result = Find(x => x.ProductId == data.ProductId);
                res.Result.ProductId = data.ProductId;
                res.Result.Value = data.Value;
                res.Result.Statement = data.Statement;
                res.Result.IsPaid = data.IsPaid;
                res.Result.IsDenied = data.IsDenied;


                if (base.Update(res.Result)==0)
                {
                    res.AddError(ErrorMessageCode.FeeCouldNotInserted, "Ücret bilgisi güncellenemedi");
                }
                else
                {
                    //string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    //string queryUri = $"{siteUri}/Home/CustomerQuery";
                    //string bankUri = $"{siteUri}/Home/Accounts";
                    //string body = $"Merhaba Değerli Müşterimiz <br><b>{res.Result.Product.Imei}</b> no lu cihazınız için daha önce belirtmiş olduğumuz <b>ücret konusunda bir hata olduğu saptandı.</b>" +
                    //    $"<br><br> <strong>Tarafımızca yapılan hatanın düzeltilmesi sonucu cihaz <b>bakım/onarım ücreti; <b>{res.Result.Value} TL </b> olarak belirlendi." +
                    //    $"<br> Hatamız için özür diler ve anlayışınız için teşekkür ederiz. </strong>" +
                    //    $"<br><br> Ücretli hizmetimizden faydalanmak isterseniz, 7 iş günü içerisinde <b>{res.Result.Product.Service.Title} Servisinin</b> hesabına ödemeyi gerçekleştirmeniz rica olunur." +
                    //    $"<br><br> Servis hesap bilgilerine <b><a href='{bankUri}' target='_blank'>adresinden</a></b> ulaşabilirsiniz." +
                    //    $"<br><br> İptal talebinde bulunmak isterseniz <b><a href='{queryUri}' target='_blank'>cihaz takip sayfasından</a></b> ya da {res.Result.Product.Service.Tel}'i arayarak gerçekleştirebilirsiniz.   " +
                    //    $"<br><br><strong>7 iş günü içerisinde</strong> işlem yapılmaması(ödeme veya iptal talebi) durumunda hizmetimiz sonlandıralarak cihazınız kayıt sırasında belirtmiş olduğunuz adrese kargolanacak ve bilgilendirme postası alacaksınız.İyi günler Dileriz.";

                    //MailHelper.SendMail(body, res.Result.Product.Owner.Email, "Ücret Bilgisi Düzeltme");
                }
            }

            return res;
        }

        public BusinessLayerResult<Fee> FeeDenied(Product product)
        {
            BusinessLayerResult<Fee> res = new BusinessLayerResult<Fee>();

            res.Result = Find(x => x.ProductId == product.Id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.FeeDoesNotExist, "Ücret bilgisi bulunamadı");
            }
            else
            {
                res.Result.IsDenied = true;
            }

            return res;
        }
    }
}
