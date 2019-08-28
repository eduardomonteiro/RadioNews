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
    [CustomAuthorize(Roles = "Administrador,Programação")]
    public class CausasController : BaseController
    {
        //
        // GET: /Materias/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int programa = 0)
        {

			ViewBag.MateriaActive = "active";
            ViewBag.MateriaSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            var prog = db.Programacao.FirstOrDefault(a => a.id == programa);

            if (prog == null)
            {
                return HttpNotFound();
            }
            ViewBag.Programa = prog;
            return View(GetListItens(page, search, sortField, order,programa));

        }


		public void allClassOffSort()
        {

		
            ViewBag.SortClassid = "sorting";

            ViewBag.SortClassidProgramacao = "sorting";

            ViewBag.SortClassidAssunto = "sorting";

            ViewBag.SortClasstitulo = "sorting";

            ViewBag.SortClasschamada = "sorting";

            ViewBag.SortClasstexto = "sorting";

            ViewBag.SortClassfoto = "sorting";

            ViewBag.SortClassstatus = "sorting";

            ViewBag.SortClassdataCadastro = "sorting";

            ViewBag.SortClassexcluido = "sorting";


        }

		public IPagedList<Materia> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "",int programa = 0)
        {


            var list = db.Materia.Where(a=>!a.excluido);

            if (programa > 0)
            {
                list = list.Where(p => p.idProgramacao == programa);
            }

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = 0;
                if (int.TryParse(searchParam, out outDate))
                {
                    int.Parse(searchParam);
                    pagedList = pagedList.Where(g => g.id == id);
                }
                else
                {
                    pagedList = pagedList.Where(g => g.titulo.Contains(searchParam) || g.texto.Contains(searchParam));
                };

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

				case "idProgramacao":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.idProgramacao);
						ViewBag.SortClassidProgramacao = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.idProgramacao);
						ViewBag.SortClassidProgramacao = "sorting_desc";
					}
					break;

				case "idAssunto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.idAssunto);
						ViewBag.SortClassidAssunto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.idAssunto);
						ViewBag.SortClassidAssunto = "sorting_desc";
					}
					break;

				case "titulo":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.titulo);
						ViewBag.SortClasstitulo = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.titulo);
						ViewBag.SortClasstitulo = "sorting_desc";
					}
					break;

				case "chamada":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.chamada);
						ViewBag.SortClasschamada = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.chamada);
						ViewBag.SortClasschamada = "sorting_desc";
					}
					break;

				case "texto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.texto);
						ViewBag.SortClasstexto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.texto);
						ViewBag.SortClasstexto = "sorting_desc";
					}
					break;

				case "foto":
					if (order == "ASC")
					{
						pagedList = pagedList.OrderBy(m => m.foto);
						ViewBag.SortClassfoto = "sorting_asc";

					}
					else
					{
						pagedList = pagedList.OrderByDescending(m => m.foto);
						ViewBag.SortClassfoto = "sorting_desc";
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

            return pagedList.ToPagedList<Materia>(pageNumber, 10);
        }

        //
        // GET: /Materias/Details/5

        public ActionResult Details(int id = 0)
        {
            Materia materia = db.Materia.Find(id);
            ViewBag.Novidades = db.MateriaNovidades.Where(x => x.idMateria == id).ToList();

            if (materia == null)
            {
                return HttpNotFound();
            }
            return View(materia);
        }

        //
        // GET: /Materias/Create

        public ActionResult Create(int programa)
        {
            ViewBag.idAssunto = new SelectList(db.AssuntoMateria, "id", "assunto");

            var programaAtual = db.Programacao.FirstOrDefault(a => a.id == programa);
            ViewBag.ProgramaId = programa;

            ViewBag.idProgramacao = new SelectList(db.Programacao.Where(a => !a.excluido), "id", "nome", programaAtual.id);
            Materia materia = new Materia { idProgramacao = programaAtual.id };

            return View(materia);
        }

        //
        // POST: /Materias/Create
        
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Create(Materia materia, HttpPostedFileBase foto, int programa = 0)
        {
            if (ModelState.IsValid)
            {

                if (foto != null)
                {

                    int suffix = 0;

                    do
                    {
                        materia.chave = materia.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                        suffix++;
                    } while (db.Materia.Where(o => o.chave == materia.chave).Count() > 0);

                    //materia.idProgramacao = 14;
                    //materia.Programacao = db.Programacao.Where(x => x.id == 14).FirstOrDefault() ;
                    materia.dataCadastro = DateTime.Now;
                    materia.foto = foto.FileName;
                    db.Materia.Add(materia);

                    db.SaveChanges();

                    var pathCapa = Server.MapPath("~/conteudo/materias/" + materia.id + "/interna/");
                    materia.foto = Utils.SaveAndCropImage(foto, pathCapa, 0, 0, 167, 300);

                    db.Entry(materia).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "Causas", new { programa = programa });

                }
                               
            }


            ViewBag.idAssunto = new SelectList(db.AssuntoMateria, "id", "assunto");
            ViewBag.idProgramacao = new SelectList(db.Programacao.Where(a => !a.excluido), "id", "nome", materia.idProgramacao);
            return View(materia);
        }

        //
        // GET: /Materias/Edit/5

        public ActionResult Edit(int id = 0)
        {

            Materia materia = db.Materia.Find(id);

            if (materia == null)
            {
                return HttpNotFound();
            }

            ViewBag.idAssunto = new SelectList(db.AssuntoMateria, "id", "assunto", materia.idAssunto);
            ViewBag.idProgramacao = new SelectList(db.Programacao.Where(a => !a.excluido), "id", "nome", materia.idProgramacao);
            return View(materia);
        }

        //
        // POST: /Materias/Edit/5

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Edit(Materia materia, HttpPostedFileBase fotoNova)
        {
            if (ModelState.IsValid)
            {

                if (fotoNova != null)
                {
                    var pathCapa = Server.MapPath("~/conteudo/materias/" + materia.id + "/interna/");
                    materia.foto = Utils.SaveAndCropImage(fotoNova, pathCapa, 0, 0, 167, 300);
                }

                int suffix = 0;

                do
                {
                    materia.chave = materia.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == materia.chave).Count() > 0);

                //materia.idProgramacao = 14;
                //materia.Programacao = db.Programacao.Where(x => x.id == 14).FirstOrDefault();
                db.Entry(materia).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index", "Causas", new { programa = materia.idProgramacao });
            }

            ViewBag.idAssunto = new SelectList(db.AssuntoMateria, "id", "assunto", materia.idAssunto);
            ViewBag.idProgramacao = new SelectList(db.Programacao.Where(a => !a.excluido), "id", "nome", materia.idProgramacao);
            return View(materia);
        }

        //
        // GET: /Materias/Delete/5

        public ActionResult Delete(int id = 0)
        {

            Materia materia = db.Materia.Find(id);

            if (materia == null)
            {
                return HttpNotFound();
            }
            return View(materia);
        }

        //
        // POST: /Materias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {

            Materia materia = db.Materia.Find(id);

            materia.excluido = true;
            //db.Materia.Remove(materia);

            db.Entry(materia).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", new { programa = materia.idProgramacao });
        }

    }
}




