using ELFinder.Connector.ASPNet.ModelBinders;
using ELFinder.Connector.Config;
using ELFinder.WebServer.ASPNet.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Administrativo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            // Use custom model binder
            ModelBinders.Binders.DefaultBinder = new ELFinderModelBinder();

            // Initialize ELFinder configuration
            InitELFinderConfiguration();

        }



        /// <summary>
        /// Initialize ELFinder configuration
        /// </summary>
        protected void InitELFinderConfiguration()
        {

            SharedConfig.ELFinder = new ELFinderConfig(
                Server.MapPath("~/Conteudo"),
                thumbnailsUrl: "Temp/"
                );

            SharedConfig.ELFinder.RootVolumes.Add(
                new ELFinderRootVolumeConfigEntry(
                    Server.MapPath("~/Conteudo"),
                    isLocked: false,
                    isReadOnly: false,
                    isShowOnly: false,
                    maxUploadSizeKb: null,      // null = Unlimited upload size
                    uploadOverwrite: true,
                    startDirectory: ""));

        }

    }
}