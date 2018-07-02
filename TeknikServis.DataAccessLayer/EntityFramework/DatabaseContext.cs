using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeknikServis.Entities;

namespace TeknikServis.DataAccessLayer.EntityFramework
{
    public class DatabaseContext:DbContext
    {
        public DbSet<Worker> Workers { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Fee> Fees { get; set; }
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Stock> Stocks { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }
    }
}
