using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Common
{
    public class DefaultCommon : ICommon // Varsayılan olarak system dönecek 
    {
        public string GetCurrentWorkerUName()
        {
            return "system";
        }
       
    }
}
