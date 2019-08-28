using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using Administrativo.Commons;
using System.Web.Script.Serialization;
using Administrativo.Helpers;
using Administrativo.Enums;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Banners")]
    public class BannerController : BaseController
    {
        public int areaADM = 3;
        //
        // GET: /Banner/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            db.Configuration.LazyLoadingEnabled = false;

            ViewBag.BannersActive = "active";
            ViewBag.BannersSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();


            return View(GetListItens(page, search, sortField, order));

        }


        public void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";

            ViewBag.SortClassTitulo = "sorting";

            ViewBag.SortClassArquivo = "sorting";

            ViewBag.SortClassAnunciante = "sorting";

            ViewBag.SortClassLink = "sorting";

            ViewBag.SortClassTipoArquivo = "sorting";

            ViewBag.SortClassExibicoesMaximo = "sorting";

            ViewBag.SortClassExibicoes = "sorting";

            ViewBag.SortClassExcluido = "sorting";

            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<Banners> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Banners.Include("BannersVisualizacoesCliques").Where(x => !x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {

                int outDate = 0;
                int id = 0;
                if (int.TryParse(searchParam, out outDate))
                {
                    int.Parse(searchParam);
                    pagedList = pagedList.Where(g => g.Id == id);
                }
                else
                {
                    pagedList = pagedList.Where(g => g.Titulo.ToLower().Contains(searchParam.ToLower()) || g.Anunciante.ToLower().Contains(searchParam.ToLower()));
                };

            }


            switch (sortField)
            {

                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;

                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Titulo);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Titulo);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;

                case "Arquivo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Arquivo);
                        ViewBag.SortClassArquivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Arquivo);
                        ViewBag.SortClassArquivo = "sorting_desc";
                    }
                    break;

                case "Anunciante":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Anunciante);
                        ViewBag.SortClassAnunciante = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Anunciante);
                        ViewBag.SortClassAnunciante = "sorting_desc";
                    }
                    break;

                case "Link":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Link);
                        ViewBag.SortClassLink = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Link);
                        ViewBag.SortClassLink = "sorting_desc";
                    }
                    break;

                case "TipoArquivo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.TipoArquivo);
                        ViewBag.SortClassTipoArquivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.TipoArquivo);
                        ViewBag.SortClassTipoArquivo = "sorting_desc";
                    }
                    break;

                case "Excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Excluido);
                        ViewBag.SortClassExcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Excluido);
                        ViewBag.SortClassExcluido = "sorting_desc";
                    }
                    break;

                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(b => b.Id);
                    ViewBag.SortClassId = "sorting_asc";
                    break;

            }

            return pagedList.ToPagedList<Banners>(pageNumber, 10);
        }

        //
        // GET: /Banner/Details/5

        public ActionResult Details(int id = 0)
        {
            Banners banners = db.Banners.Include("AreaBanner").FirstOrDefault(a => a.Id == id);

            ViewBag.Tamanho = banners.AreaBanner.FirstOrDefault().Tamanho;
            ViewBag.Tamanho2 = banners.AreaBanner.FirstOrDefault().Tamanho2;

            if (banners == null)
            {
                return HttpNotFound();
            }
            return View(banners);
        }

        //
        // GET: /Banner/Create

        public ActionResult Create()
        {
            List<AreaBanner> areas = db.AreaBanner.Where(x => x.Ativo.Value).OrderBy(x => x.Titulo).ToList();

            List<object> listaArea = new List<object>();

            listaArea.Add(new { Text = "Selecione...", Value = "" });

            foreach (var a in areas.OrderBy(a => a.Titulo))
            {
                listaArea.Add(new { Text = a.Tamanho + " - " + a.Titulo + " - " + a.Descricao, Value = a.Id });
            }

            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(new SelectListItem { Selected = true, Text = "Selecione...", Value = "" });

            ViewBag.Tamanhos = new SelectList(listaArea, "Value", "Text");

            ViewBag.List = new SelectList(
            new List<SelectListItem>
            {
                new SelectListItem { Selected = true, Text = "Selecione...", Value = ""},
                new SelectListItem { Selected = false, Text = "Imagem", Value = "false"},
                new SelectListItem { Selected = false, Text = "Flash", Value = "true"},
            }, "Value", "Text");

            return View();
        }

        //
        // POST: /Banner/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Banners banners, HttpPostedFileBase Arquivo, HttpPostedFileBase Arquivo2, int[] Area, int? Tamanhos)
        {
            if (Tamanhos == null)
            {
                ModelState.AddModelError("Tamanho", "Escolha um tamanho.");
            }
            if (ModelState.IsValid)
            {
                Banners banner = new Banners
                {
                    Anunciante = banners.Anunciante,
                    Arquivo = Arquivo?.FileName,
                    Arquivo2 = Arquivo2?.FileName,
                    DataCadastro = DateTime.Now,
                    DataFim = banners.DataFim,
                    DataInicio = banners.DataInicio,
                    Excluido = false,
                    Exibicoes = 0,
                    Cliques = 0,
                    Link = banners.Link,
                    TipoArquivo = banners.TipoArquivo,
                    Titulo = banners.Anunciante,
                    Local = Tamanhos,
                    Liberado = banners.Liberado,
                    Html = banners.Html
                };

                #region add area
                if (Tamanhos.HasValue)
                {
                    AreaBanner Areas = db.AreaBanner.Where(x => x.Id == Tamanhos).FirstOrDefault();
                    banner.AreaBanner.Add(Areas);
                }
                #endregion

                if (banner.Html != string.Empty)
                {
                    banner.Arquivo = SalvarArquivo(Arquivo, Tamanhos, 0);

                    banner.Arquivo2 = SalvarArquivo(Arquivo2, Tamanhos, 1);

                }

                db.Banners.Add(banner);

                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, banner.Id);

                return RedirectToAction("Index");
            }

            List<AreaBanner> areas = db.AreaBanner.Where(x => x.Ativo.Value).OrderBy(x => x.Titulo).ToList();

            List<object> listaArea = new List<object>();

            listaArea.Add(new { Text = "Selecione...", Value = "" });

            foreach (var a in areas)
            {
                listaArea.Add(new { Text = a.Tamanho + " - " + a.Titulo + " - " + a.Descricao, Value = a.Id });
            }

            List<SelectListItem> lista = new List<SelectListItem>();

            lista.Add(new SelectListItem { Selected = true, Text = "Selecione...", Value = "" });

            ViewBag.Tamanhos = new SelectList(listaArea, "Value", "Text");
            return View(banners);
        }

        private string SalvarArquivo(HttpPostedFileBase Arquivo, int? idAreaBanner = 0, int index = 0)
        {
            string retorno = "";
            if (Arquivo != null)
            {
                string Tamanho;
                if (index == 0)
                    Tamanho = db.AreaBanner.Find(idAreaBanner).Tamanho;
                else
                    Tamanho = db.AreaBanner.Find(idAreaBanner).Tamanho2;

                var tamanho = Tamanho.Split('x');

                var pathArquivoTemp = Server.MapPath("~/Conteudo/Temp/");
                var arquivoTempFinal = System.IO.Path.Combine(pathArquivoTemp, Arquivo.FileName);
                Arquivo.SaveAs(arquivoTempFinal);

                if (!Arquivo.FileName.Contains(".gif"))
                {
                    var pathArquivo = Server.MapPath("~/Conteudo/Banners/" + Tamanho + "/");
                    retorno = Utils.resizeImageAndSave3(arquivoTempFinal, int.Parse(tamanho[0]), int.Parse(tamanho[1]), pathArquivo);
                }
                else
                {
                    var pathArquivo = Server.MapPath("~/Conteudo/Banners/" + Tamanho + "/");
                    retorno = Utils.SaveFileBase(pathArquivo, Arquivo);
                }
            }
            return retorno;

        }

        //
        // GET: /Banner/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Banners banner = db.Banners.Find(id);

            if (banner == null)
            {
                return HttpNotFound();
            }


            List<AreaBanner> areas = db.AreaBanner.Where(x => x.Ativo.Value).OrderBy(x => x.Titulo).ToList();

            List<object> listaArea = new List<object>();

            //  listaArea.Add(new { Text = "Selecione...", Value = "" });
            var tamanhoSelecionado = "";

            if (banner.AreaBanner != null)
            {
                tamanhoSelecionado = banner.AreaBanner.Select(a => a.Tamanho).First().ToString();
            }
            string tamanhoSelecionado2 = null;

            var banners = banner.AreaBanner.Select(a => a.Tamanho2).First();
            if (banners != null)
            {
                tamanhoSelecionado2 = banner.AreaBanner.Select(a => a.Tamanho2).First().ToString();
            }

            foreach (var a in areas.Where(a=> a.Tamanho == tamanhoSelecionado && a.Tamanho2 == tamanhoSelecionado2).OrderBy(a => a.Titulo).ToList())
            {
                listaArea.Add(new { Text = a.Tamanho + " - " + a.Titulo + " - " + a.Descricao, Value = a.Id });
            }


            ViewBag.tamanhoSelecionado = tamanhoSelecionado;
            ViewBag.tamanhoSelecionado2 = tamanhoSelecionado2;

            ViewBag.Tamanho = new SelectList(listaArea, "Value", "Text", banner.AreaBanner.FirstOrDefault().Id);
            ViewBag.TamanhoId = banner.AreaBanner.FirstOrDefault().Id;

            return View(banner);
        }

        //
        // POST: /Banner/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Banners banners, HttpPostedFileBase arquivoUpload, HttpPostedFileBase Arquivo2, int[] Area, int Tamanho)
        {
            if (ModelState.IsValid)
            {
                db.Entry(banners).State = EntityState.Modified;


                #region areas
                db.Entry(banners).Collection("AreaBanner").Load();
                banners.AreaBanner.Clear();

                var dbApresent = db.AreaBanner.Find(Tamanho);
                banners.AreaBanner.Add(dbApresent);
                #endregion

                if (banners.Html != string.Empty)
                {
                    if (arquivoUpload != null)
                    {
                        banners.Arquivo = SalvarArquivo(arquivoUpload, Tamanho, 0);
                    }
                    if (Arquivo2 != null)
                    {
                        banners.Arquivo2 = SalvarArquivo(Arquivo2, Tamanho, 1);
                    }
                }
                banners.Titulo = banners.Anunciante;
                db.Entry(banners).Property("Exibicoes").IsModified = false;
                db.Entry(banners).Property("Cliques").IsModified = false;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, banners.Id);

                return RedirectToAction("Index");
            }

            return View(banners);
        }


        public ActionResult DashBoard(int Id, string data = null)
        {
            DateTime today = DateTime.Now;

            int quantidadeVisualizacoes = 0, quantidadeCliques = 0;

            var visualizacoesQuery = db.BannersVisualizacoesCliques.Where(x => x.CodigoBanner == Id && x.Visualizacao && !x.Clique).AsQueryable();
            var cliquesQuery = db.BannersVisualizacoesCliques.Where(x => x.CodigoBanner == Id && !x.Visualizacao && x.Clique).AsQueryable();

            if (!string.IsNullOrEmpty(data))
            {
                DateTime dataInicial, dataFinal;

                string[] dataGeral = data.Split(new char[] { '-' });
                dataInicial = Convert.ToDateTime(dataGeral[0] + " 00:00:00");
                dataFinal = Convert.ToDateTime(dataGeral[1] + " 23:59:59");

                visualizacoesQuery = visualizacoesQuery.Where(x => x.DataCadastro >= dataInicial && x.DataCadastro <= dataFinal);
                quantidadeVisualizacoes = visualizacoesQuery.Count();

                cliquesQuery = cliquesQuery.Where(x => x.DataCadastro >= dataInicial && x.DataCadastro <= dataFinal);
                quantidadeCliques = cliquesQuery.Count();
            }
            else
            {
                quantidadeCliques = cliquesQuery.Count();
                quantidadeVisualizacoes = visualizacoesQuery.Count();
            }

            var banner = db.Banners.FirstOrDefault(x => !x.Excluido && x.Id == Id);

            if (banner != null)
            {
                var dataInicial = DateTime.Now.AddMonths(-12);

                var valorVisualizacao = db.BannersVisualizacoesCliques
                              .Where(x => x.CodigoBanner == Id && x.Visualizacao && x.DataCadastro >= dataInicial && x.DataCadastro <= today)
                              .ToList();


                var valorClique = db.BannersVisualizacoesCliques
                              .Where(x => x.CodigoBanner == Id && x.Clique && x.DataCadastro >= dataInicial && x.DataCadastro <= today)
                              .ToList();



                var graficoVisualizacao = (from c in valorVisualizacao
                                           group c by new
                                           {
                                               c.DataCadastro.Month,
                                               c.CodigoBanner

                                           } into g
                                           orderby
                                         (long)g.Count() descending
                                           select new ViewModel.ItensGrafico
                                           {
                                               valor = (long)g.Count(),
                                               texto = g.Key.Month.MonthName()
                                           }).ToList();

                graficoVisualizacao.Reverse();


                var graficoClique = (from c in valorClique
                                     group c by new
                                     {
                                         c.DataCadastro.Month,
                                         c.CodigoBanner

                                     } into g
                                     orderby
                                   (long)g.Count() descending
                                     select new ViewModel.ItensGrafico
                                     {
                                         valor = (long)g.Count(),
                                         texto = g.Key.Month.MonthName()
                                     }).ToList();

                graficoClique.Reverse();

                #region add list
                List<ViewModel.ItensGrafico> list = new List<ViewModel.ItensGrafico>();

                var obj = new ViewModel.ItensGrafico();
                obj.texto = "Visualizacoes";
                obj.valor = quantidadeVisualizacoes;
                list.Add(obj);

                var obj1 = new ViewModel.ItensGrafico();
                obj1.texto = "Cliques";
                obj1.valor = quantidadeCliques;

                list.Add(obj1);
                #endregion

                var viewModel = new ViewModel.DashboardViewModel
                {
                    grafico1Cliques = graficoClique,
                    grafico1Visualizacoes = graficoVisualizacao,
                    qtdCliques = quantidadeCliques,
                    qtdVisualizacoes = quantidadeVisualizacoes,
                    grafico2 = list
                };

                ViewBag.Id = Id;
                return View(viewModel);
            }

            return RedirectToAction("Details", "Banner", new { Id = Id });
        }

        //
        // GET: /Banner/Delete/5

        public ActionResult Delete(int id = 0)
        {

            Banners banners = db.Banners.Find(id);

            if (banners == null)
            {
                return HttpNotFound();
            }
            return View(banners);
        }

        //
        // POST: /Banner/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Banners banners = db.Banners.Find(id);

            db.Entry(banners).State = EntityState.Modified;

            /*
            db.Entry(banners).Collection("AreaBanner").Load();

            banners.AreaBanner.Clear();

            db.Banners.Remove(banners);
            */
            banners.Excluido = true;
            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, banners.Id);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Areas(string tamanho)
        {

            var resultado = (from c in db.AreaBanner
                             where c.Tamanho.ToLower().Contains(tamanho.ToLower())
                             select new ResultAreas
                             {
                                 Id = c.Id,
                                 Titulo = c.Titulo,
                             }).ToList();

            return Json(resultado);
        }

        public class ResultAreas
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
        }

        public JsonResult DuasImagens(int areaId)
        {
            string tamanho2 = db.AreaBanner.Where(a => a.Id == areaId).Select(a => a.Tamanho2).FirstOrDefault();
            if (string.IsNullOrEmpty(tamanho2))
                return Json("false");
            else
                return Json("true");
        }



    }
}




