using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DiamondWorldWebApi.Configuration
{
    public static class DiamondWorldWebAPIConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API routes
            config.MapHttpAttributeRoutes();

            //config.Filters.Add(new CustomExceptionFilter());
            //config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());

            // config.Routes.MapHttpRoute(
            //     name: "DefaultApi",
            //     routeTemplate: "api/{controller}/{id}",
            //     defaults: new { id = RouteParameter.Optional }
            // );


            // config.Routes.MapHttpRoute(
            //    name: "DownloadPdfFile",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new {}
            //);

            //    config.Routes.MapHttpRoute(
            //    name: "DownloadPdfFile",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new {  controller = "school" }
            //    constraints: new { id = "/d+" }
            //);



        }
    }
}