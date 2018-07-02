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
    [Table("BankAccounts")]
    public class BankAccount : MyEntityBase
    {
        [Required(ErrorMessage = "{0}  alanı gereklidir"), DisplayName("Hesap Adı"),StringLength(40, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "{0}  alanı gereklidir"), DisplayName("Banka Adı"), StringLength(30, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Bank { get; set; }

        [Required(ErrorMessage = "{0}  alanı gereklidir"), DisplayName("Hesap Numarası"), StringLength(30, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string AccountNo { get; set; }

        [Required(ErrorMessage = "{0}  alanı gereklidir"), StringLength(30, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string IBAN { get; set; }

        [Required(ErrorMessage = "{0}  alanı gereklidir"), DisplayName("Servis Id")]
        public int ServiceId { get; set; }

        public virtual Service Service { get; set; }

    }
}
