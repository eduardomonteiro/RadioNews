using API.Services;
using API.Util;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [RoutePrefix("publicidade")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class PublicidadesController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("principais")]
        public IHttpActionResult ObterPublicidadesPrincipais()
        {
            var publicidades = PublicidadesService.ObterPublicidadesPrincipais();
            return Json(publicidades);
        }

        [AcceptVerbs("GET")]
        [Route("internas")]
        public IHttpActionResult ObterPublicidadesInternas()
        {
            var publicidades = PublicidadesService.ObterPublicidadesInternas();
            return Json(publicidades);
        }

        [AcceptVerbs("GET")]
        [Route("clique")]
        public IHttpActionResult Clique(int idBanner)
        {
            var url = ServerSettings.Url;

            using (var db = new ModeloDados())
            {

                var publicidade = db.Banners.FirstOrDefault(banner => banner.Id == idBanner);
                publicidade.BannersVisualizacoesCliques.Add(new BannersVisualizacoesCliques
                {
                    CodigoBanner = publicidade.Id,
                    Visualizacao = false,
                    Clique = true,
                    DataCadastro = DateTime.Now
                });

                db.Entry(publicidade).State = EntityState.Modified;

                db.SaveChanges();

                HttpContext.Current.Response.Redirect(publicidade.Link);
            }
            return Json(true);
        }
    }
}
