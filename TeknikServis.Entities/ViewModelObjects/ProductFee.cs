using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Entities.ViewModelObjects
{
    public class ProductFee
    {
        public ProductFee(Product product,Fee fee)
        {
            Product= product;
            Fee = fee;
        }
        public Product Product { get; set; }
        public Fee Fee { get; set; }
    }
}
