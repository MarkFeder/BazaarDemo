using BazaarDemo.BackEnd.Domain.Models;
using BazaarDemo.BackEnd.Infrastructure.OData.CustomRoutingConventions;
using BazaarDemo.BackEnd.Infrastructure.OData.DependencyResolver;
using Castle.Windsor;
using EverNext.Domain.Contracts.Model;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace BazaarDemo.BackEnd.Infrastructure.OData.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config, IWindsorContainer container)
        {
            // Register controller activator
            RegisterControllerActivator(container);

            // Create the default collection of built-in conventions
            var conventions = ODataRoutingConventions.CreateDefault();

            // Insert the custom conventions
            conventions[1] = new CustomEntitySetRoutingConvention();
            conventions[3] = new CustomEntityRoutingConvention();
            conventions[8] = new CustomActionRoutingConvention();

            // Config ODataServiceRoutes
            config.EnableUnqualifiedNameCall(unqualifiedNameCall: true);
            config.MapODataServiceRoute("OData", "api", GetImplicitModel(), new DefaultODataPathHandler(), conventions);

            // Ensure configurations
            config.EnsureInitialized();
        }

        /// <summary>
        /// Register controller activator
        /// </summary>
        /// <param name="container">Windsor container</param>
        private static void RegisterControllerActivator(IWindsorContainer container)
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(container));
        }

        /// <summary>
        /// Get the ODataModelBuilder
        /// </summary>
        /// <returns></returns>
        public static IEdmModel GetImplicitModel()
        {
            ODataModelBuilder builder = new ODataConventionModelBuilder();

            // Register EntitySets
            builder.EntitySet<ProductModel>("Products");
            builder.EntitySet<OrderModel>("Orders");
            builder.EntitySet<ProductFamilyModel>("ProductFamilies");
            builder.EntitySet<CustomerModel>("Customers");

            // Register Actions for model builder
            CreateActionsForEntityType<ProductModel>(builder, "Product", "Products");
            CreateActionsForEntityType<CustomerModel>(builder, "Customer", "Customers");
            CreateActionsForEntityType<OrderModel>(builder, "Order", "Orders");
            CreateActionsForEntityType<ProductFamilyModel>(builder, "ProductFamily", "ProductFamilies");

            return builder.GetEdmModel();
        }

        /// <summary>
        /// Create each Action for each EntityType
        /// </summary>
        /// <typeparam name="M">the type of the model</typeparam>
        /// <param name="builder">the builder to inject the actions</param>
        /// <param name="nameSpace">the namespace for each model</param>
        /// <param name="entitySet"> the name of the entitySet. It should be equals to the previous builder.EntitySet<Type>(entitySet) </param>
        private static void CreateActionsForEntityType<M>(ODataModelBuilder builder, string nameSpace, string entitySet)
            where M : class, IBaseEntity
        {
            // OData Endpoint
            var modelType = builder.EntityType<M>();
            modelType.Namespace = String.IsNullOrEmpty(nameSpace) ? "Default" : nameSpace + "Namespace";

            // Register Actions
            ActionConfiguration getModelAction = modelType.Action("GetModel");
            getModelAction.ReturnsFromEntitySet<M>(entitySet);
            getModelAction.Namespace = modelType.Namespace;

            ActionConfiguration getModelsAction = modelType.Collection.Action("GetModels");
            getModelsAction.ReturnsCollectionFromEntitySet<M>(entitySet);
            getModelsAction.Namespace = modelType.Namespace;

            ActionConfiguration postModelAction = modelType.Collection.Action("PostModel");
            postModelAction.EntityParameter<M>("model");
            postModelAction.ReturnsFromEntitySet<M>(entitySet);
            postModelAction.Namespace = modelType.Namespace;

            ActionConfiguration postModelsAction = modelType.Collection.Action("PostModels");
            postModelsAction.CollectionEntityParameter<M>("models");
            postModelsAction.ReturnsCollectionFromEntitySet<M>(entitySet);
            postModelsAction.Namespace = modelType.Namespace;

            ActionConfiguration putModelAction = modelType.Collection.Action("PutModel");
            putModelAction.EntityParameter<M>("model");
            putModelAction.ReturnsFromEntitySet<M>(entitySet);
            putModelAction.Namespace = modelType.Namespace;

            ActionConfiguration putModelsAction = modelType.Collection.Action("PutModels");
            putModelsAction.CollectionEntityParameter<M>("models");
            putModelsAction.ReturnsCollectionFromEntitySet<M>(entitySet);
            putModelsAction.Namespace = modelType.Namespace;

            ActionConfiguration deleteModelAction = modelType.Collection.Action("DeleteModel");
            deleteModelAction.EntityParameter<M>("model");
            deleteModelAction.Returns<IHttpActionResult>();
            deleteModelAction.Namespace = modelType.Namespace;

            ActionConfiguration deleteModelsAction = modelType.Collection.Action("DeleteModels");
            deleteModelsAction.CollectionEntityParameter<M>("models");
            deleteModelsAction.Returns<IHttpActionResult>();
            deleteModelsAction.Namespace = modelType.Namespace;

            ActionConfiguration batchModelsAction = modelType.Collection.Action("BatchModels");
            batchModelsAction.CollectionEntityParameter<M>("models_d");
            batchModelsAction.CollectionEntityParameter<M>("models_p");
            batchModelsAction.Returns<IHttpActionResult>();
            batchModelsAction.Namespace = modelType.Namespace;
        }
    }
}
