using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeknikServis.Entities;

namespace TeknikServis.WebApp.Models
{
    public class CurrentSession
    {
        public static Worker Personel
        {
            get
            {
                return Get<Worker>("login");
            }
        } //property newlenmedn çağrılabilecek

        public static Product Product
        {
            get
            {
                return Get<Product>("info");
            }
        }

        public static void Set<T>(string key,T obj)
        {
            HttpContext.Current.Session[key] = obj;
        }

        public static T Get<T>(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                return (T)HttpContext.Current.Session[key]; // Generic tipe cast et
            }

            return default(T); // in verildiyse 0, class verildiyse null, bool ise false, string ise null vs.
        }

        public static void Remove(string key)
        {
            if (HttpContext.Current.Session[key]!=null)
            {
                HttpContext.Current.Session.Remove(key);

            }
        }

        public static void Clear()
        {
            HttpContext.Current.Session.Clear();
        }

    }
}