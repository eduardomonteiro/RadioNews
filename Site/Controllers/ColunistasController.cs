using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using Site.ViewModels;
using PagedList;

namespace Site.Controllers
{
    public class ColunistasController : BaseController
    {
        // GET: Colunistas

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public async Task<ActionResult> Index()
        {
            var colunistas = await db.Colunista.Where(x => !x.excluido && x.liberado).OrderBy(a => a.nome).ToListAsync();

            if (colunistas == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(colunistas);
        }


        [OutputCache(Duration = 120, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "colunistaSlug;editoria;palavrachave;datainicio;datafinal;page")]
        public async Task<ActionResult> Detalhes(int? page, string colunistaSlug, string editoria, string palavrachave, string datainicio, string datafinal)
        {
            ViewBag.editoria = new SelectList(db.Editoriais.Where(x => x.ativo && !x.excluido && !x.esportes && !x.especial), "chave", "nome");

            var colunista = await db.Colunista.Where(x => !x.excluido && x.liberado && x.chave == colunistaSlug).FirstOrDefaultAsync();

            if (colunista == null)
            {
                return RedirectToAction("Index", "Colunistas");
            }

            int pageNumber = (page ?? 1);

            var query = colunista.Noticias.Where(noticia => !noticia.excluido && noticia.dataAtualizacao < DateTime.Now && noticia.liberado).AsQueryable();

            if (!string.IsNullOrEmpty(palavrachave))
            {
                query = query.Where(n => (n.titulo != null && n.titulo.ToUpper().Contains(palavrachave.ToUpper())) || (n.chamada != null && n.chamada.ToUpper().Contains(palavrachave.ToUpper())));
            }

            if (!string.IsNullOrEmpty(editoria))
            {
                query = query.Where(n => n.Editoriais.Any(p => p.chave == editoria));
            }

            if (!string.IsNullOrEmpty(datainicio))
            {
                DateTime DataInicio = DateTime.Parse(datainicio).AddHours(0).AddMinutes(0).AddSeconds(0);

                query = query.Where(d => d.dataCadastro >= DataInicio);
            }

            if (!string.IsNullOrEmpty(datafinal))
            {
                DateTime DataFim = DateTime.Parse(datafinal).AddHours(23).AddMinutes(59).AddSeconds(59);

                query = query.Where(d => d.dataCadastro <= DataFim);
            }


            var viewModel = new ColunistaMateriasViewModel
            {
                Colunista = colunista,
                paginacao = query.ToList().OrderByDescending(x => x.dataAtualizacao).ToPagedList(pageNumber, 8)
            };

            return View(viewModel);

        }


        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave;colunistaSlug")]
        public async Task<ActionResult> Materia(string chave, string colunistaSlug)
        {
            var colunista = await db.Noticias.Where(x => !x.excluido && x.liberado && x.url == chave).FirstOrDefaultAsync();

            if (colunista == null)
            {
                return RedirectToAction("Detalhes", "Colunistas", new { chave = colunistaSlug });
            }

            colunista.visualizacao = colunista.visualizacao + 1;
            db.Entry(colunista).State = EntityState.Modified;
            await db.SaveChangesAsync();

            return View(colunista);
        }

        public PartialViewResult SidebarColunista(Colunista colunista)
        {
            var lista = db.Colunista.Where(x => !x.excluido && x.liberado && x.id != colunista.id).OrderBy(x => x.Ordem);

            var ultimas = db.Noticias.Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.idColunista != null).OrderByDescending(x => x.Colunista.Ordem).Select(x => x.idColunista).ToArray();

            var viewModel = new ColunistaSideBarViewModel
            {
                Colunista = colunista,
                ListaColunista = lista.Where(x => ultimas.Contains(x.id)).Select(x => new ColunistaVM
                {
                    Chave = x.chave,
                    Foto = x.foto,
                    Nome = x.nome,
                    Titulo = x.Noticias.OrderByDescending(n => n.dataCadastro).FirstOrDefault().titulo,
                    Id = x.id,
                    Url = x.Noticias
                           .OrderByDescending(noticia => noticia.dataAtualizacao)
                           .FirstOrDefault(c => !c.excluido && c.liberado && c.dataAtualizacao < DateTime.Now).url,
                }).ToList()
                //ListaColunista = lista.OrderByDescending(x => x.Noticias != null ? x.Noticias.OrderByDescending(y => y.dataAtualizacao) : null).ToList(),
            };

            return PartialView("_SidebarColunista", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ColunistaNews(ColunistaSeguidores seguidor)
        {

            if (ModelState.IsValid)
            {
                var hasExist = db.ColunistaSeguidores.Where(x => x.Email == seguidor.Email && x.ColunistaId == seguidor.ColunistaId).FirstOrDefault();

                if (hasExist != null)
                {
                    return Json(new { result = "already" });
                }

                seguidor.DataCadastro = DateTime.Now;
                db.ColunistaSeguidores.Add(seguidor);
                try
                {
                    db.SaveChanges();
                    return Json(new { result = "success" });
                }
                catch (Exception ex)
                {
                    return Json(new { result = "error-" + ex.Message });
                }

            }
            else
            {
                return Json(new { result = "invalid" });
            }
        }

    }


}