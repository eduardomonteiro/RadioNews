using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class GaleriasController : BaseController
    {
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;chave")]
        public async Task<ActionResult> Index(string chave, int? page)
        {
            var fotos = await db.Noticias.Where(x => x.liberado && !x.excluido && x.dataAtualizacao < DateTime.Now && x.Galeria != null)
                  .Select(n => n.Galeria).Where(g => !g.excluido && g.ativo && g.Midias.Any(m => !m.excluido && !m.video))
                  .Distinct().OrderByDescending(x => x.dataCadastro).ToListAsync();


            if (fotos == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(chave))
            {
                var item = fotos.Where(x => x.chave == chave).FirstOrDefault();
                fotos.Remove(item);
                fotos.Insert(0, item);
            }

            var pagination = fotos.ToPagedList(pageNumber, 6);

            return View(pagination);
        }


        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;chave")]
        public async Task<ActionResult> Videos(string chave, int? page)
        {
            //var idsGalerias =  db.Noticias.Where(x => x.liberado && !x.excluido && x.Galeria != null).Select(n => n.Galeria).Where(g => !g.excluido).Select(x => x.id).ToArray();

            var videos = await db.Noticias.Where(x => x.liberado && !x.excluido && x.dataAtualizacao < DateTime.Now && x.Galeria != null)
                  .Select(n => n.Galeria).Where(g => !g.excluido && g.ativo && g.Midias.Any(m => !m.excluido && m.video))
                  .OrderByDescending(x => x.dataCadastro).ToListAsync();

            if (videos == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (!string.IsNullOrEmpty(chave))
            {
                var item = videos.Where(x => x.chave == chave).FirstOrDefault();
                videos.Remove(item);
                videos.Insert(0, item);
            }

            int pageNumber = (page ?? 1);

            var pagination = videos.ToPagedList(pageNumber, 6);

            return View(pagination);
        }
    }
}