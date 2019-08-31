using API.Services;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [RoutePrefix("colunistas")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ColunistasController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idColunista")]
        public IHttpActionResult ObterColunistas(int? idColunista = null)
        {
            var colunistas = ColunistasService.ObterColunistas(idColunista);
            return Json(colunistas);
        }
    }
}
