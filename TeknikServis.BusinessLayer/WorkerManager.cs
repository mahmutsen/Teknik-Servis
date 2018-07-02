using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.BusinessLayer.Abstract;
using TeknikServis.BusinessLayer.Results;
using TeknikServis.DataAccessLayer.EntityFramework;
using TeknikServis.Entities;
using TeknikServis.Entities.Messages;
using TeknikServis.Entities.ViewModelObjects;

namespace TeknikServis.BusinessLayer
{
    public class WorkerManager : ManagerBase<Worker>
    {

        public BusinessLayerResult<Worker> LoginWorker(LoginViewModel data)
        {
            //Service Id email kontrol
            //Service e ait productların gösterilmesi UI ın işi


            BusinessLayerResult<Worker> res = new BusinessLayerResult<Worker>();
            res.Result = Find(x => x.ServiceId == data.ServiceId && x.Email == data.Email && x.Password == data.Password);

            if (res.Result != null)
            {
                if (!res.Result.IsActive) //Personel aktif edilmemişse
                {
                    res.AddError(ErrorMessageCode.WorkerIsNotActive, "Personel aktif değil!");
                }
            }
            else
            {
                res.AddError(ErrorMessageCode.EmailOrPassWrong, "E-posta ve şifre uyuşmuyor");

                res.AddError(ErrorMessageCode.ServiceIdWrong, "Hatalı servis girişi");
            }

            return res;
        }

        public BusinessLayerResult<Worker> GetWorkerById(int id)
        {
            BusinessLayerResult<Worker> res = new BusinessLayerResult<Worker>();

            res.Result = Find(x => x.Id == id);

            if (res.Result == null)
            {
                res.AddError(ErrorMessageCode.WorkerNotFound, "Personel Bulunamadı");
            }
            return res;
        }

        public BusinessLayerResult<Worker> UpdateProfile(Worker data)
        {
            Worker db_worker = Find(x => x.Id != data.Id && x.Email == data.Email);
            BusinessLayerResult<Worker> res = new BusinessLayerResult<Worker>();

            if (db_worker != null) //Başka bir kullanıcının mailini girmiş isem
            {
                res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu e-posta adresi kullanılmakta");
                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Password = data.Password;

            if (string.IsNullOrEmpty(data.PersonelImage) == false)
            {
                res.Result.PersonelImage = data.PersonelImage;
            }

            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.ProfileCouldNotUpdated, "Profil Güncellenemedi");
            }

            return res;
        }

        //Metod Hiding.. Bu şekilde base en gelen insert metodunu geriye attık,Override a benzer mantık, ama burda dönüş tipi de değiştirilebildi
        public new BusinessLayerResult<Worker> Insert(Worker data)
        {

            Worker worker = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<Worker> res = new BusinessLayerResult<Worker>();

            res.Result = data;

            if (worker != null)
            {
                if (worker.Username == data.Username)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, " Kullanıcı Adı Kayıtlı");
                }
                if (worker.Email == data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, " E-posta adresi kayıtlı");
                }
            }
            else
            {
                res.Result.PersonelImage = data.PersonelImage;
                res.Result.ActivateGuid = Guid.NewGuid();

                if (base.Insert(res.Result) == 0)
                {
                    res.AddError(ErrorMessageCode.WorkerCouldNotInserted, "Personel eklenemedi");
                }

            }

            return res;
        }

        public new BusinessLayerResult<Worker> Update(Worker data)
        {
            Worker db_worker = Find(x => x.Username == data.Username || x.Email == data.Email);
            BusinessLayerResult<Worker> res = new BusinessLayerResult<Worker>();
            res.Result = data;

            if (db_worker != null && db_worker.Id!=data.Id) //Başka bir kullanıcının mailini girmiş isem
            {
                if (db_worker.Username==data.Username)
                {
                    res.AddError(ErrorMessageCode.UserNameAlreadyExist, "Kullanıcı Adı Kayıtlı");
                }

                if (db_worker.Email==data.Email)
                {
                    res.AddError(ErrorMessageCode.EmailAlreadyExist, "Bu e-posta adresi kayıtlı");
                }

                return res;
            }

            res.Result = Find(x => x.Id == data.Id);
            res.Result.ServiceId = data.ServiceId;
            res.Result.Email = data.Email;
            res.Result.Name = data.Name;
            res.Result.Surname = data.Surname;
            res.Result.Username = data.Username;
            res.Result.Password = data.Password;
            res.Result.IsActive = data.IsActive;
            res.Result.IsAdmin = data.IsAdmin;
            res.Result.IsManager = data.IsManager;
            if (base.Update(res.Result) == 0)
            {
                res.AddError(ErrorMessageCode.WorkerCouldNotUpdated, "Personel Güncellenemedi");
            }

            return res;
        }
    }

}
