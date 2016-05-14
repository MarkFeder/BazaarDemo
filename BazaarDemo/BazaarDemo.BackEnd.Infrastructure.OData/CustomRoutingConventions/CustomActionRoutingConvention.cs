using System.Linq;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.OData.Routing;
using System.Web.OData.Routing.Conventions;

namespace BazaarDemo.BackEnd.Infrastructure.OData.CustomRoutingConventions
{
    public class CustomActionRoutingConvention : ActionRoutingConvention
    {
        public override string SelectAction(ODataPath odataPath, HttpControllerContext controllerContext, ILookup<string, HttpActionDescriptor> actionMap)
        {
            var baseAction = base.SelectAction(odataPath, controllerContext, actionMap);

            if (baseAction == null)
            {
                // Custom convention
                if (odataPath.PathTemplate == "~/entityset/action")
                {
                    HttpMethod httpMethod = controllerContext.Request.Method;

                    BoundActionPathSegment boundActionPathSegment = odataPath.Segments[1] as BoundActionPathSegment;

                    if (IsSupportedMethod(httpMethod))
                    {
                        controllerContext.RouteData.Values[ODataRouteConstants.Action] = boundActionPathSegment.Action;
                        string theAction = GetAction(httpMethod);

                        return (actionMap.Contains(theAction)) ? theAction : null;
                    }
                }
            }

            return baseAction;
        }

        private bool IsSupportedMethod(HttpMethod httpMethod)
        {
            return httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Delete || httpMethod == HttpMethod.Put;
        }

        private string GetAction(HttpMethod httpMethod)
        {
            string action = null;

            switch(httpMethod.Method)
            {
                case "POST":
                    action = "PostModels";
                    break;
                case "DELETE":
                    action = "DeleteModels";
                    break;
                case "PUT":
                    action = "PutModels";
                    break;
            }

            return action;
        }
    }
}
