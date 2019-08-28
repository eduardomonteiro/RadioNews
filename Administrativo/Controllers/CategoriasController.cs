using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;

namespace Administrativo.Controllers
{
    
    public class CategoriasController : BaseController
    {
        [HttpPost]
        public bool ReOrder(int[] ids)
        {
            if (ids != null)
            {

                foreach (var id in ids.Select((value, i) => new { i, value }))
                {
                    Categorias categoria = db.Categorias.Find(id.value);
                    categoria.Ordem = id.i;
                    db.Entry(categoria).State = EntityState.Modified;

                }
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        //
        // GET: /Categorias/

        public ActionResult Index(int editorial,int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.CategoriasActive = "active";
            ViewBag.CategoriasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.id == editorial);

            return View(GetListItens(editorial,page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassEditoriaId = "sorting";
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Categorias> GetListItens(int editorial,int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editorial);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(c => c.Id == id || c.Titulo.ToLower().Contains(searchParam.ToLower()) || c.Editoriais.nome.ToLower().Contains(searchParam.ToLower()));

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
                case "EditoriaId":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.EditoriaId);
                        ViewBag.SortClassEditoriaId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.EditoriaId);
                        ViewBag.SortClassEditoriaId = "sorting_desc";
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
                    pagedList = pagedList.OrderBy(c => c.Ordem);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Categorias>(pageNumber, 10);
        }

        //
        // GET: /Categorias/Details/5

        public ActionResult Details(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }

            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.id == categorias.EditoriaId);
            return View(categorias);
        }

        //
        // GET: /Categorias/Create

        public ActionResult Create(int editorial)
        {
            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", editorial);

            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.id == editorial);
            return View();
        }

        //
        // POST: /Categorias/Create

        [HttpPost]
        public ActionResult Create(Categorias categorias, int editorial)
        {
            if (ModelState.IsValid)
            {
                categorias.DataCadastro = DateTime.Now;
                db.Categorias.Add(categorias);
                db.SaveChanges();
                return RedirectToAction("Index", "Categorias", new { editorial = editorial });
            }

            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", categorias.EditoriaId);
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.id == editorial);
            return View(categorias);
        }

        //
        // GET: /Categorias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", categorias.EditoriaId);
            
            return View(categorias);
        }

        //
        // POST: /Categorias/Edit/5

        [HttpPost]
        public ActionResult Edit(Categorias categorias)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categorias).State = EntityState.Modified;
                db.Entry(categorias).Property("DataCadastro").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index", "Categorias", new { editorial = categorias.EditoriaId });
            }
            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", categorias.EditoriaId);
            return View(categorias);
        }

        //
        // GET: /Categorias/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Categorias categorias = db.Categorias.Find(id);
            if (categorias == null)
            {
                return HttpNotFound();
            }
            return View(categorias);
        }

        //
        // POST: /Categorias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Categorias categorias = db.Categorias.Find(id);
            var idEditorial = categorias.EditoriaId;
            categorias.Excluido = true;
            
            db.Entry(categorias).State = EntityState.Modified;
            db.Entry(categorias).Property(b=>b.DataCadastro).IsModified = false;
            db.Entry(categorias).Property(b => b.Ordem).IsModified = false;
            db.Entry(categorias).Property(b => b.destaque).IsModified = false;

            db.SaveChanges();
            return RedirectToAction("Index", "Categorias", new { editorial = idEditorial });
        }

    }
}




