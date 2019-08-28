using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/posts")]
    public class PostagensController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObterPostagens()
        {
            using (var db = new ModeloDados())
            {
                var noticias = db.Noticias.Where(noticia => noticia.Colunista != null).Take(20).OrderByDescending(noticia => noticia.data).Select(noticia => new { noticia.id, noticia.titulo, noticia.chamada, noticia.texto, noticia.foto, noticia.data, nomeAutor = noticia.Colunista == null ? string.Empty : noticia.Colunista.nome }).ToList();

                return Json(noticias);
            }
        }
    }
}
