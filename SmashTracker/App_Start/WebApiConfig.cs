using SmashTracker.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SmashTracker
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
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

			// make all web-api requests to be sent over https
			config.MessageHandlers.Add(new EnforceHttpsHandler());
		}
    }
}
