using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

using Administrativo.Models;
using System.Web.UI.WebControls;
using Administrativo.Commons;
using System.IO;



namespace Administrativo.Controllers
{

    public class NovidadesController : BaseController
    {

        //
        // GET: /Novidades/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

			ViewBag.MateriaNovidadesActive = "active";
            ViewBag.MateriaNovidadesSubActive = "active";
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

            ViewBag.SortClassidMateria = "sorting";

            ViewBag.SortClassstatus = "sorting";

            ViewBag.SortClassdataPrograma = "sorting";

            ViewBag.SortClassnovidades = "sorting";

            ViewBag.SortClassaudio = "sorting";

            ViewBag.SortClassdataCadastro = "sorting";

            ViewBag.SortClassexcluido = "sorting";


        }

		public IPagedList<MateriaNovidades> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.MateriaNovidades;

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(m => m.id == id);

            }


            switch (sortField)
            {

				case "id":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.id);
						ViewBag.SortClassid = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.id);
						ViewBag.SortClassid = "sorting_desc";
					}
					break;

				case "idMateria":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.idMateria);
						ViewBag.SortClassidMateria = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.idMateria);
						ViewBag.SortClassidMateria = "sorting_desc";
					}
					break;

				case "status":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.status);
						ViewBag.SortClassstatus = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.status);
						ViewBag.SortClassstatus = "sorting_desc";
					}
					break;

				case "dataPrograma":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.dataPrograma);
						ViewBag.SortClassdataPrograma = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.dataPrograma);
						ViewBag.SortClassdataPrograma = "sorting_desc";
					}
					break;

				case "novidades":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.novidades);
						ViewBag.SortClassnovidades = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.novidades);
						ViewBag.SortClassnovidades = "sorting_desc";
					}
					break;

				case "audio":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.audio);
						ViewBag.SortClassaudio = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.audio);
						ViewBag.SortClassaudio = "sorting_desc";
					}
					break;

				case "dataCadastro":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.dataCadastro);
						ViewBag.SortClassdataCadastro = "sorting_desc";
					}
					break;

				case "excluido":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.excluido);
						ViewBag.SortClassexcluido = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.excluido);
						ViewBag.SortClassexcluido = "sorting_desc";
					}
					break;

                default:
                    pagedList = pagedList.OrderByDescending(m => m.id);
					ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<MateriaNovidades>(pageNumber, 10);
        }

        //
        // GET: /Novidades/Details/5

        public ActionResult Details(int id = 0)
        {

            var materianovidades = db.MateriaNovidades.Include("Materia").FirstOrDefault(a => a.id == id);

            if (materianovidades == null)
            {
                return HttpNotFound();
            }
            return View(materianovidades);
        }

        //
        // GET: /Novidades/Create

        public ActionResult Create(int idMateria = 0)
        {

            ViewBag.Materia = db.Materia.Where(x => x.id == idMateria).FirstOrDefault();
            return View();
        }

        //
        // POST: /Novidades/Create

        [HttpPost]
        public ActionResult Create(MateriaNovidades materianovidades, HttpPostedFileBase audio)
        {
            if (ModelState.IsValid)
            {
                if (audio != null)
                {
                    var path = Server.MapPath("~/conteudo/materias/" + materianovidades.idMateria + "/audios/");
                    materianovidades.audio = Utils.SaveFileBase(path, audio);

                }

                materianovidades.Materia = db.Materia.Where(x => x.id == materianovidades.idMateria).FirstOrDefault();
                materianovidades.dataCadastro = DateTime.Now;
                db.MateriaNovidades.Add(materianovidades);

                db.SaveChanges();
                return RedirectToAction("Details", "Causas", new { id = materianovidades.idMateria });
            }


            ViewBag.Materia = db.Materia.Where(x => x.id == materianovidades.idMateria).FirstOrDefault();

            return View(materianovidades);
        }

        //
        // GET: /Novidades/Edit/5

        public ActionResult Edit(int id = 0)
        {

            MateriaNovidades materianovidades = db.MateriaNovidades.Find(id);
            ViewBag.Materia = db.Materia.Where(x => x.id == materianovidades.idMateria).FirstOrDefault();

            if (materianovidades == null)
            {
                return HttpNotFound();
            }

            ViewBag.idMateria = new SelectList(db.Materia.Where(a => !a.excluido), "id", "titulo", materianovidades.idMateria);

            return View(materianovidades);
        }

        //
        // POST: /Novidades/Edit/5

        [HttpPost]
        public ActionResult Edit(MateriaNovidades materianovidades, HttpPostedFileBase audioNovo)
        {
            if (ModelState.IsValid)
            {
                if (audioNovo != null)
                {
                    var path = Server.MapPath("~/conteudo/materias/" + materianovidades.idMateria + "/audios/");
                    materianovidades.audio = Utils.SaveFileBase(path, audioNovo);
                }
                else
                {
                    materianovidades.audio = materianovidades.audio;
                }

                materianovidades.Materia = db.Materia.Where(x => x.id == materianovidades.idMateria).FirstOrDefault();
                materianovidades.dataCadastro = DateTime.Now;

                db.Entry(materianovidades).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Details", "Causas", new { id = materianovidades.idMateria });
            }

            ViewBag.idMateria = new SelectList(db.Materia.Where(a => !a.excluido), "id", "titulo", materianovidades.idMateria);

            return View(materianovidades);
        }

        //
        // GET: /Novidades/Delete/5

        public ActionResult Delete(int id = 0)
        {

            MateriaNovidades materianovidades = db.MateriaNovidades.Find(id);
            ViewBag.Materia = db.Materia.Where(x => x.id == materianovidades.idMateria).FirstOrDefault();

            if (materianovidades == null)
            {
                return HttpNotFound();
            }
            return View(materianovidades);
        }

        //
        // POST: /Novidades/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            MateriaNovidades materianovidades = db.MateriaNovidades.Find(id);
            var idMateria = materianovidades.idMateria;
            db.MateriaNovidades.Remove(materianovidades);

            db.SaveChanges();
            return RedirectToAction("Details", "Causas", new { id = materianovidades.idMateria });
        }

    }
}




