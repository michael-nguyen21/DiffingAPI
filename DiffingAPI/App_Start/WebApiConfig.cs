using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DiffingAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
               routeTemplate: "v1/{controller}/{id}/{pos}",
                defaults: new { id = RouteParameter.Optional, pos = RouteParameter.Optional }
            );
        }
    }
}
