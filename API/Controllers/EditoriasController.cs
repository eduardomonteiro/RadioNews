using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API.Models;
using API.Services;
using System.Web.Http.Cors;
using API.Util;
using API.ViewModel;

namespace API.Controllers
{
    [RoutePrefix("editorias")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EditoriasController : ApiController
    {
        [AcceptVerbs("GET")]
        [Route("")]
        [System.Web.Mvc.OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public IHttpActionResult ObterEditorias()
        {
            string key = "editorias:ObterEditorias:TEditoriais";
            Func<object, EditoriasViewModel> funcao = t => ObterEditoriasDB();
            EditoriasViewModel retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 120);

            return Json(retorno);
        }

        public EditoriasViewModel ObterEditoriasDB()
        {
            int[] listaIdTimes = { 16, 14, 13, 17 };
            var url = ServerSettings.Url;

            using (var db = new ModeloDados())
            {
                var esportes = db.Editoriais.Where(editoria =>

                    !editoria.excluido
                    && editoria.ativo
                    && editoria.esportes
                    && !listaIdTimes.Contains(editoria.id)
                    ).OrderBy(editoria => editoria.nome)

                    .Select(editoria => new EditoriaisViewModel
                    {
                        id = editoria.id,
                        titulo = editoria.nome,
                        chave = editoria.chave

                    }).ToList();

                var editoriais = db.Editoriais.Where(editoria =>

                    !editoria.excluido
                    && editoria.ativo
                    && !editoria.esportes

                    ).OrderBy(editoria => editoria.nome)

                    .Select(editoria => new EditoriaisViewModel
                    {
                        id = editoria.id,
                        titulo = editoria.nome,
                        chave = editoria.chave

                    }).ToList();

                var times = db.Editoriais.Where(editoria =>

                    !editoria.excluido
                    && editoria.ativo
                    && editoria.esportes
                    && listaIdTimes.Contains(editoria.id)
                    ).OrderBy(editoria => editoria.nome)

                    .Select(editoria => new TimesViewModel
                    {
                        id = editoria.id,
                        titulo = editoria.nome,
                        chave = editoria.chave,
                        logo = url + "/Admin/Conteudo/times/" + editoria.Times.FirstOrDefault().Id + "/original/" + editoria.Times.FirstOrDefault().Escudo

                    }).ToList();

                return new EditoriasViewModel { esportes = esportes, editoriais = editoriais, times = times };
            }
        }
    }
}
