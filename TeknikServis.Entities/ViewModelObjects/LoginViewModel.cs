using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TeknikServis.Entities.ViewModelObjects
{ 
    public class LoginViewModel
    {
        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),DisplayName("Servis Numarası")]
        public int ServiceId { get; set; }

        [Required(ErrorMessage ="{0} Alanı Boş Geçilemez"),DisplayName("E-posta"), StringLength(70),EmailAddress(ErrorMessage ="Lütfen geçerli bir e-posta adresi girin")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DisplayName("Şifre"), DataType(DataType.Password), StringLength(25)]
        public string Password { get; set; }
    }
}