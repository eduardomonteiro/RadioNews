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
    [Authorize]
    public class CategoriaAudiosController : BaseController
    {
        //
        // GET: /CategoriaAudios/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.CategoriasActive = "active";
            ViewBag.CategoriasSubActive = "active";
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
            ViewBag.SortClassDescricao = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<CategoriasAudios> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {

            var list = db.CategoriasAudios.Where(a => !a.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(c => c.Id == id || c.Descricao.ToLower().Contains(searchParam.ToLower()));

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
                case "Descricao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Descricao);
                        ViewBag.SortClassDescricao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Descricao);
                        ViewBag.SortClassDescricao = "sorting_desc";
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
        public ActionResult Create(CategoriasAudios categoria)
        {
            categoria.DataCadastro = DateTime.Now;
            db.Entry(categoria).State = EntityState.Added;



            db.SaveChanges();

            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {

            var categoria = db.CategoriasAudios.FirstOrDefault(cat => cat.Id == id && !cat.Excluido);

            if (categoria == null)
            {
                return RedirectToAction("Index");
            }

            return View(categoria);

        }

        [HttpPost]
        public ActionResult Edit(CategoriasAudios categoria)
        {
            db.Entry(categoria).State = EntityState.Modified;
            db.Entry(categoria).Property(a => a.DataCadastro).IsModified = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var categoria = db.CategoriasAudios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido);

            if (categoria == null)
            {
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        public ActionResult Form(CategoriasAudios model)
        {
            //ViewBag.Colecoes = db.ColecoesAudios.Select(colecao => new SelectListItem { Text = colecao.Titulo, Value = colecao.Id.ToString() }).ToList();
            //ViewBag.Categorias = db.CategoriasAudios.Select(categoria => new SelectListItem { Text = categoria.Descricao, Value = categoria.Id.ToString() }).ToList();
            return PartialView("_Form", model);
        }


        public ActionResult Delete(int id)
        {
            var categoria = db.CategoriasAudios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido);

            if (categoria == null)
            {
                return RedirectToAction("Index");
            }

            return View(categoria);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var categoria = db.CategoriasAudios.Find(id);

            categoria.Excluido = true;
            db.Entry(categoria).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }





    }
}
