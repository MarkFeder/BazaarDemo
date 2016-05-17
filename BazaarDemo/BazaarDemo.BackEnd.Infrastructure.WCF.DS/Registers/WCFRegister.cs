using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Castle.Facilities.WcfIntegration;


namespace BazaarDemo.BackEnd.Infrastructure.WCF.DS.Registers
{
    public class WCFRegister : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {

            try
            {
                container.Register(Classes.FromAssemblyNamed("BazaarDemo.BackEnd.Domain.Services")
                .BasedOn(typeof(EverNext.Domain.Contracts.Model.IDeserveAService))
                .LifestylePerWcfOperation()
                .WithService.AllInterfaces()
                .Configure(component => component.Named(component.Implementation.FindInterfaces(new System.Reflection.TypeFilter((typeObj, criteriaObj) =>
                {
                    if (((Type)criteriaObj).IsAssignableFrom(typeObj) && ((Type)criteriaObj) != typeObj)
                        return true;
                    else
                        return false;

                }), typeof(EverNext.Domain.Contracts.Model.IDeserveAService)).FirstOrDefault().FullName)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                container.Dispose();
                throw;
            }
        }
    }
}