using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Administrativo
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            // Commands - publish
            routes.MapRoute("Connector", "ELFinderConnector",
                new { controller = "ELFinderConnector", action = "Main" });
            //local
            /*
            routes.MapRoute("Connector", "ELFinderConnector",
                new { controller = "ELFinderConnector", action = "Main" });
                */

            //// Thumbnails
            //routes.MapRoute("Thumbnauls", "Thumbnails/{target}",
            //    new { controller = "ELFinderConnector", action = "Thumbnails" });

            //// Index view
            //routes.MapRoute("DefaultElfinder", "{controller}/{action}",
            //    new { controller = "ELFinder", action = "Index" }
            //);
            routes.MapRoute(
                name: "ApoioPAD",
                url: "ApoioPaineis/{action}/{id}",
                defaults: new { controller = "ApoioPAD", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "TimesPAD",
                url: "TimesPaineis/{action}/{id}",
                defaults: new { controller = "TimesPAD", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "NoticiasPAD",
                url: "NoticiasPaineis/{action}/{id}",
                defaults: new { controller = "NoticiasPAD", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "RedesSociaisPAD",
                url: "RedesSociaisPaineis/{action}/{id}",
                defaults: new { controller = "RedesSociaisPAD", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "EGolPAD",
                url: "EGolPaineis/{action}/{id}",
                defaults: new { controller = "EGolPAD", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "BannersPAD",
                url: "BannersPaineis/{action}/{id}",
                defaults: new { controller = "BannersPAD", action = "Index", id = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}