using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ErrosController : Controller
    {
        // GET: Error
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Erro500()
        {
            return View();
        }
    }
}