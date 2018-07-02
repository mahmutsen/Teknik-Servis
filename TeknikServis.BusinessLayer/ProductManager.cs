using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.BusinessLayer.Abstract;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.Entities;
using TeknikServis.Entities.Messages;
using TeknikServis.Entities.ViewModelObjects;

namespace TeknikServis.BusinessLayer
{
    public class ProductManager:ManagerBase<Product>
    {
        public new BusinessLayerResult<Product> Insert(Product data)
        {
            Product product = Find(x => x.Imei == data.Imei);
            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            res.Result = data;

            if (product!=null)
            {
                res.AddError(ErrorMessageCode.DeviceAlreadyExist, "Cihaz imei numarası kayıtlı.");

            }
            else
            {
                if (base.Insert(res.Result)==0)
                {
                    res.AddError(ErrorMessageCode.DeviceCouldNotInserted, " Cihaz kayıt edilemedi.");
                }
            }

            return res;
        }

        public new BusinessLayerResult<Product> Update(Product data)
        {
            Product db_product = Find(x => x.Imei == data.Imei);
            BusinessLayerResult<Product> res = new BusinessLayerResult<Product>();

            res.Result = data;

            if (db_product != null&&db_product.Id!=data.Id)
            {
                res.AddError(ErrorMessageCode.DeviceAlreadyExist, "Cihaz imei numarası kayıtlı.");
                return res;
            }
            else
            {
                res.Result = Find(x => x.Id == data.Id);
                res.Result.Imei = data.Imei;
                res.Result.CustomerId = data.CustomerId;
                res.Result.ServiceId = data.ServiceId;
                res.Result.WorkerId = data.WorkerId;
                res.Result.Problems = data.Problems;
                res.Result.Warranty = data.Warranty;
                res.Result.IsRepaired = data.IsRepaired;
                res.Result.AtService = data.AtService;
                //res.Result.Owner.IsActive = data.Owner.IsActive;
                if (base.Update(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.DeviceCouldNotInserted, " Cihaz kayıt edilemedi.");
                }
            }

            return res;
        }

        public int PersonelUpdate(Product data)
        {
            Product db_product = Find(x => x.Imei == data.Imei);

            //db_product.Owner.IsActive = data.Owner.IsActive;
            //db_product.Imei = data.Imei;
            //db_product.CustomerId = data.CustomerId;
            //db_product.ServiceId = data.ServiceId;
            //db_product.Problems = data.Problems;
            //db_product.Warranty = data.Warranty;
            //db_product.WorkerId = data.WorkerId;
            //db_product.CargoId = data.CargoId;
            db_product.IsRepaired = data.IsRepaired;
            //db_product.AtService = data.AtService;
            return (base.Update(db_product));
        }

    }
}
