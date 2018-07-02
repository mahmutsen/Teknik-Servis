using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.DataAccessLayer;

namespace TeknikServis.DataAccessLayer.EntityFramework
{
    public class RepositoryBase //Singleton Pattern uygulamak için. DatabaseContext eğer yoksa burada oluşturulacak.tekrar tekrar newlenmeyecek
    {
        protected static DatabaseContext context;//Miras alan tarafında kullanılailecek protected
        private static object _lockSync = new object();

        protected RepositoryBase() // Protected yaptık çünkü newlenmesini istemiyoruz. Miras alanları bu constructor ile  context e erişmiş olacak
        {
            CreateContext();
        }

        private static void CreateContext() // static yapıldı çünkü class newlenmeden de metod çağırılabilmeli.
        {
            if (context==null)
            {
                lock (_lockSync)// Multithread sistemlerde dbcontextin aynı anda newlenmemesini garanti altına aldık
                {
                    if (context == null)
                    {
                        context = new DatabaseContext();
                    }
                }
            }

        }

    }
}
