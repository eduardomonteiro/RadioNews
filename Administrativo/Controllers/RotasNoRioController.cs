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
    public class RotasNoRioController : BaseController
    {

        //
        // GET: /RotasNoRio/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.RotasNoRioActive = "active";
            ViewBag.RotasNoRioSubActive = "active";
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

            ViewBag.SortClassStatus = "sorting";

            ViewBag.SortClassTexto = "sorting";

            ViewBag.SortClassExcluido = "sorting";

            ViewBag.SortClassDataCadastro = "sorting";


        }

		public IPagedList<RotasNoRio> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.RotasNoRio;

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
                    pagedList = pagedList.Where(g => g.Texto.ToLower().Contains(searchParam.ToLower()) || g.Titulo.ToLower().Contains(searchParam.ToLower()));
                };

            }


            switch (sortField)
            {

				case "Id":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.Id);
						ViewBag.SortClassId = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.Id);
						ViewBag.SortClassId = "sorting_desc";
					}
					break;

				case "Titulo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.Titulo);
						ViewBag.SortClassTitulo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.Titulo);
						ViewBag.SortClassTitulo = "sorting_desc";
					}
					break;

				case "Status":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.Status);
						ViewBag.SortClassStatus = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.Status);
						ViewBag.SortClassStatus = "sorting_desc";
					}
					break;

				case "Texto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.Texto);
						ViewBag.SortClassTexto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.Texto);
						ViewBag.SortClassTexto = "sorting_desc";
					}
					break;

				case "Excluido":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.Excluido);
						ViewBag.SortClassExcluido = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.Excluido);
						ViewBag.SortClassExcluido = "sorting_desc";
					}
					break;

				case "DataCadastro":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(r => r.DataCadastro);
						ViewBag.SortClassDataCadastro = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(r => r.DataCadastro);
						ViewBag.SortClassDataCadastro = "sorting_desc";
					}
					break;

                default:
                    pagedList = pagedList.OrderByDescending(r => r.Id);
					ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<RotasNoRio>(pageNumber, 10);
        }

        //
        // GET: /RotasNoRio/Details/5

        public ActionResult Details(int id = 0)
        {

            RotasNoRio rotasnorio = db.RotasNoRio.Find(id);

            if (rotasnorio == null)
            {
                return HttpNotFound();
            }
            return View(rotasnorio);
        }

        //
        // GET: /RotasNoRio/Create

        public ActionResult Create()
        {

            return View();
        }

        //
        // POST: /RotasNoRio/Create

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(RotasNoRio rotasnorio)
        {
            if (ModelState.IsValid)
            {
                rotasnorio.DataCadastro = DateTime.Now;
                rotasnorio.Atualizacao = DateTime.Now;

                if(rotasnorio.Status == "Bom")
                {
                    rotasnorio.Texto = null;
                }
                
                db.RotasNoRio.Add(rotasnorio);

                db.SaveChanges();
                return RedirectToAction("Index", "Transporte");
            }


            return View(rotasnorio);
        }

        //
        // GET: /RotasNoRio/Edit/5

        public ActionResult Edit(int id = 0)
        {

            RotasNoRio rotasnorio = db.RotasNoRio.Find(id);

            if (rotasnorio == null)
            {
                return HttpNotFound();
            }

            return View(rotasnorio);
        }

        //
        // POST: /RotasNoRio/Edit/5

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(RotasNoRio rotasnorio)
        {
            if (ModelState.IsValid)
            {
                if (rotasnorio.Status == "Bom")
                {
                    rotasnorio.Texto = null;
                }
                rotasnorio.Atualizacao = DateTime.Now;

                db.Entry(rotasnorio).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Transporte");
            }

            return View(rotasnorio);
        }

        //
        // GET: /RotasNoRio/Delete/5

        public ActionResult Delete(int id = 0)
        {

            RotasNoRio rotasnorio = db.RotasNoRio.Find(id);

            if (rotasnorio == null)
            {
                return HttpNotFound();
            }
            return View(rotasnorio);
        }

        //
        // POST: /RotasNoRio/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            RotasNoRio rotasnorio = db.RotasNoRio.Find(id);
            db.RotasNoRio.Remove(rotasnorio);

            db.SaveChanges();
            return RedirectToAction("Index", "Transporte");
        }

    }
}




