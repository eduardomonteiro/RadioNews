using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrativo.Models;
using Administrativo.Commons;
using System.Data;
using System.Data.Entity;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Eventos")]
    public class EventosController : BaseController
    {
        //
        // GET: /Eventos/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.EditoriaisActive = "active";
            ViewBag.EditoriaisSubActive = "active";
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
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Eventos> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.Eventos.Where(evt => !evt.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(e => e.Id == id || e.Titulo.ToLower().Contains(searchParam.ToLower()));

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
                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Titulo);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Titulo);
                        ViewBag.SortClassnome = "sorting_desc";
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
                default:
                    pagedList = pagedList.OrderByDescending(e => e.Id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Eventos>(pageNumber, 10);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Eventos evento, HttpPostedFileBase imageFile)
        {
            if (imageFile == null)
            {
                ModelState.AddModelError("imagemArquivo", "A imagem é obrigatória.");
            }


            if (ModelState.IsValid)
            {
                evento.DataCadastro = DateTime.Now;

                if (imageFile != null)
                {
                    evento.Imagem = Utils.SaveAndCropImage(imageFile, Server.MapPath("~/Conteudo/Eventos/Foto/"), 0, 0, 744, 500);
                }

                db.Eventos.Add(evento);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(evento);
        }

        public ActionResult Edit(int id = 0)
        {
            var evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }

            return View(evento);
        }

        public ActionResult Delete(int id = 0)
        {
            var evento = db.Eventos.Find(id);
            if (evento == null)
            {
                return HttpNotFound();
            }
            return View(evento);
        }

        //
        // POST: /Categorias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var evento = db.Eventos.Find(id);

            evento.Excluido = true;

            db.Entry(evento).State = EntityState.Modified;
            db.Entry(evento).Property(b => b.DataCadastro).IsModified = false;

            db.SaveChanges();
            return RedirectToAction("Index", "Eventos");
        }

        [HttpPost]
        public ActionResult Edit(Eventos evento, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {                
                db.Entry(evento).State = EntityState.Modified;

                if (imageFile != null)
                {
                    evento.Imagem = Utils.SaveAndCropImage(imageFile, Server.MapPath("~/Conteudo/Eventos/Foto/"), 0, 0, 744, 500);
                }

                db.Entry(evento).Property(propriedades => propriedades.DataCadastro).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(evento);
        }

        public ActionResult Details(int id = 0)
        {
            var eventos = db.Eventos.FirstOrDefault(a => a.Id == id && !a.Excluido);

            if (eventos == null)
            {
                return HttpNotFound();
            }
            return View(eventos);
        }

    }
}
