using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Services;
using System.Web.Http.Cors;
using API.Models;

namespace API.Controllers
{
    [RoutePrefix("audios")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class AudiosController : ApiController
    {
        [HttpGet]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterAudios()
        {
            var audios = AudiosService.ObterAudios();

            return Json(audios);
        }

        [HttpGet]
        [Route("categorias")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterCategorias()
        {
            var audios = AudiosService.ObterCategorias();
            return Json(audios);
        }

        [HttpGet]
        [Route("podcast")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterPodcast()
        {
            List<AudioViewModel> audios = AudiosService.ObterAudios();
            return Json(audios);
        }

        [HttpGet]
        [Route("categoria")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterCategorias(int? id)
        {
            var audios = AudiosService.ObterAudioCategoria(id);
            return Json(audios);
        }
    }
}
