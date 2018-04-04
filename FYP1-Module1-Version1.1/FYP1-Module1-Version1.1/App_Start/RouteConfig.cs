using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FYP1_Module1_Version1._1
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "MainView", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                 "Home",
                 "Home/{controller}/{action}/{id}",
                 new { controller = "Home", action = "newRegisterIdea", id = UrlParameter.Optional }
            );


        }
    }
}
