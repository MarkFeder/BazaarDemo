//using Castle.MicroKernel.Registration;
//using Castle.Windsor;
//using Castle.Windsor.Configuration.Interpreters;
//using Castle.Windsor.Installer;
//using Microsoft.Owin;
//using Owin;
//using OWIN.Windsor.DependencyResolverScopeMiddleware;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.Http;

//[assembly: OwinStartup(typeof(BazaarDemo.BackEnd.Infrastructure.OData.App_Start.OwinStartup))]

//namespace BazaarDemo.BackEnd.Infrastructure.OData.App_Start
//{
//    public class OwinStartup
//    {
//        private IWindsorContainer _container;

//        public void Configuration(IAppBuilder app)
//        {
//            // Initialise CastleWindsor
//            _container = InitializeCastleWindsor();

//            // Initialise HttpConfiguration
//            HttpConfiguration config = new HttpConfiguration();

//            // Register WebApiConfiguration
//            WebApiConfig.Register(config, _container);

//            // Use WebApiConfiguration and inyect container
//            app.UseWindsorDependencyResolverScope(config, _container).UseWebApi(config);
//        }

//        private IWindsorContainer InitializeCastleWindsor()
//        {
//            IWindsorContainer container = new WindsorContainer(new XmlInterpreter());

//            container.Install(FromAssembly.InDirectory(new AssemblyFilter(System.AppDomain.CurrentDomain.RelativeSearchPath, "BazaarDemo.BackEnd.*.dll")));
//            //container.Install(FromAssembly.This());

//            return container;
//        }
//    }
//}