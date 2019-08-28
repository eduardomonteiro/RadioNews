using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/audios")]
    public class AudiosController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObterAudios()
        {
            using (var db = new ModeloDados())
            {
                var audios = db.Noticias.Where(noticia => noticia.Colunista == null && string.IsNullOrEmpty(noticia.audio)).Take(20).OrderByDescending(noticia => noticia.data).Select(noticia => new { url = noticia.audio, noticia.titulo, noticia.data, noticia.id }).ToList();

                return Json(audios);
            }
        }
    }
}
