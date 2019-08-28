using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using Omu.Drawing;
using System.IO;
using System.Drawing;
using Administrativo.Commons;
using Administrativo.Enums;
using WebMatrix.WebData;
using Administrativo.Helpers;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Apresentadores")]
    public class ApresentadoresController : BaseController
    {
         public int areadADM = 2;

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.ApresentadoresActive = "active";
            ViewBag.ApresentadoresSubActive = "active";
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

        }

        public IPagedList<Apresentadores> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Apresentadores;

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            //searchParam = Utils.RemoveAccent(searchParam);


            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(a => a.id == id || a.nome.ToLower().Contains(searchParam.ToLower()));

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(a => a.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(a => a.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(a => a.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(a => a.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(a => a.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Apresentadores>(pageNumber, 10);
        }

       
        //
        // GET: /Apresentadores/Details/5

        public ActionResult Details(int id = 0)
        {
            Apresentadores apresentadores = db.Apresentadores.Find(id);
            if (apresentadores == null)
            {
                return HttpNotFound();
            }
            return View(apresentadores);
        }

        //
        // GET: /Apresentadores/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Apresentadores/Create

        [HttpPost]
        public ActionResult Create(Apresentadores apresentadores, HttpPostedFileBase fotoExterna)
        {
            if (ModelState.IsValid)
            {

                if (fotoExterna != null)
                {

                    var pathinterna = Server.MapPath(caminhoInterna);
                    var pathLista = Server.MapPath(caminhoListagem);

                  //  apresentadores.fotoInterna = Utils.SaveAndCropImage(fotoInterna, pathinterna, 0, 0, 621, 201);

                    apresentadores.fotoExterna = Utils.SaveAndCropImage(fotoExterna, pathLista, 0, 0, 110, 110);

                };

                int suffix = 0;

                do
                {
                    apresentadores.chave = apresentadores.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Apresentadores.Where(o => o.chave == apresentadores.chave).Count() > 0);

                apresentadores.DataCadastro = DateTime.Now;
                db.Apresentadores.Add(apresentadores);
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areadADM, TipoAcesso.Insercao, apresentadores.id);

                return RedirectToAction("Index");
            }

            return View(apresentadores);
        }

        //
        // GET: /Apresentadores/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Apresentadores apresentadores = db.Apresentadores.Find(id);
            if (apresentadores == null)
            {
                return HttpNotFound();
            }
            return View(apresentadores);
        }

        //
        // POST: /Apresentadores/Edit/5

        [HttpPost]
        public ActionResult Edit(Apresentadores apresentadores, HttpPostedFileBase fotoExternaUpload)
        {
            if (ModelState.IsValid)
            {
                
                if (fotoExternaUpload != null)
                {
     
                    var pathLista = Server.MapPath(caminhoListagem);
                    apresentadores.fotoExterna = Utils.SaveAndCropImage(fotoExternaUpload, pathLista, 0, 0,110, 110);

                };



                int suffix = 0;

                do
                {
                    apresentadores.chave = apresentadores.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Apresentadores.Where(o => o.chave == apresentadores.chave && o.id != apresentadores.id).Count() > 0);

                
                db.Entry(apresentadores).State = EntityState.Modified;
                db.Entry(apresentadores).Property("DataCadastro").IsModified = false;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areadADM, TipoAcesso.Edicao, apresentadores.id);
                return RedirectToAction("Index");
            }

            return View(apresentadores);
        }

        //
        // GET: /Apresentadores/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Apresentadores apresentadores = db.Apresentadores.Find(id);
            if (apresentadores == null)
            {
                return HttpNotFound();
            }
            return View(apresentadores);
        }

        //
        // POST: /Apresentadores/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Apresentadores apresentadores = db.Apresentadores.Find(id);
            db.Apresentadores.Remove(apresentadores);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areadADM, TipoAcesso.Exclusao, apresentadores.id);
            return RedirectToAction("Index");
        }
    }
}




