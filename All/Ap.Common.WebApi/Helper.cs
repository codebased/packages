using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace Ap.Common.WebApi
{
    public static class Helper
    {
        public static void Initialize()
        {
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerSelector),
             new HttpNotFoundAwareDefaultHttpControllerSelector(GlobalConfiguration.Configuration));
            GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpActionSelector),
                new HttpNotFoundAwareControllerActionSelector());

            RegisterRoutes(GlobalConfiguration.Configuration.Routes);

        }

        private static void RegisterRoutes(HttpRouteCollection routes)
        {
            routes.MapHttpRoute("Error404", "{*url}", new { controller = "Error", action = "Handle404" });
        }
    }
}

