using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Services;
using System.Web.Http.Cors;
using API.ViewModel;

namespace API.Controllers
{
    [RoutePrefix("cidades")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CidadesController : ApiController
    {
        [HttpGet]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterCidades()
        {
            string key = "cidades:ObterCidades:TCidades";
            Func<object, List<CidadesViewModel>> funcao = t => ObterCidadesDB();
            List<CidadesViewModel> retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 120);
            return Json(retorno);            
        }

        private List<CidadesViewModel> ObterCidadesDB()
        {
            using (var db = new ModeloDados())
            {

                var cidades = db.cidade.Select(cidade => new CidadesViewModel{ id = cidade.id, nome = cidade.nome }).ToList();

                return cidades;
            }
        }
    }
}
