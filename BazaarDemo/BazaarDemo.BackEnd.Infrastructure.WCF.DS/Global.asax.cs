using BazaarDemo.BackEnd.Domain.Services.Registers;
using BazaarDemo.BackEnd.Infrastructure.DataBase.Registers;
using BazaarDemo.BackEnd.Infrastructure.WCF.DS.Registers;
using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Configuration.Interpreters;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace BazaarDemo.BackEnd.Infrastructure.WCF.DS
{
    public class Global : System.Web.HttpApplication
    {
        private IWindsorContainer _container;

        protected void Application_Start(object sender, EventArgs e)
        {
            _container = InitializeCastleWindsor();
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {
            if (_container != null)
                _container.Dispose();
        }

        private IWindsorContainer InitializeCastleWindsor()
        {
            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

            container.AddFacility<WcfFacility>(f => f.CloseTimeout = TimeSpan.Zero);
            container.Install(new DBRegister_Wcf(), new WCFRegister(), new ServicesRegister());
            //container.Install(FromAssembly.This());
            return container;
        }
    }
}