using Site.Helpers;
using Site.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Site.Controllers
{

    //[OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
    public class BaseController : Controller
    {
        protected ModeloDados db = new ModeloDados();
        public BaseController()
        {
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.ValidateOnSaveEnabled = false;


            /*
            db.Configuration.AutoDetectChangesEnabled = false;
            db.Configuration.EnsureTransactionsForFunctionsAndCommands = false;
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = false;
            db.Configuration.UseDatabaseNullSemantics = false;
            db.Configuration.ValidateOnSaveEnabled = false;
            */
        }

        /*  [OutputCache(Duration = 28800, Location = System.Web.UI.OutputCacheLocation.Server)]
          protected override void OnActionExecuted(ActionExecutedContext filterContext)
          {

              db.Configuration.AutoDetectChangesEnabled = false;
              db.Configuration.ValidateOnSaveEnabled = false;
          }
          */
        protected string RenderRazorViewToString(string viewName, object model)
        {
            ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }

 
}