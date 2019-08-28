using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Site.ViewModels;
using Site.Models;

namespace Site.Controllers
{
    public class EsportesController : BaseController
    {
        // GET: Esportes
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            return RedirectToAction("Index","Editorias",new { chave= "esporte" });
            //var hoje = DateTime.Now.Date;

            //#region DEclaração de performance
            //var noticiasGeneral = db.Noticias.Where(x => !x.excluido && x.liberado).OrderByDescending(x => x.dataCadastro);
            //var bannersGeneral = db.Banners.Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim);
            //#endregion

            //var banner = await bannersGeneral.Where(x => x.AreaBanner.Any(w => w.chave == "banner-esportes-geral" && w.Ativo.Value)).FirstOrDefaultAsync();
            ////var slider = await noticiasGeneral.Where(x => x.Colunista == null && (x.destaqueEditoria || x.destaque) && x.foto != null && x.Editoriais.Any(p => p.esportes || p.chave == "esporte")).OrderByDescending(x => x.id).ToListAsync();

            //var slider = new List<Noticias>();

            //var destaques1 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.TipoDestaque == Enums.TipoDestaque.Interna1 && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //if (destaques1 == null)
            //{
            //    destaques1 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //}
            //slider.Add(destaques1);

            //var destaques2 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.TipoDestaque == Enums.TipoDestaque.Interna2 && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //if (destaques2 == null)
            //{
            //    destaques2 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //}
            //slider.Add(destaques2);

            //var destaques3 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.TipoDestaque == Enums.TipoDestaque.Interna3 && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //if (destaques3 == null)
            //{
            //    destaques3 = await noticiasGeneral.FirstOrDefaultAsync(x => x.Colunista == null && x.foto != null && x.Editoriais.Any(p => p.esportes || p.chave == "esporte"));
            //}
            //slider.Add(destaques3);

            //var notIds1 = slider.Select(x => x.id).ToArray();

            //var destaques = await noticiasGeneral.Where(x => !notIds1.Contains(x.id) && x.Colunista == null &&
            //x.Editoriais.Any(p => p.esportes || p.chave == "esporte")).Take(2).ToListAsync();

            //var notIds2 = destaques.Select(x => x.id).ToArray();

            //var not23 = notIds1.Union(notIds2);

            //var america = await noticiasGeneral.Where(x => !not23.Contains(x.id) &&
            //x.Editoriais.Any(p => p.esportes && p.chave == "america")).OrderByDescending(x => x.dataAtualizacao).Take(2).ToListAsync();


            //var atletico = await noticiasGeneral.Where(x => !not23.Contains(x.id) &&
            //x.Editoriais.Any(p => p.esportes && p.chave == "atletico")).OrderByDescending(x => x.dataAtualizacao).Take(2).ToListAsync();
            
            //var cruzeiro = await noticiasGeneral.Where(x => !not23.Contains(x.id) && x.Editoriais.Any(p => p.esportes && p.chave == "cruzeiro")).OrderByDescending(x => x.dataAtualizacao).Take(2).ToListAsync();
            
            //var brasil = await noticiasGeneral.Where(x => !not23.Contains(x.id) &&
            //x.Editoriais.Any(p => p.esportes && p.chave == "selecao-brasileira")).OrderByDescending(x => x.dataAtualizacao).Take(2).ToListAsync();
            
            //not23 = not23.Union(america.Select(x => x.id).ToArray());
            //not23 = not23.Union(atletico.Select(x => x.id).ToArray());
            //not23 = not23.Union(cruzeiro.Select(x => x.id).ToArray());
            //not23 = not23.Union(brasil.Select(x => x.id).ToArray());

            //var editoriaEsportes = await noticiasGeneral.Where(x => !not23.Contains(x.id) && x.Colunista == null &&
            //x.Editoriais.Any(p => p.chave == "esporte")).OrderByDescending(x => x.dataAtualizacao).Take(6).ToListAsync();

            //var view = new EsportesViewModel
            //{
            //    Banner = banner,
            //    NoticiasSlider = slider,
            //    NoticiasAmerica = america,
            //    NoticiasAtletico = atletico,
            //    NoticiasBrasil = brasil,
            //    NoticiasCruzeiro = cruzeiro,
            //    NoticiasDestaque = destaques,
            //    NoticiasEditoriaEsportes = editoriaEsportes

            //};

            //return View(view);


        }
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Clubes()
        {
            return View();
        }
    }
}