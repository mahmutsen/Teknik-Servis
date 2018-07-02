using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Entities.ViewModelObjects
{
    public class QueryViewModel
    {
        [DisplayName("Cihaz Imei No"), StringLength(15)]
        public string İmeiNo { get; set; }

        [DisplayName("Form No"),StringLength(40)]
        public string FormNo { get; set; }

        [DisplayName("E-mail"), StringLength(60)]
        public string Email { get; set; }
    }
}
