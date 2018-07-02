using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using TeknikServis.BusinessLayer;
using TeknikServis.Entities;

namespace TeknikServis.WebApp.Models
{
    public class CacheHelper
    {
        //Her Product Eklendiğinde/Edilendiğinde ya da silindiğinde Cache i temizle!
        public static IQueryable<Product> GetProductsFromCache() //Product Controllerinda da kullanıcam!
        {
            var result= WebCache.Get("product-cache"); //Cache doluysa 

            if (result==null)
            {
                ProductManager productManager  = new ProductManager();

                result= productManager.ListQueryable().Include("Service").Where(
                    x => x.ServiceId == CurrentSession.Personel.ServiceId).OrderByDescending(
                    x => x.ModifiedOn);

                WebCache.Set("product-cache", result,40, true);
            }

            return result;
        }


        public static void RemoveProductsFromCache()
        {
            Remove("product-cache");
        }

        public static void Remove(string key)
        {
            WebCache.Remove(key);
        }
    }
}