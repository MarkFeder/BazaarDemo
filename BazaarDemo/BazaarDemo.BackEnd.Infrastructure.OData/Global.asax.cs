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
using Castle.Facilities.WcfIntegration;
using BazaarDemo.BackEnd.Infrastructure.DataBase.Registers;
using BazaarDemo.BackEnd.Infrastructure.OData.Registers;

namespace BazaarDemo.BackEnd.Infrastructure.OData
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start()
        {
            // Set up data directory
            SetUpDataDirectory();

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

        private void SetUpDataDirectory()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);
        }

        private IWindsorContainer InitializeCastleWindsor()
        {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            container.Install(new DBRegister_WebRequest(), new ODataRegister());
            //container.Install(FromAssembly.This());
            return container;
        }
    }
}
