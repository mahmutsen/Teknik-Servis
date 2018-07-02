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
    [Table("Customers")]
    public class Customer : MyEntityBase
    {
        [DisplayName("İsim"),StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Name { get; set; }

        [DisplayName("Soyisim"), StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Surname { get; set; }

        [DisplayName("Telefon"),DataType(DataType.PhoneNumber), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(20, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Tel { get; set; }

        [DisplayName("E-posta"),DataType(DataType.EmailAddress,ErrorMessage ="Lütfen Geçerli bir mail adresi girin"), Required(ErrorMessage = "{0} alanı gereklidir."), StringLength(70, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır.")]
        public string Email { get; set; }

        [DisplayName("Şehir")]
        public string City { get; set; }

        [DisplayName("İlçe")]
        public string District { get; set; }

        [DisplayName("Adres"), StringLength(70, ErrorMessage = "{0} alanı maks. {1} karakter olmalıdır."),Required(ErrorMessage = "{0} alanı gereklidir.")]
        public string Adress { get; set; }

        [ScaffoldColumn(false)]
        public Guid FormNo { get; set; }
        [DisplayName("Aktif")]
        public bool IsActive { get; set; }

        //public virtual Service service{ get; set; }
        public virtual List<Product> Products{ get; set; }

        public Customer()
        {
            Products = new List<Product>();
        }
    }
}