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
    [Table("Pricings")]
    public class Pricing:MyEntityBase
    {
        [Required(ErrorMessage = "{0} alanı gereklidir."),DisplayName("Arıza Tipi"), StringLength(30, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string DefectType { get; set; }

        [DisplayName("Fiyat")]
        public string Price { get; set; }

        [DisplayName("Kategori Id"),Required(ErrorMessage = "{0} alanı gereklidir.")]
        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
