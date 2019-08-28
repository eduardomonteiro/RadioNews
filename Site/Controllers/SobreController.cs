using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Site.ViewModels;

namespace Site.Controllers
{
    public class SobreController : BaseController
    {
        // GET: Sobre
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult QuemSomos()
        {
            return View();
        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult TrofeuGuara()
        {
            var idNoticia = db.Noticias.SingleOrDefault(noticia => noticia.chamada == "trofeu-guara" && noticia.url == "trofeu-guara")?.id;

            ViewBag.IdNoticia = idNoticia;

            return View();
        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult CopaCompanyName()
        {
            var idNoticia = db.Noticias.SingleOrDefault(noticia => noticia.chamada == "copa-CompanyName" && noticia.url == "copa-CompanyName")?.id;

            ViewBag.IdNoticia = idNoticia;

            return View();
        }

        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Equipe()
        {
            var hoje = DateTime.Now;
            var equipes = await db.Equipe.Where(x => !x.excluido).ToListAsync();
            var banner = await db.Banners.Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-equipe" && w.Ativo.Value)).FirstOrDefaultAsync();

            var viewModel = new EquipeViewModel
            {
                Banner = banner,
                Equipes = equipes
            };
            
            return View(viewModel);
        }


        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page")]
        public async Task<ActionResult> MidiaKit(int? page)
        {
            var midias = await db.MidiaKit.Where(x => x.Ativo && !x.Excluido).OrderBy(x => x.Titulo).ToListAsync();

            if (midias == null)
            {
                return RedirectToAction("Index","Home");
            }

            int pageNumber = (page ?? 1);

            var pagination = midias.ToPagedList(pageNumber, 8);

            return View(pagination);
        }



        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "filename")]
        public ActionResult Download(string filename)
        {
            var document = Server.MapPath("/admin/conteudo/MidiaKit/" + filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(document);
            string fileName = filename;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

    }
}