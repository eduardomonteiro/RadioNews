using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Site.Models;
using System.Data.Entity;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/noticias")]
    public class NoticiasController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObterNoticias()
        {
            using (var db = new ModeloDados())
            {
                var noticias = db.Noticias.Where(noticia => noticia.Colunista == null).Take(20).OrderByDescending(noticia => noticia.data).Select(noticia => new { noticia.id, noticia.titulo, noticia.chamada, noticia.texto, noticia.foto, noticia.data, nomeAutor = noticia.Colunista == null ? string.Empty : noticia.Colunista.nome }).ToList();

                return Json(noticias);
            }
        }

        [HttpGet]
        [Route("maisVistas")]
        public IHttpActionResult MaisVistas()
        {
            using (var db = new ModeloDados())
            {
                var noticias = db.Noticias.Where(noticia => noticia.Colunista == null).Take(7).OrderByDescending(noticia => noticia.visualizacao).ThenByDescending(noticia => noticia.data).Select(noticia => new { noticia.id, noticia.titulo, noticia.chamada, noticia.texto, noticia.foto, noticia.data, nomeAutor = noticia.Colunista == null ? string.Empty : noticia.Colunista.nome }).ToList();

                return Json(noticias);
            }
        }


    }
}
