using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Common;


namespace _5577yx
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.Add("cpsRoute", new ReDoRoute(
              "cps.5577yx.com",     // Domain with parameters 
              "{controller}/{action}/{id}",    // URL with parameters 
              new { controller = "SpreadCenter", action = "Index", id = UrlParameter.Optional }  // Parameter defaults 
              ));

            //routes.Add("adminRoute", new ReDoRoute(
            //  "admin.5577yx.com",     // Domain with parameters 
            //  "{controller}/{action}/{id}",    // URL with parameters 
            //  new { controller = "Admin", action = "Index", id = UrlParameter.Optional }  // Parameter defaults 
            //  ));

            //routes.Add("tgRoute", new ReDoRoute(
            // "tg.5577yx.com",     // Domain with parameters 
            // "{controller}/{action}/{id}",    // URL with parameters 
            // new { controller = "Tg", action = "Index", id = UrlParameter.Optional }  // Parameter defaults 
            // ));

            routes.Add("bbsRoute", new ReDoRoute(
            "www.5577yx.com",     // Domain with parameters 
            "{controller}/{action}/{id}",    // URL with parameters 
            new { controller = "Home", action = "Index", id = UrlParameter.Optional }  // Parameter defaults 
            ));


            routes.Add("DomainRoute", new DomainRoute(
            "{controller}.5577yx.com",     // Domain with parameters 
            "{controller}/{action}/{id}",    // URL with parameters 
            new { controller = "Home", action = "Index", id = UrlParameter.Optional }  // Parameter defaults 
            ));

            routes.MapRoute(
              name: "Default",
              url: "{controller}/{action}/{id}",
              defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
          );

            
        }
    }
}