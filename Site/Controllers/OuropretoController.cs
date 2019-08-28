using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class OuropretoController : Controller
    {
        // GET: Ouropreto
        public ActionResult Index()
        {
            return RedirectPermanent("http://CompanyName.radiosaqui.com.br");            
        }
    }
}