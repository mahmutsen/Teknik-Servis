using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.WebApp.Models;

namespace TeknikServis.WebApp.Filters
{
    public class AuthAdminOrManager : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.Personel != null && CurrentSession.Personel.IsAdmin == true&&CurrentSession.Personel.IsManager==true)
            {
                filterContext.Result = new RedirectResult("/Home/AccessDenied");
            }
        }
    }
}