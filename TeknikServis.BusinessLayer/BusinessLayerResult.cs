using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.BusinessLayer
{
    public class BusinessLayerResult<T> where T:class //Tek bir exception nesnesine bağlı kalmadan istediğimiz kadar Hata dönderebiliriz
    {
        public List<string> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors = new List<string>(); //class oluşturulduğunda error yoksa sorun cıkmasın
        }
    }
}
