using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Hanabi_ASP
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var smth = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
