using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.DataAccessLayer.EntityFramework;
using TeknikServis.Entities;

namespace TeknikServis.BusinessLayer
{
    public class Test
    {
        private Repository<Worker> repo_worker = new Repository<Worker>();
        private Repository<Category> repo_category = new Repository<Category>();
        private Repository<Service> repo_service = new Repository<Service>();
        private Repository<Customer> repo_customer = new Repository<Customer>();
        private Repository<Product> repo_product = new Repository<Product>();
        private Repository<Report> repo_report = new Repository<Report>();
        private Repository<Pricing> repo_pricing = new Repository<Pricing>();
        private Repository<BankAccount> repo_bankAccount = new Repository<BankAccount>();

        public Test()
        {
            //DataAccessLayer.DatabaseContext db = new DataAccessLayer.DatabaseContext();
            ////db.Database.CreateIfNotExists(); Burası Initializer ile birlikte çalışmıyor. Onu da dahil etmek için bir Tabloya ToList() dedik aşağıda
            //db.Categories.ToList();

            List<Category> categories = repo_category.List();
            //List<Category> categories_filtered = repo_category.List(x => x.Id >= 5);
        }

        public void InsertTest()
        {

            int result = repo_worker.Insert(new Worker()
            {
                Name = "Mustafa",
                Surname = "Şen",
                Email = "senmustafa@gmail.com",
                ActivateGuid = Guid.NewGuid(),  //Normalde Böyle yapılmayacak. Test Datası olduğundan el ile guid i verdik
                IsActive = true,
                IsAdmin = true,
                IsManager=false,
                Username = "nebere",
                Password = "123123",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "nebere"
            });
        }

        //public void InsertTest2()
        //{

        //    int result2 = repo_product.Insert(new Product()
        //    {

        //        Imei = "121321412412145",
        //        Problems = "212eweweqweqewwqeqweqwd",
        //        Owner = new Customer()
        //        {
        //            Name="sadsad",
        //            Surname = "asas",
        //            Email = "daasdqq@mynt.com",
        //            Tel = "2321412412",
        //            City = "wqewe",
        //            Adress = "dsasdsadasdasdasd",
        //            FormNo = Guid.NewGuid(),
        //            CreatedOn = DateTime.Now,
        //            ModifiedOn = DateTime.Now.AddMinutes(5),
        //            ModifiedUserName = "nebere",

        //        },
        //        CreatedOn = DateTime.Now,
        //        ModifiedOn = DateTime.Now.AddMinutes(5),
        //        ModifiedUserName = "nebere"

        //    });
            //int result2=repo_product.Insert(new Product()
            //{
            //    Imei = "121321412412",
            //    Problems = "212eweweqweqewwqeqweqwd",
            //    Owner=new Customer
            //    {
            //        Name = "sdasdas",
            //        Surname = "asas",
            //        Email = "daasdqq@mynt.com",
            //        Tel = "2321412412",
            //        City = "wqewe",
            //        Adress = "dsasdsadasdasdasd"
            //    }
            //});
        //}

        public void UpdateTest()
        {
            Worker worker = repo_worker.Find(x => x.Username == "nebere");

            if (worker != null)
            {
                worker.Username = "mmsen44";
                worker.Email = "seleme@mynet.com";

                int result = repo_worker.Update(worker);
            }
        }

        public void DeleteTest()
        {
            Worker worker = repo_worker.Find(x => x.Id == 295);

            if (worker != null)
            {
                int result = repo_worker.Delete(worker);
            }
        }

        //public void ReportTest()
        //{
        //    Worker worker = repo_worker.Find(x => x.Id == 2);
        //    Product product = repo_product.Find(x => x.Id == 1);

        //    Report report = new Report()
        //    {
        //        Text = "Deneme test",
        //        CreatedOn = DateTime.Now,
        //        ModifiedOn = DateTime.Now,
        //        ModifiedUserName = "mahmutsen",
        //        Product = product,
        //        Worker = worker
        //    };

        //    repo_report.Insert(report);//An entity object cannot be referenced by multiple instances of IEntityChangeTracker. Bu hatayı worker ve product repoları report reposundan farklı contextler kullandığından alıyoruz.Yani insert etmeye çalıştığımız reportun içinde başka bir contextin verdiği nesneler olamaz diyor
        //}
    }
}
