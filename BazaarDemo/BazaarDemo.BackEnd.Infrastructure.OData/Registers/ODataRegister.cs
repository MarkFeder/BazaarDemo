using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System.Web.OData;
using System.Web.Http.Controllers;
using System.Web.Http;
using BazaarDemo.BackEnd.Domain.Contracts.MappableEntityRepository;
using EverNext.Infrastructure.WebApi.OData.Tests.CodeFirst.Repositories.MappableRepositories;
using BazaarDemo.BackEnd.Domain.Entities;
using BazaarDemo.BackEnd.Domain.Models;

namespace BazaarDemo.BackEnd.Infrastructure.OData.Registers
{
    public class ODataRegister : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                // Register Repositories

                RegisterRepositories(container);

                // Register Controllers

                container.Register(Component.For<MetadataController>().ImplementedBy<MetadataController>().LifestylePerWebRequest());

                container
                    .Register(Classes.FromThisAssembly().BasedOn<IHttpController>().LifestylePerWebRequest())
                    .Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest())
                    .Register(Classes.FromThisAssembly().BasedOn<ODataController>().LifestylePerWebRequest());
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                container.Dispose();
                throw;
            }
        }

        private void RegisterRepositories(IWindsorContainer container)
        {
            container.Register(Component.For<IMappableEntityRepository<Product, ProductModel>>()
                   .ImplementedBy<ProductRepository>()
                   .LifestylePerWebRequest());

            container.Register(Component.For<IMappableEntityRepository<Order, OrderModel>>()
               .ImplementedBy<OrderRepository>()
               .LifestylePerWebRequest());

            container.Register(Component.For<IMappableEntityRepository<Customer, CustomerModel>>()
               .ImplementedBy<CustomerRepository>()
               .LifestylePerWebRequest());

            container.Register(Component.For<IMappableEntityRepository<ProductFamily, ProductFamilyModel>>()
               .ImplementedBy<ProductFamilyRepository>()
               .LifestylePerWebRequest());
        }
    }
}