using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BazaarDemo.BackEnd.Domain.Entities;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace BazaarDemo.BackEnd.Infrastructure.DataBase.Context
{
    public class BazaarContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductFamily> ProductFamilies { get; set; }

        public BazaarContext()
            : base("name=BazaarContext")
        {
            // Set up our custom database initializer
            Database.SetInitializer<BazaarContext>(new BazaarDBInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Set up conventions
            modelBuilder.Conventions.Add<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Add<OneToManyCascadeDeleteConvention>();
        }
    }
}
