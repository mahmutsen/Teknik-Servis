using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeknikServis.Common;
using TeknikServis.Entities;
using TeknikServis.WebApp.Models;

namespace TeknikServis.WebApp.Init
{
    public class WebCommon : ICommon //  UI web değil de başka birşey olursa ICommon u implemente eden başka bir sınıf oluştururuz
    {
        public string GetCurrentWorkerUName()
        {
            Worker personel = CurrentSession.Personel;

            if (personel != null)
                return personel.Username;
            else
                return "system";
        }
    }
}