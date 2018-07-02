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
    [Table("Stocks")]
    public class Stock:MyEntityBase
    {
        [DisplayName("Parça"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(30, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Name { get; set; }

        [DisplayName("Servis")]
        public int ServiceId { get; set; }

        [Required,DisplayName("Adet")]
        public int Quantity { get; set; }

        [DisplayName("Alış Fiyat")]
        public int BPrice { get; set; }

        [DisplayName("Servis")]
        public virtual Service Service { get; set; }
    }
}
