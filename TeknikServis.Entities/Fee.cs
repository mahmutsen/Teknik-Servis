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
    [Table("Fees")]
    public class Fee
    {
        //[Key,ForeignKey("Product")]
        //public int Id { get; set; }
        [Key,ForeignKey("Product"),DisplayName("Cihaz")]
        public int ProductId { get; set; }

        [DisplayName("Tutar"), Required(ErrorMessage = "{0} Alanı Gereklidir")]
        public int Value { get; set; }

        [Required(ErrorMessage = "{0} Alanı Gereklidir."), DisplayName("Ödendi")]
        public bool IsPaid { get; set; }

        [DisplayName("İptal")]
        public bool IsDenied { get; set; }

        [DisplayName("Açıklama")]
        public string Statement { get; set; }

        //[DisplayName("Cihaz")]
        //public int ProductId { get; set; }

        //[Required(ErrorMessage = "{0} Alanı Gereklidir.")]
        public virtual Product Product { get; set; }

        [DisplayName("Oluşturulma"), ScaffoldColumn(false), Required]
        public DateTime CreatedOn { get; set; }

        [DisplayName("Güncellenme"), ScaffoldColumn(false), Required]
        public DateTime ModifiedOn { get; set; }

        [DisplayName("Güncelleyen"), ScaffoldColumn(false), Required, StringLength(30)]
        public string ModifiedUserName { get; set; }
    }
}
