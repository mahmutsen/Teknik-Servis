using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeknikServis.Common //Bu Katman diğer katmanlardan UI katmanına erişebilmek için kuruldu(Normal akış (UI>BL>DatatAccess)>Entity) Şeklinde
{
    public interface ICommon // data Access layerdan UI da tutulan sesion bilgiine erişilmek istendiğinden
    {
        string GetCurrentWorkerUName();

    }
}
