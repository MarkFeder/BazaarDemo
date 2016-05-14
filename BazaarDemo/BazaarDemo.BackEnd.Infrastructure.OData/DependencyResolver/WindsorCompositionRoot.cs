using System;
using Castle.Windsor;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace BazaarDemo.BackEnd.Infrastructure.OData.DependencyResolver
{
    /// <summary>
    /// Manage windsor composition root for WebAPI controller registration
    /// </summary>
    public class WindsorCompositionRoot : IHttpControllerActivator
    {
        #region Private Properties

        private readonly IWindsorContainer _container;

        #endregion

        #region Constructor 

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="container">Windsor container</param>
        public WindsorCompositionRoot(IWindsorContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Crete http controller
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="controllerDescriptor">Descriptor</param>
        /// <param name="controllerType">Type</param>
        /// <returns>Registered controller</returns>
        public IHttpController Create(
            HttpRequestMessage request,
            HttpControllerDescriptor controllerDescriptor,
            Type controllerType)
        {
            var controller =
                (IHttpController)_container.Resolve(controllerType);
            
            request.RegisterForDispose(
                new Release(
                    () => _container.Release(controller)));

            return controller;
        }

        #endregion

        /// <summary>
        /// Manage Release class
        /// </summary>
        private sealed class Release : IDisposable
        {
            private readonly Action _release;

            public Release(Action release)
            {
                _release = release;
            }

            public void Dispose()
            {
                _release();
            }
        }
    }
}