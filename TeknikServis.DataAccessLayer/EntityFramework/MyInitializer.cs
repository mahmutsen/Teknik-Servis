using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TeknikServis.Entities;

namespace TeknikServis.DataAccessLayer.EntityFramework
{
    public class MyInitializer : CreateDatabaseIfNotExists<DatabaseContext>
    {

        protected override void Seed(DatabaseContext context)
        {
            Worker admin = new Worker()
            {
                Name = "Mahmut",
                Surname = "Şen",
                Email = "senmahmutm@gmail.com",
                ActivateGuid = Guid.NewGuid(),  //Normalde Böyle yapılmayacak. Test Datası olduğundan el ile guid i verdik
                IsActive = true,
                IsAdmin = true,
                IsManager=false,
                PersonelImage = "skeeter-vac-repair.png",
                Username = "mahmutsen",
                Password = "2525252",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "mahmutsen",
                ServiceId=1
            };

            Worker siteadmin = new Worker()
            {
                Name = "Mahmut",
                Surname = "Şen",
                Email = "mahmutsen@gmail.com",
                ActivateGuid = Guid.NewGuid(),  //Normalde Böyle yapılmayacak. Test Datası olduğundan el ile guid i verdik
                IsActive = true,
                IsAdmin = false,
                IsManager = true,
                PersonelImage = "skeeter-vac-repair.png",
                Username = "msen",
                Password = "123123",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "msen",
                ServiceId = 1
            };

            context.Workers.Add(siteadmin);
            context.Workers.Add(admin);
            //context.SaveChanges();

            int indexWorker = 2;// Worker username i ve modifiedUsernamei için
            List<Worker> personelList = new List<Worker>();

            List<string> pricings = new List<string>() { "Yazılım Güncelleme", "Ekran", "Şarj Soketi", "Sim Soketi", "Mikrofon", "Hoparlör", "Anakasa", "Anakart", "Ön Kamera", "Arka Kamera", "Batarya" };

            //Adding Fake Categories
            for (int i = 0; i < 7; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.TextData.GetSentences(1),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "mahmutsen"
                };

                context.Categories.Add(cat);

                //Adding Fake Pricings
                for (int z = 0; z < pricings.Count; z++)
                {
                    Pricing pric = new Pricing()
                    {
                        DefectType = pricings[z],
                        Price = FakeData.NumberData.GetNumber(30, 100).ToString(),
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-2), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-1), DateTime.Now),
                        ModifiedUserName = admin.Username
                    };

                    cat.Pricings.Add(pric);
                }

                //Adding Fake Services...
                for (int j = 0; j < FakeData.NumberData.GetNumber(3, 4); j++)
                {

                    Service service = new Service()
                    {
                        Title = FakeData.NameData.GetCompanyName(),
                        Text = FakeData.TextData.GetSentence(),
                        City = FakeData.PlaceData.GetCity(),
                        Adress = FakeData.PlaceData.GetAddress(),
                        Tel = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Fax = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Email = FakeData.NetworkData.GetEmail(),
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-2), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-1), DateTime.Now),
                        ModifiedUserName = admin.Username,
                    };

                    //Adding Fake Stocks
                    for (int s = 0; s < pricings.Count; s++)
                    {
                        Stock stock = new Stock()
                        {
                            Name = pricings[s],
                            Quantity = FakeData.NumberData.GetNumber(20,150),
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-2), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-1), DateTime.Now),
                            ModifiedUserName = admin.Username
                        };

                        service.Stocks.Add(stock);
                    }

                    for (int y = 0; y < FakeData.NumberData.GetNumber(1, 2); y++)
                    {
                        BankAccount bank = new BankAccount()
                        {
                            AccountName = "MMS Teknik Servis Hiz. ve Tic. A.Ş.",
                            Bank = FakeData.NameData.GetCompanyName(),
                            AccountNo = FakeData.PhoneNumberData.GetUsPhoneNumber(),
                            IBAN = $"TR{FakeData.PhoneNumberData.GetInternationalPhoneNumber()}",
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-2), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-1), DateTime.Now),
                            ModifiedUserName = admin.Username
                        };

                        service.BankAccounts.Add(bank);
                    }

                    cat.Services.Add(service);

                    //Adding Fake Workers
                    for (int k = 0; k < FakeData.NumberData.GetNumber(4, 7); k++)
                    {
                        Worker personal = new Worker()
                        {
                            Name = FakeData.NameData.GetFirstName(),
                            Surname = FakeData.NameData.GetSurname(),
                            Email = FakeData.NetworkData.GetEmail(),
                            ActivateGuid = Guid.NewGuid(),  //Normalde Böyle yapılmayacak. Test Datası olduğundan el ile guid i verdik
                            IsActive = true,
                            IsAdmin = false,
                            IsManager=false,
                            Username = $"user{indexWorker}",
                            Password = "123123",
                            PersonelImage = "skeeter-vac-repair.png",
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-2), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-1), DateTime.Now),
                            ModifiedUserName = $"user{indexWorker}"
                        };

                        indexWorker++;
                        personelList.Add(personal);

                        service.Workers.Add(personal);
                    

                    //Adding Fake Products
                    for (int m = 0; m < FakeData.NumberData.GetNumber(3, 5); m++)
                    {
                        //Worker ModifiedUserName = personelList[FakeData.NumberData.GetNumber(0, personelList.Count - 1)];

                        Product product = new Product()
                        {
                            Imei = FakeData.TextData.GetNumeric(15),
                            //Service = service,
                            Owner = new Customer()
                            {
                                Name = FakeData.NameData.GetMaleFirstName(),
                                Surname = FakeData.NameData.GetSurname(),
                                Tel = FakeData.PhoneNumberData.GetPhoneNumber(),
                                Email = FakeData.NetworkData.GetEmail(),
                                City = FakeData.PlaceData.GetCity(),
                                Adress = FakeData.PlaceData.GetAddress(),
                                IsActive = true,
                                FormNo = Guid.NewGuid(),
                                CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-7), DateTime.Now),
                                ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-6), DateTime.Now),
                                ModifiedUserName = personal.Username
                            },
                            IsRepaired=false,
                            Problems = FakeData.TextData.GetSentences(2),
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-7), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-6), DateTime.Now),
                            ModifiedUserName = personal.Username
                        };

                            personal.Products.Add(product);
                            service.Products.Add(product);

                            //Adding Fake Reports
                            for (int n = 0; n < FakeData.NumberData.GetNumber(2, 3); n++)
                        {


                            Report report = new Report()
                            {
                                Text = FakeData.TextData.GetSentence(),
                                //Worker = ModifiedUserName,
                                //Service = service,
                                CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-3), DateTime.Now),
                                ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-2), DateTime.Now),
                                ModifiedUserName = personal.Username
                            };
                            personal.Reports.Add(report);
                            product.Reports.Add(report);

                        }
                    }
                    }
                }


            }
            context.SaveChanges();
        }


    }
}

