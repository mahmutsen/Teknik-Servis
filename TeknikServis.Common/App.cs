using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Common //Dışarıdan erişim için bu class kullanılacak
{
    public static class App
    {
        public static ICommon Common = new DefaultCommon(); //Static olduğu için nesnesi oluşturulamaz.İlk çağrıldığında başka bir değer atanmazsa DefaultCommon GetWorkerName metodundaki system atanır
    }
}
