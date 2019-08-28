using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrativo.Models;
using Administrativo.Filters;
using System.Web.Security;

namespace Administrativo.Controllers
{
    [InitializeSimpleMembership]
    [Authorize]
    public class BaseController : Controller
    {
        
        //
        // GET: /Base/
        protected RadioCompanyNameContext db = new RadioCompanyNameContext();
        protected readonly UsersContext _db = new UsersContext();
        protected const string caminhoCapa = "~/Conteudo/Programas/Capa/";
        protected const string caminhoLogo = "~/Conteudo/Programas/Logo/";
        protected const string caminhoInterna = "~/Conteudo/Apresentadores/Internas/";
        protected const string caminhoListagem = "~/Conteudo/Apresentadores/Lista/";

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //FaleConosco
            ViewBag.QuantMensagens = int.Parse(db.FaleConosco.Where(x=> x.lida == 0 && !x.excluido).Count().ToString());
            ViewBag.Mensagem = db.FaleConosco.Where(x => x.lida == 0 && !x.excluido).OrderByDescending(x => x.dataCadastro).ToList();

            //Apresentadores
            ViewBag.caminhoListagem = caminhoListagem;
            ViewBag.caminhoInterna = caminhoInterna;

            //Programação
            ViewBag.caminhoCapa = caminhoCapa;
            ViewBag.caminhoLogo = caminhoLogo;

            ViewBag.EditoriasMenu = db.Editoriais.Where(a => !a.excluido).OrderBy(a => a.nome).ToList();

            ViewBag.EspeciaisECanais = db.Editoriais.Where(e => !e.excluido && (e.canal || e.especial)).OrderBy(a => a.nome).ToList();

            ViewBag.IsColunista = Roles.GetRolesForUser().Contains("Colunistas").ToString();
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

    }
}
