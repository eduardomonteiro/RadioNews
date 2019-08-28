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
    public class BastidoresController : BaseController
    {
        // GET: Bastidores
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page")]
        public async Task<ActionResult> Videos(int? page)
        {
            var videos = await db.BastidoresMidias.Where(x => x.ativo && !x.excluido && x.video
           && x.idGaleria == 1 && x.Bastidores.chave == "bastidores-videos").OrderByDescending(x => x.dataCadastro).ToListAsync();

            if (videos == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int pageNumber = (page ?? 1);

            var pagination = videos.ToPagedList(pageNumber, 6);

            return View(pagination);
        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;chave")]
        public async Task<ActionResult> Fotos(string chaveBastidor, int? page)
        {
            var fotos = await db.Bastidores.Where(x => x.ativo && !x.excluido && x.BastidoresMidias.Any(w=>!w.video && !w.excluido))
                .OrderByDescending(x=>x.dataCadastro).ToListAsync();
            
            
            if (fotos == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(chaveBastidor))
            {
                var item = fotos.Where(x => x.chave == chaveBastidor).FirstOrDefault();
                fotos.Remove(item);
                fotos.Insert(0, item);
            }


            var pagination = fotos.ToPagedList(pageNumber, 6);

            if (pagination[0] == null)
            {
                return RedirectToAction("Fotos");
            }


            return View(pagination);
        }

    }
}