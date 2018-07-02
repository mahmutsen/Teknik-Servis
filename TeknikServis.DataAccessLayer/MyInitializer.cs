using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TeknikServis.Entities;

namespace TeknikServis.DataAccessLayer
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
                Username = "mahmutsen",
                Password = "2525252",
                CreatedOn = DateTime.Now,
                ModifiedOn = DateTime.Now.AddMinutes(5),
                ModifiedUserName = "mahmutsen"
            };

            context.Workers.Add(admin);
            context.SaveChanges();


            //Adding Fake Categories
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.NameData.GetCompanyName(),
                    Description = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(15, 20)),
                    CreatedOn = DateTime.Now,
                    ModifiedOn = DateTime.Now,
                    ModifiedUserName = "mahmutsen"
                };

                context.Categories.Add(cat);

                //Adding Fake Services...
                for (int j = 0; j < FakeData.NumberData.GetNumber(5, 9); j++)
                {
                    Service service = new Service()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 20)),
                        Text = FakeData.TextData.GetSentence(),
                        //Workers = new List<Worker>() { admin },
                        City = FakeData.PlaceData.GetCity(),
                        Adress = FakeData.PlaceData.GetAddress(),
                        Tel = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Fax = FakeData.PhoneNumberData.GetPhoneNumber(),
                        Email = FakeData.NetworkData.GetEmail(),
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-2), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUserName = admin.Username,
                    };

                    cat.Services.Add(service);

                    //Adding Fake Workers
                    for (int k = 0; k < FakeData.NumberData.GetNumber(4, 7); k++)
                    {
                        Worker personel = new Worker()
                        {
                            Name = FakeData.NameData.GetFirstName(),
                            Surname = FakeData.NameData.GetSurname(),
                            Email = FakeData.NetworkData.GetEmail(),
                            ActivateGuid = Guid.NewGuid(),  //Normalde Böyle yapılmayacak. Test Datası olduğundan el ile guid i verdik
                            IsActive = true,
                            IsAdmin = false,
                            Username = $"user{k}",
                            Password = "123123",
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddMonths(-8), DateTime.Now),
                            ModifiedUserName = $"user{k}"
                        };

                        service.Workers.Add(personel);

                        //Adding Fake Customers
                        for (int l = 0; l < FakeData.NumberData.GetNumber(10, 20); l++)
                        {
                            Customer customer = new Customer()
                            {
                                Name = FakeData.NameData.GetMaleFirstName(),
                                Surname = FakeData.NameData.GetSurname(),
                                Tel = FakeData.PhoneNumberData.GetPhoneNumber(),
                                Email = FakeData.NetworkData.GetEmail(),
                                City = FakeData.PlaceData.GetCity(),
                                Adress = FakeData.PlaceData.GetAddress(),
                                InformCustomer = true,
                                FormNo = Guid.NewGuid(),
                                CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-7), DateTime.Now),
                                ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-6), DateTime.Now),
                                ModifiedUserName = admin.Name
                            };

                            service.Customers.Add(customer);

                            //Adding Fake Products
                            for (int m = 0; m < FakeData.NumberData.GetNumber(1, 2); m++)
                            {
                                Product product = new Product()
                                {
                                    Imei = FakeData.NumberData.GetNumber(15),
                                    Problems = FakeData.TextData.GetSentences(2),
                                    Category = cat,
                                    Service = service,
                                    CreatedOn = customer.CreatedOn,
                                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-6), DateTime.Now),
                                    ModifiedUserName = personel.Name
                                };

                                customer.Products.Add(product);
                                personel.Products.Add(product);

                                //Adding Fake Reports
                                for (int n = 0; n < FakeData.NumberData.GetNumber(2, 3); n++)
                                {
                                    Report report = new Report()
                                    {
                                        Text = FakeData.TextData.GetSentence(),
                                        Service = service,
                                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-3), DateTime.Now),
                                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddDays(-2), DateTime.Now),
                                        ModifiedUserName = personel.Name
                                    };

                                    product.Reports.Add(report);

                                }
                            }
                        }
                    }
                    
                }
            }

            context.SaveChanges();
        }
    }
}
