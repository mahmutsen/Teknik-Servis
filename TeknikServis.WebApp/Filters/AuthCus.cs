using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TeknikServis.WebApp.Models;

namespace TeknikServis.WebApp.Filters
{
    public class AuthCus : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.Product==null)
            {
                filterContext.Result = new RedirectResult("/Home/CustomerQuery");
            }
        }
    }
}