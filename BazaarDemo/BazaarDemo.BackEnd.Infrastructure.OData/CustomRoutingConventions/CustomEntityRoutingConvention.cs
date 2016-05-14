using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace BazaarDemo.BackEnd.Infrastructure.OData.CustomRoutingConventions
{
    public class CustomEntityRoutingConvention : EntityRoutingConvention
    {
        private const string customGetAction = "GetModel";
        private const string customDeleteAction = "DeleteModel";
        private const string customPutAction = "PutModel";

        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            var baseAction = base.SelectAction(odataPath, controllerContext, actionMap);

            if (baseAction == null)
            {
                // CustomConvention
                if (odataPath.PathTemplate == "~/entityset/key")
                {
                    EntitySetPathSegment entityPathSegment = odataPath.Segments[0] as EntitySetPathSegment;
                    KeyValuePathSegment keyPathSegment = odataPath.Segments[1] as KeyValuePathSegment;

                    HttpMethod httpMethod = controllerContext.Request.Method;

                    if (httpMethod == HttpMethod.Get)
                    {
                        controllerContext.RouteData.Values[ODataRouteConstants.Key] = keyPathSegment;

                        return (actionMap.Contains(customGetAction)) ? customGetAction : null;
                    }
                    else if (httpMethod == HttpMethod.Delete)
                    {
                        controllerContext.RouteData.Values[ODataRouteConstants.Key] = keyPathSegment;

                        return (actionMap.Contains(customDeleteAction)) ? customDeleteAction : null;
                    }
                    else if (httpMethod == HttpMethod.Put)
                    {
                        controllerContext.RouteData.Values[ODataRouteConstants.Key] = keyPathSegment;

                        return (actionMap.Contains(customPutAction)) ? customPutAction : null;
                    }
                }
            }

            return baseAction;
        }
    }
}
