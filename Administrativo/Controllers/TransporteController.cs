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
    [CustomAuthorize(Roles = "Administrador,Seu Caminho")]
    public class TransporteController : BaseController
    {

        //
        // GET: /Transporte/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.TransporteActive = "active";
            ViewBag.TransporteSubActive = "active";
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

            ViewBag.SortClassCssClass = "sorting";

            ViewBag.SortClassStatus = "sorting";

            ViewBag.SortClassTexto = "sorting";

            ViewBag.SortClassExcluido = "sorting";

            ViewBag.SortClassDataCadastro = "sorting";


        }

		public IPagedList<Transporte> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Transporte.OrderByDescending(x=> x.Id);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(t => t.Id == id || t.Titulo.ToLower().Contains(searchParam.ToLower()));

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

				case "CssClass":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.CssClass);
						ViewBag.SortClassCssClass = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.CssClass);
						ViewBag.SortClassCssClass = "sorting_desc";
					}
					break;

				case "Status":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.Status);
						ViewBag.SortClassStatus = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.Status);
						ViewBag.SortClassStatus = "sorting_desc";
					}
					break;

				case "Texto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(t => t.Texto);
						ViewBag.SortClassTexto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(t => t.Texto);
						ViewBag.SortClassTexto = "sorting_desc";
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

                default:
                    pagedList = pagedList.OrderByDescending(t => t.Id);
					ViewBag.SortClassId = "sorting_asc";
                    break;
            }
            ViewBag.RotasNoRio = db.RotasNoRio.OrderByDescending(x=>x.Atualizacao).ToList();
            return pagedList.ToPagedList<Transporte>(pageNumber, 10);
        }

        //
        // GET: /Transporte/Details/5

        public ActionResult Details(int id = 0)
        {

            Transporte transporte = db.Transporte.Find(id);

            if (transporte == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
            //return View(transporte);
        }

        //
        // GET: /Transporte/Create

        public ActionResult Create()
        {
            return RedirectToAction("Index");
            //return View();
        }

        //
        // POST: /Transporte/Create

        [HttpPost]
        public ActionResult Create(Transporte transporte)
        {
            if (ModelState.IsValid)
            {

               
                db.Transporte.Add(transporte);

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
            //return View(transporte);
        }

        //
        // GET: /Transporte/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Transporte transporte = db.Transporte.Find(id);

            if (transporte == null)
            {
                return HttpNotFound();
            }

            return View(transporte);
        }

        //
        // POST: /Transporte/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Transporte transporte)
        {
            if (ModelState.IsValid)
            {
                if (transporte.Status == "Bom")
                {
                    transporte.Texto = null;
                }

                db.Entry(transporte).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(transporte);
        }

        //
        // GET: /Transporte/Delete/5

        public ActionResult Delete(int id = 0)
        {

            Transporte transporte = db.Transporte.Find(id);

            if (transporte == null)
            {
                return HttpNotFound();
            }
            return View(transporte);
        }

        //
        // POST: /Transporte/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            Transporte transporte = db.Transporte.Find(id);
            db.Transporte.Remove(transporte);

            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}




