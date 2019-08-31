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
    [RoutePrefix("buscar")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BuscarController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave")]
        public IHttpActionResult BuscarConteudo(string chave = null)
        {
            var audios = AudiosService.ObterAudios(chave);
            var colunistas = ColunistasService.ObterColunistas(null, chave);
            var noticias = NoticiasService.ObterNoticias(null, chave);
            var postagens = PostagensService.ObterPostagens(null, null, chave);

            var buscarViewModel = new BuscarViewModel { Audios = audios, Colunistas = colunistas, Noticias = noticias, Postagens = postagens };

            return Json(buscarViewModel);
        }
    }
}
