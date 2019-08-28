using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using PagedList;

namespace Site.Controllers
{
    public class CampeonatosController : BaseController
    {
        // GET: Campeonatos
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            return View();
        }
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult SerieA(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "campeonato-brasileiro-serie-a";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias do Campeonato Brasileiro - Série A";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "campeonato-brasileiro-serie-a"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            #endregion            

            var modelo = ObterNoticias(busca, "campeonato-brasileiro-serie-a", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult SerieB(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "campeonato-brasileiro-serie-b";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias do Campeonato Brasileiro - Série B";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "campeonato-brasileiro-serie-b"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            #endregion

            var modelo = ObterNoticias(busca, "campeonato-brasileiro-serie-b", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult Mineiro(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "campeonato-mineiro";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias do Campeonato Mineiro";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "campeonato-mineiro"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            #endregion

            var modelo = ObterNoticias(busca, "campeonato-mineiro", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult CopaDoBrasil(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "copa-do-brasil";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "copa-do-brasil"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            ViewBag.NoticiasDoCampeonato = "Notícias da Copa do Brasil";

            #endregion

            var modelo = ObterNoticias(busca, "copa-do-brasil", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult Eliminatorias(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "eliminatorias-da-copa";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "eliminatorias-da-copa"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            ViewBag.NoticiasDoCampeonato = "Notícias das Eliminatórias da Copa";

            #endregion

            var modelo = ObterNoticias(busca, "eliminatorias-da-copa", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult Olimpiadas(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "olimpiadas";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "olimpiadas"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            ViewBag.NoticiasDoCampeonato = "Notícias das Olimpíadas 2016";

            #endregion

            var modelo = ObterNoticias(busca, "olimpiadas", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        //[OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult Libertadores(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "copa-libertadores";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias da Taça Libertadores da América";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "copa-libertadores"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            #endregion

            var modelo = ObterNoticias(busca, "copa-libertadores", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult PrimeiraLiga(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "primeira-liga";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias da Primeira Liga";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "primeira-liga"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            #endregion

            var modelo = ObterNoticias(busca, "primeira-liga", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult CopaAmerica(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "copa-america";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias da Copa América";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "copa-america"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            #endregion

            var modelo = ObterNoticias(busca, "copa-america", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult Amistoso(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "amistoso";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias dos Jogos Amistosos";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "amistoso"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));
            #endregion

            var modelo = ObterNoticias(busca, "amistoso", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public ActionResult MostrarMais(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = editoria;
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;

            #endregion

            var modelo = ObterNoticias(busca, editoria, dataInicio, dataFim, page, categoria);
            return View(modelo);

        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria")]
        public IPagedList<Noticias> ObterNoticias(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            var dados = db.Noticias.Where(n => n.liberado && !n.excluido && n.dataAtualizacao < DateTime.Now);

            #region Filtros
            if (!string.IsNullOrEmpty(busca))
            {
                dados = dados.Where(n => n.titulo.Contains(busca));
            }
            if (!string.IsNullOrEmpty(editoria))
            {
                dados = dados.Where(n => n.Editoriais.Any(e => e.ativo && e.chave.Equals(editoria)));
            }

            if (categoria >0)
            {
                dados = dados.Where(n => n.Categorias.Any(e => e.Id == categoria));
            }
            if (!string.IsNullOrEmpty(dataInicio))
            {
                var data = DateTime.Parse(dataInicio);
                dados = dados.Where(n => n.dataCadastro >= data);
            }
            if (!string.IsNullOrEmpty(dataFim))
            {
                var data = DateTime.Parse(dataFim);
                dados = dados.Where(n => n.dataCadastro <= data);
            }
            #endregion

            var lista = dados.OrderByDescending(n => n.dataCadastro).ToPagedList(page ?? 1, 1);
            return lista;
        }

        public ActionResult SulAmericana(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = "sul-americana";
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;
            ViewBag.NoticiasDoCampeonato = "Notícias da Copa Sul-Americana";
            ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == "sul-americana"), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            #endregion

            var modelo = ObterNoticias(busca, "sul-americana", dataInicio, dataFim, page, categoria);
            return View(modelo);
        }
    }
}