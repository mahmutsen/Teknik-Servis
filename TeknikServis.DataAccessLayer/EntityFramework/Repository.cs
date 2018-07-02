using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Common;
using TeknikServis.Core.DataAccess;
using TeknikServis.Entities;

namespace TeknikServis.DataAccessLayer.EntityFramework
{
    public class Repository<T>:RepositoryBase, IDataAccess<T> where T:class
    {
        private DbSet<T> _objectSet;

        public Repository()
        {
            _objectSet = context.Set<T>(); //Repositorybase den gelen context
        }

        public List<T> List()
        {
            //return db.Set<T>().ToList(); Bunun yerine db.Set ı yukarda contr. ile set etmek daha karlı
            return _objectSet.ToList();
        }

        public IQueryable<T> ListQueryable()
        {
            return _objectSet.AsQueryable<T>(); //Queryable olduğundan istediğimiz sorgularla birlikte sql e gönderebiliriz.
        }

        public List<T> List(Expression<Func<T,bool>> where)
        {
            return _objectSet.Where(where).ToList();
        }

        public T Find(Expression<Func<T,bool>> where)
        {
            return _objectSet.FirstOrDefault(where);
        }

        public int Insert(T obj)
        {
            _objectSet.Add(obj);
            DateTime now = DateTime.Now;

            if (obj is MyEntityBase) //insert edilecek nesne myentitybase den kalıtılmışsa  ona dönüştürüp ilgili özelliklerini set ediyoruz
            {
                MyEntityBase o = obj as MyEntityBase;
                
                o.CreatedOn = now;
                o.ModifiedOn = now;
                o.ModifiedUserName = App.Common.GetCurrentWorkerUName();//TODO işlem ypaan kullanıcı adı yazılacak
            }
            else if (obj is Cargo)
            {
                Cargo c = obj as Cargo;

                c.CreatedOn = now;
                c.ModifiedOn = now;
                c.ModifiedUserName = App.Common.GetCurrentWorkerUName();
            }
            else if (obj is Fee)
            {
                Fee f = obj as Fee;

                f.CreatedOn = now;
                f.ModifiedOn = now;
                f.ModifiedUserName = App.Common.GetCurrentWorkerUName();
            }

            return Save();
        }

        public int Update(T obj)
        {
            if (obj is MyEntityBase) //insert edilecek nesne myentitybase den kalıtılmışsa  ona dönüştürüp ilgili özelliklerini set ediyoruz
            {
                MyEntityBase o = obj as MyEntityBase;

                o.ModifiedOn = DateTime.Now;
                o.ModifiedUserName = App.Common.GetCurrentWorkerUName();//TODO işlem ypaan kullanıcı adı yazılacak
            }

            else if (obj is Cargo)
            {
                Cargo c = obj as Cargo;

                c.ModifiedOn = DateTime.Now;
                c.ModifiedUserName = App.Common.GetCurrentWorkerUName();
            }

            else if (obj is Fee)
            {
                Fee f = obj as Fee;

                f.ModifiedOn = DateTime.Now;
                f.ModifiedUserName = App.Common.GetCurrentWorkerUName();
            }
            return Save();
        }

        public int Delete(T obj)
        {
            _objectSet.Remove(obj);
            return Save();
        }

        public int Save()
        {
            return context.SaveChanges(); //Etkilenen satırların sayısıı dönderir
        }

    }

}
