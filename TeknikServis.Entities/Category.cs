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
    [Table("Categories")]
    public class Category:MyEntityBase
    {
        [DisplayName("Marka"),
            Required(ErrorMessage ="{0} isimli alanı gereklidir"),
            StringLength(30,ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Title { get; set; }

        [DisplayName("Açıklama"), StringLength(100, ErrorMessage = "{0}  alanı maks. {1} karakter içermelidir")]
        public string Description { get; set; }

        [DisplayName("Servisler")]
        public virtual List<Service> Services { get; set; }

        [DisplayName("Fiyatlandırmalar")]
        public virtual List<Pricing> Pricings { get; set; }

        [StringLength(50), ScaffoldColumn(false)]
        public string CategoryImage { get; set; }

        public Category()
        {
            Services = new List<Service>(); // Category oluşturulduktan sonra service ler eklemek istediğimizde, null hatası almamak için contructorına sevicelerle birlikte oluşturduk
            Pricings = new List<Pricing>();
        }
    }
}
