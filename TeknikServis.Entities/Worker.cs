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
    [Table("Workers")]
    public class Worker : MyEntityBase
    {
        [Required,DisplayName("Servis")]
        public int ServiceId { get; set; }

        [DisplayName("İsim"),
            StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyisim"),
            StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Surname { get; set; }

        [DisplayName("Kullanıcı Adı"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Username { get; set; }

        [DisplayName("E-posta"),
            Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(70, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şifre"), Required(ErrorMessage = "{0} alanı gereklidir."),
            StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Password { get; set; }

        [StringLength(50),ScaffoldColumn(false)]
        public string PersonelImage { get; set; }

        [DisplayName("Aktif")]
        public bool IsActive { get; set; }//Varsayılan hali boşgeçilemez)

        [DisplayName("Yönetici")]
        public bool IsAdmin { get; set; }

        [DisplayName("Site Yöneticisi")]
        public bool IsManager { get; set; }

        [Required,ScaffoldColumn(false)]
        public Guid ActivateGuid { get; set; }

        public virtual Service Service { get; set; }//Çalışan bağlı olduğu teknik servis merkezi
        public virtual List<Product> Products{ get; set; }
        public virtual List<Report> Reports { get; set; }

        public Worker()
        {
            Products = new List<Product>();
            Reports = new List<Report>();
        }
    }
}
