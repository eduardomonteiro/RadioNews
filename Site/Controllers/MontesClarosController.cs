using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class MontesClarosController : Controller
    {
        // GET: AoVivo

        public ActionResult Index()
        {
            return RedirectPermanent("https://www.facebook.com/CompanyNameFM");
        }
    }
}