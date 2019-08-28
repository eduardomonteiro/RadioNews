using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using System.IO;
using Administrativo.Commons;
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Galerias")]
    public class GaleriaController : BaseController
    {
        public int areaADM = 14;
        //
        // GET: /Galeria/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.GaleriaActive = "active";
            ViewBag.GaleriaSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }


		public void allClassOffSort()
        {

		    ViewBag.SortClassid = "sorting";
            ViewBag.SortClasstitulo = "sorting";
            ViewBag.SortClasschamada = "sorting";
            ViewBag.SortClasstexto = "sorting";
            ViewBag.SortClassexcluido = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

		public IPagedList<Galeria> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Galeria.Where(x => !x.excluido && !x.Fixa);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = 0;
                    if(int.TryParse(searchParam, out outDate))
                    {
                        int.Parse(searchParam);
                        pagedList = pagedList.Where(g => g.id == id);
                    }
                    else
                    {
                        pagedList = pagedList.Where(g => g.titulo.ToLower().Contains(searchParam.ToLower()) || g.texto.ToLower().Contains(searchParam.ToLower()) || g.chamada.ToLower().Contains(searchParam.ToLower()));
                    };
            }


            switch (sortField)
            {
				case "id":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.id);
						ViewBag.SortClassid = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.id);
						ViewBag.SortClassid = "sorting_desc";
					}
					break;
				case "titulo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.titulo);
						ViewBag.SortClasstitulo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.titulo);
						ViewBag.SortClasstitulo = "sorting_desc";
					}
					break;
				case "chamada":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.chamada);
						ViewBag.SortClasschamada = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.chamada);
						ViewBag.SortClasschamada = "sorting_desc";
					}
					break;
				case "texto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.texto);
						ViewBag.SortClasstexto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.texto);
						ViewBag.SortClasstexto = "sorting_desc";
					}
					break;
				case "excluido":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.excluido);
						ViewBag.SortClassexcluido = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.excluido);
						ViewBag.SortClassexcluido = "sorting_desc";
					}
					break;
				case "dataCadastro":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(g => g.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(g => g.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_desc";
					}
					break;
                default:
                    pagedList = pagedList.OrderByDescending(g => g.id);
					ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Galeria>(pageNumber, 10);
        }

        //
        // GET: /Galeria/Details/5

        public ActionResult Details(int id = 0)
        {
            Galeria galeria = db.Galeria.Find(id);
            if (galeria == null)
            {
                return HttpNotFound();
            }
            ViewBag.fotos = galeria.Midias.Where(f => !f.excluido);
            return View(galeria);
        }

        //
        // GET: /Galeria/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Galeria/Create

        [HttpPost]
        public ActionResult Create(Galeria galeria)
        {
            if (ModelState.IsValid)
            {
                galeria.excluido = false;
                galeria.dataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {
                    galeria.chave = galeria.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == galeria.chave).Count() > 0);

                db.Galeria.Add(galeria);
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, galeria.id);
                return RedirectToAction("Edit", new { id=galeria.id });
            }

            return View(galeria);
        }

        //
        // GET: /Galeria/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Galeria galeria = db.Galeria.Find(id);
            if (galeria == null)
            {
                return HttpNotFound();
            }

            ViewBag.fotos = galeria.Midias.Where(f => !f.excluido && f.video == false);
            ViewBag.videos = galeria.Midias.Where(f => !f.excluido && f.video == true);

            return View(galeria);
        }

        //
        // POST: /Galeria/Edit/5

        [HttpPost]
        public ActionResult Edit(Galeria galeria)
        {
            
            if (ModelState.IsValid)
            {
                int suffix = 0;

                do
                {
                    galeria.chave = galeria.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == galeria.chave).Count() > 0);

                db.Entry(galeria).State = EntityState.Modified;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, galeria.id);
                return RedirectToAction("Index");
            }
            return View(galeria);
        }

        //
        // GET: /Galeria/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Galeria galeria = db.Galeria.Find(id);
            
            if (galeria == null)
            {
                return HttpNotFound();
            }
            return View(galeria);
        }       

        //
        // POST: /Galeria/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Galeria galeria = db.Galeria.Find(id);
            db.Entry(galeria).Collection("Midias").Load();
            galeria.Midias.Clear();

            db.Galeria.Remove(galeria);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, galeria.id);
            return RedirectToAction("Index");
        }

    }
}




