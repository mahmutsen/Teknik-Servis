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
    public class CargoManager:ManagerBase<Cargo>
    {

        public new BusinessLayerResult<Cargo> Insert(Cargo data)
        {
            Cargo cargo = Find(x => x.ProductId == data.ProductId);
            BusinessLayerResult<Cargo> res = new BusinessLayerResult<Cargo>();
            res.Result = data;

            if (cargo != null)
            {
                res.AddError(ErrorMessageCode.CargoCouldNotInserted, "Bu cihaz için daha önce kargo bilgisi belirlendi");
            }
            else
            {
                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.CargoCouldNotInserted, "Kargo bilgisi eklenemedi");
                }
                else
                {
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");

                    string body = $"Merhaba Değerli Müşterimiz <br><br>{res.Result.Product.Imei} no lu cihazınız adresinize teslim edilmek üzere kargoya verilmiştir." +
                        $"<br><strong> Kargo bilgileriniz;</strong>" +
                        $"<br><br><b>Firma:<i>{res.Result.Firm}</i> Kargo. Takip Numarası:<i>{res.Result.TrackerNo}</i>.</b>";   

                    MailHelper.SendMail(body, res.Result.Product.Owner.Email, "Cihaz Kargo Takip");
                }
            }

            return res;
        }

        public BusinessLayerResult<Cargo> Update(Cargo data,int? ctrl)
        {
            Cargo db_cargo = Find(x => x.ProductId == data.ProductId);
            BusinessLayerResult<Cargo> res = new BusinessLayerResult<Cargo>();

            res.Result = data;

            if (db_cargo == null)
            {
                res.AddError(ErrorMessageCode.CargoDoesNotExist, "Güncellemek istediğiniz kargo bilgisi bulunamadı");
                return res;
            }
            else
            {
                res.Result = Find(x => x.ProductId == data.ProductId);
                res.Result.ProductId = data.ProductId;
                res.Result.Firm = data.Firm;
                res.Result.TrackerNo = data.TrackerNo;
                res.Result.IsArrived = data.IsArrived;

                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.CargoCouldNotInserted, "Kargo bilgisi güncellenemedi");
                }
                else
                {
                    string siteUri = ConfigHelper.Get<string>("SiteRootUri");
                    string queryUri = $"{siteUri}/Home/CustomerQuery";
                    string bankUri = $"{siteUri}/Home/Accounts";
                    string body = $"Merhaba Değerli Müşterimiz <br><b>{res.Result.Product.Imei}</b> no lu cihazınız için daha önce belirtmiş olduğumuz <b><strong>Kargo</strong> bilgileri konusunda bir hata olduğu saptandı.</b>" +
                        $"<br><br> <strong>Tarafımızca yapılan hatanın düzeltilmesi sonucu cihaz <b>Kargo bilgisi</b>; Firma:<b> {res.Result.Firm}</b>, Kargo Takip No:<b> {res.Result.TrackerNo}</b></strong> olarak güncellendi." +
                        $"<br><br> <i>Hatamız için özür diler ve anlayışınız için teşekkür ederiz. </i>";

                    MailHelper.SendMail(body, res.Result.Product.Owner.Email, "Kargo Bilgisi Düzeltme");
                }
            }

            return res;
        }
    }
}
