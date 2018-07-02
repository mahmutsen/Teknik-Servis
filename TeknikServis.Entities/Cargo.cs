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
    [Table("Cargos")]
    public class Cargo
    {
        [Key, ForeignKey("Product"), DisplayName("Cihaz")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "{0} Alanı Gereklidir."), DisplayName("Kargo Firması")]
        public string Firm { get; set; }

        [Required(ErrorMessage = "{0} Alanı Gereklidir."),DisplayName("Takip Numarası")]
        public string TrackerNo { get; set; }

        [DisplayName("Ulaştı")]
        public bool IsArrived { get; set; }
        
        public virtual Product Product { get; set; }

        [DisplayName("Oluşturulma"), ScaffoldColumn(false), Required]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Güncellenme"), ScaffoldColumn(false), Required]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Güncelleyen"), ScaffoldColumn(false), Required, StringLength(30)]
        public string ModifiedUserName { get; set; }
    }
}
