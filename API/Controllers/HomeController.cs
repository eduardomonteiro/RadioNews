using API.Models;
using API.Services;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [RoutePrefix("home")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class HomeController : ApiController
    {
        [HttpGet]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterConteudoHome()
        {
            using (var db = new ModeloDados())
            {
                //Stopwatch swAudio = Stopwatch.StartNew();
                var audios = AudiosService.ObterAudios();
                //TimeSpan audioTime = swAudio.Elapsed;

                //Stopwatch swColunista = Stopwatch.StartNew();
                var colunistas = ColunistasService.ObterColunistas();
                //TimeSpan colunistaTime = swColunista.Elapsed;

                //Stopwatch swPublicidade = Stopwatch.StartNew();
                var publicidade = PublicidadesService.ObterPublicidadesPrincipais();
                //TimeSpan publicidadeTime = swPublicidade.Elapsed;

                //Stopwatch swNoticia = Stopwatch.StartNew();
                var noticias = NoticiasService.ObterNoticiasHome();
                //TimeSpan noticiaTime = swNoticia.Elapsed;

                var esportes = noticias.Take(3).ToList();
                var gerais = noticias.Skip(3).Take(3).ToList();

                var homeModel = new HomeViewModel { Audios = audios, Colunistas = colunistas, Noticias = gerais, Esportes = esportes, Publicidade = publicidade };

                //return Json("Audio: " + audioTime + " Colunistas: " + colunistaTime + " Publicidade: " + publicidadeTime + " Noticias: " + noticiaTime);
                return (Json(homeModel));
            }

        }
    }
}
