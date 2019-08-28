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

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Tags")]
    public class TagsController : BaseController
    {
 
        //
        // GET: /Tags/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.TagsActive = "active";
            ViewBag.TagsSubActive = "active";
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

            ViewBag.SortClassExcluido = "sorting";

            ViewBag.SortClassDataCadastro = "sorting";

            ViewBag.SortClasschave = "sorting";


        }

		public IPagedList<Tags> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Tags;

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
                    pagedList = pagedList.Where(g => g.Titulo.Contains(searchParam));
                };

            }


            switch (sortField)
            {

				case "Id":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.Id);
						ViewBag.SortClassId = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.Id);
						ViewBag.SortClassId = "sorting_desc";
					}
					break;

				case "Titulo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.Titulo);
						ViewBag.SortClassTitulo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.Titulo);
						ViewBag.SortClassTitulo = "sorting_desc";
					}
					break;

				case "Excluido":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.Excluido);
						ViewBag.SortClassExcluido = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.Excluido);
						ViewBag.SortClassExcluido = "sorting_desc";
					}
					break;

				case "DataCadastro":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.DataCadastro);
						ViewBag.SortClassDataCadastro = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.DataCadastro);
						ViewBag.SortClassDataCadastro = "sorting_desc";
					}
					break;

				case "chave":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.chave);
						ViewBag.SortClasschave = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.chave);
						ViewBag.SortClasschave = "sorting_desc";
					}
					break;

                default:
                    pagedList = pagedList.OrderByDescending(t => t.Id);
					ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Tags>(pageNumber, 10);
        }

        //
        // GET: /Tags/Details/5

        public ActionResult Details(int id = 0)
        {

            Tags tags = db.Tags.Find(id);

            if (tags == null)
            {
                return HttpNotFound();
            }
            return View(tags);
        }

        //
        // GET: /Tags/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /Tags/Create

        [HttpPost]
        public ActionResult Create(Tags tags)
        {
            if (ModelState.IsValid)
            {

                tags.DataCadastro = DateTime.Now;
                int suffix = 0;

                do
                {
                    tags.chave = tags.Titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == tags.chave).Count() > 0);

                db.Tags.Add(tags);

                db.SaveChanges();
                return RedirectToAction("Index");
            }


            return View(tags);
        }

        //
        // GET: /Tags/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Tags tags = db.Tags.Find(id);

            if (tags == null)
            {
                return HttpNotFound();
            }

            return View(tags);
        }

        //
        // POST: /Tags/Edit/5

        [HttpPost]
        public ActionResult Edit(Tags tags)
        {
            if (ModelState.IsValid)
            {
                int suffix = 0;

                do
                {
                    tags.chave = tags.Titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == tags.chave).Count() > 0);
                db.Entry(tags).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tags);
        }

        //
        // GET: /Tags/Delete/5

        public ActionResult Delete(int id = 0)
        {

            Tags tags = db.Tags.Find(id);

            if (tags == null)
            {
                return HttpNotFound();
            }
            return View(tags);
        }

        //
        // POST: /Tags/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            Tags tags = db.Tags.Find(id);
            db.Tags.Remove(tags);

            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}




