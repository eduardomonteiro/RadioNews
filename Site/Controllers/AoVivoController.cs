using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class AoVivoController : Controller
    {
        // GET: AoVivo
        public ActionResult Index()
        {
            return RedirectToAction("Player","Home",new { chave = "belo-horizonte" });
        }
    }
}