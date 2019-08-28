using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using Site.Helpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    public class AcontecimentosController : BaseController
    {
        public int areaADM = 36;
        //
        // GET: /Acontecimentos/

        //[HttpPost]
        //public bool ReOrder(int[] ids)
        //{
        //    if (ids != null)
        //    {

        //        foreach (var id in ids.Select((value, i) => new { i, value }))
        //        {
        //            var acontecimento = db.Acontecimentos.Find(id.value);
        //            acontecimento.Ordem = id.i;
        //            db.Entry(acontecimento).State = EntityState.Modified;

        //        }
        //        db.SaveChanges();
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //
        // GET: /Categorias/

        public ActionResult Index(int evento, int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.CategoriasActive = "active";
            ViewBag.CategoriasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            ViewBag.Evento = db.Eventos.FirstOrDefault(a => a.Id == evento && !a.Excluido);

            return View(GetListItens(evento, page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassEditoriaId = "sorting";
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
            ViewBag.SortClassHoraInicio = "sorting";
            ViewBag.SortClassHoraFim = "sorting";
            ViewBag.SortClassData = "sorting";

        }

        public IPagedList<Acontecimentos> GetListItens(int evento, int? page, string searchParam = "", string sortField = "", string order = "")
        {

            var list = db.Acontecimentos.Where(a => a.EventoId == evento && !a.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(c => c.Id == id || c.Local.ToLower().Contains(searchParam.ToLower()) || c.Eventos.Titulo.ToLower().Contains(searchParam.ToLower()));

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
                case "EventoId":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.EventoId);
                        ViewBag.SortClassEditoriaId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.EventoId);
                        ViewBag.SortClassEditoriaId = "sorting_desc";
                    }
                    break;
                case "Local":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Local);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Local);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;
                case "HoraInicio":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.HoraInicio);
                        ViewBag.SortClassHoraInicio = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.HoraInicio);
                        ViewBag.SortClassHoraInicio = "sorting_desc";
                    }
                    break;
                case "HoraFim":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.HoraFim);
                        ViewBag.SortClassHoraFim = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.HoraFim);
                        ViewBag.SortClassHoraFim = "sorting_desc";
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
                case "Data":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Data);
                        ViewBag.SortClassData = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Data);
                        ViewBag.SortClassData = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderBy(c => c.Data);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Acontecimentos>(pageNumber, 10);
        }

        public ActionResult Create(int evento)
        {
            ViewBag.EventoId = new SelectList(db.Eventos.Where(evt => !evt.Excluido), "Id", "Titulo", evento);
            ViewBag.Evento = db.Eventos.FirstOrDefault(a => a.Id == evento && !a.Excluido);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Acontecimentos acontecimento, int evento)
        {
            if (!string.IsNullOrWhiteSpace(acontecimento.FlickrId))
            {
                var flickr = new CompanyNameFlickr();

                var imagens = flickr.PhotosetsGetPhotos(acontecimento.FlickrId, 1, 1);

                if (imagens == null)
                {
                    ModelState.AddModelError("FlickrId", "Não foi encontrado um álbum com esse flickrId.");
                }

            }

            if (ModelState.IsValid)
            {
                acontecimento.DataCadastro = DateTime.Now;
                db.Acontecimentos.Add(acontecimento);
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, acontecimento.Id);
                return RedirectToAction("Index", "Acontecimentos", new { evento = evento });
            }

            ViewBag.EventoId = new SelectList(db.Eventos.Where(evt => !evt.Excluido), "Id", "Titulo", evento);
            ViewBag.Evento = db.Eventos.FirstOrDefault(a => a.Id == evento && !a.Excluido);

            return View(acontecimento);

        }

        public ActionResult Edit(int id = 0)
        {
            var acontecimento = db.Acontecimentos.Find(id);
            if (acontecimento == null)
            {
                return HttpNotFound();
            }

            ViewBag.EventoId = new SelectList(db.Eventos.Where(evt => !evt.Excluido), "Id", "Titulo", acontecimento.EventoId);

            return View(acontecimento);
        }

        //
        // POST: /Categorias/Edit/5

        [HttpPost]
        public ActionResult Edit(Acontecimentos acontecimento)
        {

            if (!string.IsNullOrWhiteSpace(acontecimento.FlickrId))
            {
                var flickr = new CompanyNameFlickr();

                var imagens = flickr.PhotosetsGetPhotos(acontecimento.FlickrId, 1, 1);

                if (imagens == null)
                {
                    ModelState.AddModelError("FlickrId", "Não foi encontrado um álbum com esse flickrId.");
                }
            }

            if (ModelState.IsValid)
            {
                db.Entry(acontecimento).State = EntityState.Modified;
                db.Entry(acontecimento).Property("DataCadastro").IsModified = false;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, acontecimento.Id);
                return RedirectToAction("Index", "Acontecimentos", new { evento = acontecimento.EventoId });
            }
            ViewBag.EventoId = new SelectList(db.Eventos, "Id", "Titulo", acontecimento.EventoId);
            return View(acontecimento);
        }

        public ActionResult Details(int id = 0)
        {
            var acontecimento = db.Acontecimentos.Find(id);
            if (acontecimento == null)
            {
                return HttpNotFound();
            }

            ViewBag.Evento = db.Eventos.FirstOrDefault(a => a.Id == acontecimento.EventoId);
            return View(acontecimento);
        }


        public ActionResult Delete(int id = 0)
        {
            var acontecimento = db.Acontecimentos.Find(id);
            if (acontecimento == null)
            {
                return HttpNotFound();
            }
            return View(acontecimento);
        }

        //
        // POST: /Categorias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var acontecimento = db.Acontecimentos.Find(id);
            var idEvento = acontecimento.EventoId;

            acontecimento.Excluido = true;

            db.Entry(acontecimento).State = EntityState.Modified;
            db.Entry(acontecimento).Property(b => b.DataCadastro).IsModified = false;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, acontecimento.Id);
            return RedirectToAction("Index", "Acontecimentos", new { evento = idEvento });
        }
    }
}
