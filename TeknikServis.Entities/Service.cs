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
    [Table("Services")]
    public class Service:MyEntityBase
    {
        [DisplayName("Servis"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(40, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Title { get; set; }

        [DisplayName("Şehir"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(30, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string City { get; set; }

        [DisplayName("Adres"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(100, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Adress { get; set; }

        [DisplayName("E-posta"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(70, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Email { get; set; }

        [DisplayName("Telefon"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(20, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Tel { get; set; }

        [DisplayName("Fax"), StringLength(25, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Fax { get; set; }

        [DisplayName("Özet"), Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(150, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Text { get; set; }

        [DisplayName("Kategori Id"), Required(ErrorMessage = "{0}  alanı gereklidir")]
        public int CategoryId { get; set; }// Kategori Id sine ulaşmak istedğimizde aşağıda ki virtual prop. ile fazladan sorgu çalıştırmak yerine EFCodeFirst teki bu yapıyla yolu kısaltmış olduk.(DatabaseFirst ile de yapılabilirdi)

        public virtual Category Category { get; set; }

        [DisplayName("Personeller")]
        public virtual List<Worker> Workers { get; set; } // Bir Servisin Çalışanları

        [DisplayName("Cihazlar")]
        public virtual List<Product> Products { get; set; }

        [DisplayName("Banka Hesaplar")]
        public virtual List<BankAccount> BankAccounts{ get; set; }

        [DisplayName("Banka Hesaplar")]
        public virtual List<Stock> Stocks { get; set; }

        public Service()
        {
            Workers = new List<Worker>();
            Products = new List<Product>();
            BankAccounts = new List<BankAccount>();
            Stocks = new List<Stock>();
        }
    }
}
