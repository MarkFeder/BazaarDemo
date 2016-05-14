using BazaarDemo.BackEnd.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaarDemo.BackEnd.Infrastructure.DataBase.Context
{
    public class BazaarDBInitializer : DropCreateDatabaseIfModelChanges<BazaarContext>
    {
        protected override void Seed(BazaarContext context)
        {
            base.Seed(context);

            // Populate database
           
            #region products

            IEnumerable<Product> products = new List<Product>()
            {
                new Product()
                {
                    ProductName = "Product1",
                    ProductCost = 10.50m
                },

                new Product()
                {
                    ProductName = "Product2",
                    ProductCost = 20.50m
                },

                new Product()
                {
                    ProductName = "Product3",
                    ProductCost = 13.50m
                },

                new Product()
                {
                    ProductName = "Product4",
                    ProductCost = 5.50m
                },

                new Product()
                {
                    ProductName = "Product5",
                    ProductCost = 2.50m
                },
            };

            #endregion


            #region orders

            IEnumerable<Order> orders = new List<Order>()
            {
                new Order()
                {
                    RecurringOrder = false,
                    Products = new List<Product>()
                },

                new Order()
                {
                    RecurringOrder = false,
                    Products = new List<Product>()
                },

                new Order()
                {
                    RecurringOrder = false,
                    Products = new List<Product>()
                },

                new Order()
                {
                    RecurringOrder = false,
                    Products = new List<Product>()
                },

                new Order()
                {
                    RecurringOrder = false,
                    Products = new List<Product>()
                },
            };


            orders.ElementAt(0).Products.Add(products.ElementAt(0));
            orders.ElementAt(1).Products.Add(products.ElementAt(1));
            orders.ElementAt(2).Products.Add(products.ElementAt(2));
            orders.ElementAt(3).Products.Add(products.ElementAt(3));
            orders.ElementAt(4).Products.Add(products.ElementAt(4));

            context.SaveChanges();


            #endregion


            context.Orders.AddOrUpdate(o => o.OrderId,
                                               orders.ElementAt(0),
                                               orders.ElementAt(1),
                                               orders.ElementAt(2),
                                               orders.ElementAt(3),
                                               orders.ElementAt(4));

            context.SaveChanges();


            #region customers

            IEnumerable<Customer> customers = new List<Customer>()
            {
                new Customer()
                {
                    CustomerId = 1,
                    CustomerName = "Juan Pérez",
                    CustomerAddress = "Calle de Serrano, 9",
                    CustomerPostCode = "28941",
                    Orders = new List<Order>()
                },

                new Customer()
                {
                    CustomerId = 2,
                    CustomerName = "Jose Luis",
                    CustomerAddress = "Calle Alcalá, 10",
                    CustomerPostCode = "28942",
                    Orders = new List<Order>()
                },

                new Customer()
                {
                    CustomerId = 3,
                    CustomerName = "Luis Pérez",
                    CustomerAddress = "Calle del Coso, 11",
                    CustomerPostCode = "28943",
                    Orders = new List<Order>()
                },

                new Customer()
                {
                    CustomerId = 4,
                    CustomerName = "Víctor Robles",
                    CustomerAddress = "Calle Real, 12",
                    CustomerPostCode = "28944",
                    Orders = new List<Order>()
                },

                new Customer()
                {
                    CustomerId = 5,
                    CustomerName = "Juan Pérez",
                    CustomerAddress = "Calle de Serrano, 9",
                    CustomerPostCode = "28941",
                    Orders = new List<Order>()
                },
            };

            customers.ElementAt(0).Orders.Add(orders.ElementAt(0));
            customers.ElementAt(1).Orders.Add(orders.ElementAt(1));
            customers.ElementAt(2).Orders.Add(orders.ElementAt(2));
            customers.ElementAt(3).Orders.Add(orders.ElementAt(3));
            customers.ElementAt(4).Orders.Add(orders.ElementAt(4));

            #endregion

            context.Customers.AddOrUpdate(c => c.CustomerId,
                                               customers.ElementAt(0),
                                               customers.ElementAt(1),
                                               customers.ElementAt(2),
                                               customers.ElementAt(3),
                                               customers.ElementAt(4));

            context.SaveChanges();
        }
    }
}
