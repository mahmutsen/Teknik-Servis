using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TeknikServis.WebApp.ViewModels
{
    public class WarningViewItem:NotifyViewModelBase<string>
    {
        public WarningViewItem()
        {
            Title = "Uyarı";
        }
    }
}