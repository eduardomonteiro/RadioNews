using API.Models;
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
    [RoutePrefix("posts")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PostagensController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idEditoria;idPostagem;idColunista")]
        public IHttpActionResult ObterPostagens(int? idEditoria = null, int? idPostagem = null, int? idColunista = null)
        {
            var noticia = new PostagemViewModel();
            var noticias = new List<PostagemViewModel>();

            if (idPostagem != null)
            {
                noticia = PostagensService.ObterPostagem(idPostagem.Value);

                if (noticia == null)
                {
                    return Json(new { Falha = new { code = HttpStatusCode.NotFound, msg = "Postagem não encontada." } });
                }

                noticias = PostagensService.ObterPostagensRelacionadas(idPostagem.Value);

                return Json(new { Postagem = noticia, Relacionados = noticias });
            }
            else
            {
                noticias = PostagensService.ObterPostagens(idEditoria, idColunista);
            }

            return Json(noticias);
        }
    }
}
