using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BazaarDemo.Global
{
    public static class ContainerManager
    {
        private static IWindsorContainer _container = null;
        public static IWindsorContainer Container
        {
            get
            {
                if (_container == null)
                {
                    _container = new WindsorContainer(new XmlInterpreter());

                    _container.AddFacility<WcfFacility>(f => f.CloseTimeout = TimeSpan.Zero);

                    // Run all Installers from this global point    
                    _container.Install(FromAssembly.InDirectory(new AssemblyFilter(System.AppDomain.CurrentDomain.RelativeSearchPath, "BazaarDemo.BackEnd*.dll")));
                }

                return _container;
            }
        }
    }
}
