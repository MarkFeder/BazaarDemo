using BazaarDemo.BackEnd.Infrastructure.OData.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using System.Configuration;
using System.IO;

namespace BazaarDemo.BackEnd.Infrastructure.OData
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            // Set up data directory
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            // Initialise Castle Windsor
            _container = InitializeCastleWindsor();

            // Set up Web Api Configuration
            GlobalConfiguration.Configure(config => WebApiConfig.Register(config, _container));
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (_container != null)
                _container.Dispose();
        }

        private IWindsorContainer InitializeCastleWindsor()
        {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            container.Install(FromAssembly.InDirectory(new AssemblyFilter(System.AppDomain.CurrentDomain.RelativeSearchPath, "BazaarDemo.BackEnd*.dll")));
            //container.Install(FromAssembly.This());
            return container;
        }
    }
}
