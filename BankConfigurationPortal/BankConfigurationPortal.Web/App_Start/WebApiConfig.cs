﻿using System.Web.Http;

namespace BankConfigurationPortal.Web {
    public static class WebApiConfig {
        public static void Register(HttpConfiguration config) {
            config.EnableCors();
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
