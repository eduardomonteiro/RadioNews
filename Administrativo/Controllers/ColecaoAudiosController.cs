using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    public class ColecaoAudiosController : BaseController
    {
        //
        // GET: /ColecaoAudios/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.CategoriasActive = "active";
            ViewBag.CategoriasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            AllClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }

        public void AllClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassDescricao = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<ColecoesAudios> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {

            var list = db.ColecoesAudios.Where(a => !a.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int id;
                int.TryParse(searchParam, out id);

                pagedList = pagedList.Where(c => c.Id == id || c.Titulo.ToLower().Contains(searchParam.ToLower()));

            }

            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;
                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Titulo);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Titulo);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(c => c.Id);
                    ViewBag.SortClassId = "sorting_desc";
                    break;
            }

            return pagedList.ToPagedList(pageNumber, 10);
        }

        [HttpGet]
        public ActionResult Create()
        {

            return View();

        }

        [HttpPost]
        public ActionResult Create(ColecoesAudios colecao)
        {
            colecao.DataCadastro = DateTime.Now;
            db.Entry(colecao).State = EntityState.Added;
            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var colecaoAudios = db.ColecoesAudios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido);

            if (colecaoAudios == null)
            {
                return RedirectToAction("Index");
            }

            return View(colecaoAudios);

        }

        [HttpPost]
        public ActionResult Edit(ColecoesAudios colecao)
        {
            db.Entry(colecao).State = EntityState.Modified;
            db.Entry(colecao).Property(a => a.DataCadastro).IsModified = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var colecaoAudios = db.ColecoesAudios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido);

            if (colecaoAudios == null)
            {
                return RedirectToAction("Index");
            }

            return View(colecaoAudios);
        }     
        
        public ActionResult Form(ColecoesAudios model)
        {
            //ViewBag.Colecoes = db.ColecoesAudios.Select(colecao => new SelectListItem { Text = colecao.Titulo, Value = colecao.Id.ToString() }).ToList();
            ViewBag.Categorias = db.CategoriasAudios.Where(cat => !cat.Excluido && cat.Liberado).OrderBy(cat => cat.Descricao).Select(categoria => new SelectListItem { Text = categoria.Descricao, Value = categoria.Id.ToString() }).ToList();
            return PartialView("_Form", model);
        }


        public ActionResult Delete(int id)
        {
            var colecao = db.ColecoesAudios.FirstOrDefault(col => col.Id == id && !col.Excluido);

            if (colecao == null)
            {
                return RedirectToAction("Index");
            }

            return View(colecao);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var colecao = db.ColecoesAudios.Find(id);

            colecao.Excluido = true;
            db.Entry(colecao).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
