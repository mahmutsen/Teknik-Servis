using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Entities.Messages;

namespace TeknikServis.BusinessLayer.Results
{
    public class BusinessLayerResult<T> where T:class //Tek bir exception nesnesine bağlı kalmadan istediğimiz kadar Hata dönderebiliriz
    {
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors =new List<ErrorMessageObj>(); //class oluşturulduğunda error yoksa sorun cıkmasın
        }

        public void AddError(ErrorMessageCode code,string message)
        {
            Errors.Add(new ErrorMessageObj() { Code = code, Message = message });
        }
    }
}
