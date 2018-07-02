using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknikServis.WebApp.ViewModels
{
    public class InformViewModel:NotifyViewModelBase<string>
    {
        public InformViewModel()
        {
            Title = "Bilgilendirme";
        }
    }
}