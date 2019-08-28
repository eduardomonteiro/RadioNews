using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/colunistas")]
    public class ColunistasController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObterColunistas()
        {
            using (var db = new ModeloDados())
            {
                var colunistas = db.Colunista.OrderBy(colunista => colunista.nome).Select(colunista => new { colunista.id, colunista.nome, colunista.email, colunista.descricao, colunista.foto, colunista.sexo }).ToList();

                return Json(colunistas);
            }
        }
    }
}
