using PagedList;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebGrease.Css.Extensions;

namespace Site.Controllers
{
    public class PodcastController : BaseController
    {
        // GET: Podcast

        public PodcastController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }


        [OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;editoria;palavrachave;datainicio;datafinal")]
        public ActionResult Index(int? page, string editoria, string palavrachave, string datainicio, string datafinal)
        {

            var audios = db.Noticias.Include(not=>not.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.audio != null && x.idColunista == null).OrderByDescending(x => x.dataAtualizacao).AsQueryable();

            int pageNumber = (page ?? 1);


            if (!string.IsNullOrEmpty(palavrachave))
            {
                audios = audios.Where(n => n.titulo.ToLower().Contains(palavrachave.ToLower()) || n.chamada.Contains(palavrachave)
                || n.subtitulo.ToLower().Contains(palavrachave));

            }

            if (!string.IsNullOrEmpty(editoria))
            {
                audios = audios.Where(n => n.Editoriais.Any(p => p.chave == editoria));
            }

            if (!string.IsNullOrEmpty(datainicio))
            {
                DateTime DataInicio = DateTime.Parse(datainicio);
                audios = audios.Where(d => DbFunctions.TruncateTime(d.dataCadastro) >= DataInicio.Date);
            }


            if (!string.IsNullOrEmpty(datafinal))
            {
                DateTime DataFim = DateTime.Parse(datafinal);
                audios = audios.Where(d => DbFunctions.TruncateTime(d.dataCadastro) <= DataFim.Date);
            }


            var pagination = audios.ToPagedList(pageNumber, 12);


            ViewBag.editoria = new SelectList(db.Editoriais.Where(x => x.ativo && !x.excluido), "chave", "nome", (!string.IsNullOrEmpty(editoria) ? editoria : null));

            return View(pagination);

        }

        [OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id;page;palavrachave;datainicio;datafinal")]
        public ActionResult CategoriaAudio(int id, int? page, string palavrachave, string datainicio, string datafinal)
        {
            var categoriaModel = db.CategoriasAudios.FirstOrDefault(cat => cat.Id == id && !cat.Excluido && cat.Liberado);

            var colecoes = db.ColecoesAudios.Where(col => col.CategoriaId == categoriaModel.Id && !col.Excluido && col.Liberado);

            if (!string.IsNullOrEmpty(palavrachave))
            {
                colecoes = colecoes.Where(n => n.Titulo.ToLower().Contains(palavrachave.ToLower()));
            }

            if (!string.IsNullOrEmpty(datainicio))
            {
                DateTime DataInicio = DateTime.Parse(datainicio);
                colecoes = colecoes.Where(d => DbFunctions.TruncateTime(d.DataCadastro) >= DataInicio.Date);
            }

            if (!string.IsNullOrEmpty(datafinal))
            {
                DateTime DataFim = DateTime.Parse(datafinal);
                colecoes = colecoes.Where(d => DbFunctions.TruncateTime(d.DataCadastro) <= DataFim.Date);
            }

            var categoriaViewModel = new CategoriaAudiosViewModel
            {
                Id = categoriaModel.Id,
                Descricao = categoriaModel.Descricao,
                DataCadastro = categoriaModel.DataCadastro,
                Colecoes = colecoes.OrderByDescending(x => x.DataCadastro).ToPagedList(page ?? 1, 12)

            };

            categoriaViewModel.Colecoes.ForEach(col => col.Audios = db.Audios.Where(aud => aud.ColecaoId == col.Id && !aud.Excluido && aud.Liberado).ToList());
            

            if (categoriaViewModel == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Horoscopo = categoriaViewModel.Descricao == "Horóscopo";

            return View(categoriaViewModel);
        }

        //[OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Horoscopo()
        {
            var horoscopo = db.Horoscopo.OrderBy(h => h.Signo).ToList();

            if (horoscopo.Any())
                return View(horoscopo);
            else
                return RedirectToAction("Index", "Podcast");
        }
    }
}