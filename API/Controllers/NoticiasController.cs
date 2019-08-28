using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Data.Entity;
using API.Services;
using System.Web.Http.Cors;
using API.Models;

namespace API.Controllers
{
    [RoutePrefix("noticias")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NoticiasController : ApiController
    {
        [HttpGet]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idEditoria;idNoticia")]
        public IHttpActionResult ObterNoticias(int? idEditoria = null, int? idNoticia = null)
        {
            var noticia = new NoticiaViewModel();
            var noticias = new List<NoticiaViewModel>();

            if (idNoticia != null)
            {
                noticia = NoticiasService.ObterNoticia(idNoticia.Value);

                if (noticia == null)
                {
                    return Json(new { Falha = new { code = HttpStatusCode.NotFound, msg = "Notícia não encontada." } });
                }

                noticias = NoticiasService.ObterNoticiasRelacionadas(idNoticia.Value);

                return Json(new { Noticia = noticia, Relacionados = noticias });
            }
            else
            {
                noticias = NoticiasService.ObterNoticias(idEditoria);
            }

            return Json(noticias);
        }

        [HttpGet]
        [Route("maisVistas")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idEditoria")]
        public IHttpActionResult MaisVistas(int? idEditoria = null)
        {
            var noticias = NoticiasService.ObterNoticiasMaisVistas(idEditoria);
            return Json(noticias);
        }


    }
}
