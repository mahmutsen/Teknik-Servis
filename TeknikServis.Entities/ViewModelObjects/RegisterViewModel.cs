using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TeknikServis.Entities.ViewModelObjects
{
    public class RegisterViewModel
    {
        [DisplayName("İsim")]
        public string Name { get; set; }

        [DisplayName("Soyad")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DisplayName("Telefon"), StringLength(15, ErrorMessage = "{0} maks. {1} Karakter Olmalı")]
        public string Tel { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"),DisplayName("Email"), StringLength(70, ErrorMessage = "{0} maks. {1} Karakter Olmalı")]
        public string Email { get; set; }

        [DisplayName("Şehir"), StringLength(25,ErrorMessage ="{0} maks. {1} Karakter Olmalı")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DisplayName("Adres")]
        public string Adress { get; set; }

        //Product bilgisi
        [Required(ErrorMessage = "{0} Alanı Boş Geçilemez"), DisplayName("Imei Numarası"), StringLength(15, ErrorMessage = "{0} Imei Numarası {1} Karakter Olmalı")]
        public string Imei { get; set; }

        public string Problems { get; set; }
        //public Service Service { get; set; }

        [Display(Name = "Services")]
        public int ServiceId { get; set; }

        public IEnumerable<SelectListItem> ServiceChoices { get; set; }
        
    }
}