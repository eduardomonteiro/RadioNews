using Administrativo.Commons;
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
    public class AudiosController : BaseController
    {
        //
        // GET: /Audio

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.CategoriasActive = "active";
            ViewBag.CategoriasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            AllClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }

        public void AllClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassDescricao = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<Audios> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {

            var list = db.Audios.Where(a => !a.Excluido.Value);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(c => c.Id == id || c.Legenda.ToLower().Contains(searchParam.ToLower()) || !string.IsNullOrEmpty(c.ColecoesAudios.Titulo) ? c.ColecoesAudios.Titulo.ToLower().Contains(searchParam.ToLower()) : false);

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
                case "Legenda":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(c => c.Legenda);
                        ViewBag.SortClassLegenda = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(c => c.Legenda);
                        ViewBag.SortClassLegenda = "sorting_desc";
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
        public ActionResult Create(Audios audio, HttpPostedFileBase audioFile)
        {
            audio.DataCadastro = DateTime.Now;
            audio.Excluido = false;
            db.Entry(audio).State = EntityState.Added;
            
            db.SaveChanges();

                        
            if (audioFile != null)
            {
                var path = Server.MapPath("~/conteudo/audios/" + audio.Id + "/");
                audio.Url = Utils.SaveFileBase(path, audioFile);
                db.Entry(audio).State = EntityState.Modified;

                db.SaveChanges();
            }


            return RedirectToAction("Index");

        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var audio = db.Audios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido.Value);

            if (audio == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(audio);
        }

        [HttpPost]
        public ActionResult Edit(Audios audio, HttpPostedFileBase audioFile)
        {
            if (audioFile != null)
            {
                var path = Server.MapPath("~/conteudo/audios/" + audio.Id + "/");
                audio.Url = Utils.SaveFileBase(path, audioFile);
            }

            audio.Excluido = false;
            db.Entry(audio).State = EntityState.Modified;
            db.Entry(audio).Property(a => a.DataCadastro).IsModified = false;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var audio = db.Audios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido.Value);

            if (audio == null)
            {
                return RedirectToAction("Index");
            }

            return View(audio);
        }

        public ActionResult Form(Audios model)
        {
            ViewBag.Colecoes = db.ColecoesAudios.Where(col => !col.Excluido && col.Liberado).OrderBy(col => col.Titulo).Select(colecao => new SelectListItem { Text = colecao.Titulo, Value = colecao.Id.ToString() }).ToList();
            return PartialView("_Form", model);
        }


        public ActionResult Delete(int id)
        {
            var audio = db.Audios.FirstOrDefault(colecao => colecao.Id == id && !colecao.Excluido.Value);

            if (audio == null)
            {
                return RedirectToAction("Index");
            }           

            return View(audio);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            var audio = db.Audios.Find(id);

            audio.Excluido = true;
            db.Entry(audio).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }


    }
}
