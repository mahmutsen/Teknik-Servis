using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Entities
{
    [Table("Reports")]
    public class Report:MyEntityBase
    {
        [DisplayName("Açıklama"), Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(150, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Text { get; set; }

        [DisplayName("Cihaz")]
        public int ProductId { get; set; }

        //[DisplayName("Personel")]
        //public int WorkerId { get; set; }

        //public virtual Service Service { get; set; }
        //[DisplayName("Cihaz")]
        public virtual Product Product { get; set; }

        [DisplayName("Personel")]
        public virtual Worker Worker { get; set; }
    }
}
