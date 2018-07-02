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
    [Table("Products")]
    public class Product : MyEntityBase
    {
        [DisplayName("Cihaz Imei No"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(15, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Imei { get; set; }

        [DisplayName("Sorunlar")]
        public string Problems { get; set; }

        [DisplayName("Onarıldı")]
        public bool IsRepaired { get; set; }

        [DisplayName("Serviste")]
        public bool AtService { get; set; }

        [DisplayName("Müşteri")]
        public int CustomerId { get; set; }

        [DisplayName("Servis")]
        public int ServiceId { get; set; }

        [DisplayName("Sorumlu Personel")]
        public int? WorkerId { get; set; }

        //[DisplayName("Borç")]
        //public int FeeId { get; set; }

        [DisplayName("Ucret")]
        public virtual Fee Fee { get; set; }

        [DisplayName("Kargo")]
        public virtual Cargo Cargo { get; set; }

        //[DisplayName("Müşteri"), Required(ErrorMessage = "{0} alanı gereklidir.")]
        public virtual Customer Owner { get; set; }

        //[DisplayName("Servis")]
        public virtual Service Service { get; set; }

        public virtual Worker Personel { get; set; }

        [DisplayName("Raporlar")]
        public virtual List<Report> Reports  { get; set; }

        [DisplayName("Garanti Kapsamı")]
        public bool Warranty { get; set; }

        public Product()
        {
            Warranty = false;
            Personel = null;
            Reports = new List<Report>();
            //Service = new Service();
        }
    }
}
