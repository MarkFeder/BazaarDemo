using Castle.MicroKernel.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using EverNext.Domain.Contracts.Model;
using BazaarDemo.BackEnd.Domain.Entities;
using BazaarDemo.BackEnd.Domain.Models;
using Castle.Components.DictionaryAdapter;
using System.Data.Entity;
using BazaarDemo.BackEnd.Infrastructure.DataBase.Context;
using BazaarDemo.BackEnd.Domain.Contracts.UnitOfWork.DataBase;
using EverNext.Domain.Contracts.Services;
using BazaarDemo.BackEnd.Domain.Contracts.EntityRepositories;
using BazaarDemo.BackEnd.Infrastructure.DataBase.Repositories.Entities;
using System.Reflection;
using Castle.Facilities.WcfIntegration;

namespace BazaarDemo.BackEnd.Infrastructure.DataBase.Registers
{
    public class DBRegister : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                RegisterDbContext(container);

                RegisterUoW(container);

                RegisterMapper(container);

                RegisterDBServices(container);

                InitializeMappings();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                container.Dispose();
                throw;
            }
        }
        private void RegisterDbContext(IWindsorContainer container)
        {
            container.Register(Component.For<DbContext>()
                    .ImplementedBy<BazaarContext>()
                    .LifeStyle.PerWcfOperation());
        }

        private void RegisterUoW(IWindsorContainer container)
        {
            container.Register(Component.For<IUnitOfWork>()
                    .ImplementedBy<UnitOfWork.UnitOfWork>()
                    .LifeStyle.PerWcfOperation());
        }

        private void RegisterMapper(IWindsorContainer container)
        {
            container.Register(Component.For<IObjectMapper>()
                                    .ImplementedBy<EverNext.Infrastructure.AutoMapper.Mapper>()
                                    .LifeStyle.PerWcfOperation());
        }

        private void RegisterRepositories(IWindsorContainer container)
        {
            container.Register(Component.For<ICustomerRepository>()
                .ImplementedBy<CustomerRepository>()
                .LifestylePerWebRequest());

            container.Register(Component.For<IProductFamilyRepository>()
                .ImplementedBy<ProductFamilyRepository>()
                .LifestylePerWebRequest());

            container.Register(Component.For<IProductRepository>()
                .ImplementedBy<ProductRepository>()
                .LifestylePerWebRequest());
        }

        private void PerfomMapping<T, M>()
            where T : class, IBaseEntity
            where M : class, IBaseEntity
        {
            AutoMapper.Mapper
                      .CreateMap<T, M>()
                      .Bidirectional()
                      .ForAllMembersOfType(typeof(ICollection<>), x => x.UseDestinationValue())
                      .ForAllMembersOfType(typeof(ICollection<>), x => x.Ignore())
                      .ForAllMembersWithAnnotation(typeof(KeyAttribute), opt => opt.Ignore());
        }

        private void InitializeMappings()
        {
            //Perfom mapping

            PerfomMapping<Customer, CustomerModel>();
            PerfomMapping<Order, OrderModel>();
            PerfomMapping<Product, ProductModel>();
            PerfomMapping<ProductFamily, ProductFamilyModel>();
        }

        private void RegisterDBServices(IWindsorContainer container)
        {
            container.Register(Classes.FromAssemblyNamed("BazaarDemo.BackEnd.Infrastructure.DataBase")
                    .BasedOn(typeof(EverNext.Domain.Contracts.Model.IAggregateRoot))
                    .LifestylePerWebRequest()
                    .WithService.AllInterfaces()
                    .Configure(component => component.Named(component.Implementation.FindInterfaces(new TypeFilter((typeObj, criteriaObj) =>
                    {
                        if (((Type)criteriaObj).IsAssignableFrom(typeObj) && ((Type)criteriaObj) != typeObj)
                            return true;
                        else
                            return false;

                    }), typeof(EverNext.Domain.Contracts.Model.IAggregateRoot)).FirstOrDefault().FullName)));
        }
    }
}
