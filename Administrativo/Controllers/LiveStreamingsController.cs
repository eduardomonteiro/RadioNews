using Administrativo.Models;
using Administrativo.ViewModel;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    public class LiveStreamingsController : BaseController
    {
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", string especial = "")
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            AllClassOffSort();

            return View(GetListItens(page, search, sortField, order, especial));
        }

        public void AllClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassLegenda = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
            ViewBag.SortClassAtivo = "sorting";
        }

        public IPagedList<LiveStreamViewModel> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", string especial = "")
        {
            var list = db.LiveStreamings.ToList();

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int id;

                int.TryParse(searchParam, out id);

                pagedList = pagedList.Where(e => e.Id == id || e.Legenda.ToLower().Contains(searchParam.ToLower()) || e.CodigoTransmissao.ToLower().Contains(searchParam.ToLower()));
            }

            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Id);
                        ViewBag.SortClassid = "sorting_asc";
                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "Legenda":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Legenda);
                        ViewBag.SortClassLegenda = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Legenda);
                        ViewBag.SortClassLegenda = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                case "Ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Ativo);
                        ViewBag.SortClassAtivo = "sorting_asc";
                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Ativo);
                        ViewBag.SortClassAtivo = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(e => e.Ativo).ThenByDescending(e => e.Id);
                    ViewBag.SortClassAtivo = "sorting_asc";
                    break;
            }

            return pagedList.Select(live => new LiveStreamViewModel
            {
                Ativo = live.Ativo,
                Id = live.Id,
                DataCadastro = live.DataCadastro,
                DataFinalizacao = live.DataFinalizacao,
                CodigoTransmissao = live.CodigoTransmissao,
                Legenda = live.Legenda
            }).ToPagedList(pageNumber, 10);
        }

        public ActionResult Criar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Criar(LiveStreamViewModel viewModel)
        {
            var noticia = new Noticias
            {
                titulo = viewModel.Legenda,
                dataCadastro = DateTime.Now,
                dataAtualizacao = DateTime.Now,
                texto = viewModel.Legenda,
                chamada = viewModel.Legenda,
                excluido = true,
                liberado = false
            };

            db.LiveStreamings.Add(new LiveStreamings
            {
                Ativo = false,
                DataCadastro = DateTime.Now,
                DataFinalizacao = viewModel.DataFinalizacao,
                CodigoTransmissao = viewModel.CodigoTransmissao,
                Legenda = viewModel.Legenda,
                Noticias = noticia
            });

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Editar(int id)
        {
            var liveStream = db.LiveStreamings.Find(id);

            if (liveStream == null)
            {
                return RedirectToAction("Index");
            }

            return View(new LiveStreamViewModel
            {
                Ativo = liveStream.Ativo,
                DataCadastro = liveStream.DataCadastro,
                DataFinalizacao = liveStream.DataFinalizacao,
                Legenda = liveStream.Legenda,
                CodigoTransmissao = liveStream.CodigoTransmissao
            });
        }

        [HttpPost]
        public ActionResult Editar(LiveStreamViewModel viewModel)
        {
            var liveStream = db.LiveStreamings.Find(viewModel.Id);
            
            liveStream.Legenda = viewModel.Legenda;
            liveStream.CodigoTransmissao = viewModel.CodigoTransmissao;

            db.Entry(liveStream).State = EntityState.Modified;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Detalhes(int id)
        {
            var liveStream = db.LiveStreamings.Find(id);

            if (liveStream == null)
            {
                return RedirectToAction("Index");
            }

            return View(new LiveStreamViewModel
            {
                Id = liveStream.Id,
                Ativo = liveStream.Ativo,
                DataCadastro = liveStream.DataCadastro,
                DataFinalizacao = liveStream.DataFinalizacao,
                Legenda = liveStream.Legenda,
                CodigoTransmissao = liveStream.CodigoTransmissao
            });
        }

        [HttpPost]
        public ActionResult AtivarDesativar(int id)
        {
            var liveStream = db.LiveStreamings.Find(id);

            if (db.LiveStreamings.Any(live => live.Ativo && live.Id != id))
            {
                return Json(false);
            }

            liveStream.Ativo = !liveStream.Ativo;

            db.Entry(liveStream).State = EntityState.Modified;

            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public ActionResult ForcarAtivacao(int id)
        {
            var liveStream = db.LiveStreamings.Find(id);

            var liveDesativar = db.LiveStreamings.FirstOrDefault(live => live.Ativo && live.Id != id);

            liveDesativar.Ativo = !liveDesativar.Ativo;
            liveStream.Ativo = !liveStream.Ativo;

            db.Entry(liveDesativar).State = EntityState.Modified;
            db.Entry(liveStream).State = EntityState.Modified;

            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public ActionResult FinalizarStreaming(int id)
        {
            var liveStream = db.LiveStreamings.Find(id);

            if (liveStream.DataFinalizacao != null)
            {
                return Json(false);
            }

            liveStream.Ativo = false;
            liveStream.DataFinalizacao = DateTime.Now;

            db.Entry(liveStream).State = EntityState.Modified;

            db.SaveChanges();

            return Json(true);
        }
    }
}
