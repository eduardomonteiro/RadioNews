using API.Services;
using API.Util;
using API.ViewModel;
using Site.Enums;
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
    [RoutePrefix("radios")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RadiosController : ApiController
    {
        [HttpGet]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "idCidade")]
        public IHttpActionResult ObterRadios()
        {
            string key = "radios:ObterRadios:TRadios";
            Func<object, List<RadiosViewModel>> funcao = t => ObterRadiosDB();
            List<RadiosViewModel> retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 120);

            return Json(retorno);
        }
        private List<RadiosViewModel> ObterRadiosDB()
        {
            var url = ServerSettings.Url;

            using (var db = new ModeloDados())
            {
                var radios = db.Radios
                    .Where(radio => radio.Ativo)
                    .Select(radio =>
                    new RadiosViewModel
                    {
                        Id = radio.Id,
                        streamingMobile = (radio.TipoStream1 == TipoStream.MP3 ? radio.Stream1 : radio.TipoStream2 == TipoStream.MP3 ? radio.Stream2 : string.Empty),
                        Titulo = radio.Titulo,
                        logo = string.IsNullOrEmpty(radio.Imagem) ? null : url + "/Admin/Conteudo/Radio/" + radio.Imagem
                    }).OrderBy(radio => radio.Titulo).ToList();

                return radios;
            }
        }
    }
}
