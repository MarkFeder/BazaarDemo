using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using BazaarDemo.BackEnd.Domain.Contracts.DomainServices;
using BazaarDemo.BackEnd.Domain.Services;
using Castle.Facilities.WcfIntegration;

namespace BazaarDemo.BackEnd.Domain.Services.Registers
{
    public class ServicesRegister : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            try
            {
                container.Register(Component.For<IBazaarService>().ImplementedBy<BazaarService>().LifeStyle.PerWcfOperation());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                container.Dispose();
                throw;
            }
        }
    }
}
