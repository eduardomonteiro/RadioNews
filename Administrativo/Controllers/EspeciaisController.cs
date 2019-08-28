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
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Especiais")]
    public class EspeciaisController : BaseController
    {
        //private RadioCompanyNameContext db = new RadioCompanyNameContext();

        //
        // GET: /Especiais/
        public int areaADM = 11;
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.EspeciaisActive = "active";
            ViewBag.EspeciaisSubActive = "active";
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
            ViewBag.SortClassChave = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Especiais> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.Especiais;

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(e => e.Id == id);

            }


            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;
                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Titulo);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Titulo);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;
                case "Chave":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Chave);
                        ViewBag.SortClassChave = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Chave);
                        ViewBag.SortClassChave = "sorting_desc";
                    }
                    break;
                case "Ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Ativo);
                        ViewBag.SortClassAtivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Ativo);
                        ViewBag.SortClassAtivo = "sorting_desc";
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
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Especiais>(pageNumber, 10);
        }

        //
        // GET: /Especiais/Details/5

        public ActionResult Details(int id = 0)
        {
            Especiais especiais = db.Especiais.Find(id);
            if (especiais == null)
            {
                return HttpNotFound();
            }
            return View(especiais);
        }

        //
        // GET: /Especiais/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Especiais/Create

        [HttpPost]
        public ActionResult Create(Especiais especiais)
        {
            especiais.Chave = especiais.Titulo.GenerateSlug();

            if (ModelState.IsValid)
            {
                especiais.DataCadastro = DateTime.Now;
                especiais.Excluido = false;

                db.Especiais.Add(especiais);
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, especiais.Id);
                return RedirectToAction("Index");
            }

            return View(especiais);
        }

        //
        // GET: /Especiais/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Especiais especiais = db.Especiais.Find(id);
            if (especiais == null)
            {
                return HttpNotFound();
            }
            return View(especiais);
        }

        //
        // POST: /Especiais/Edit/5

        [HttpPost]
        public ActionResult Edit(Especiais especiais)
        {
            if (ModelState.IsValid)
            {
                especiais.DataCadastro = DateTime.Now;
                especiais.Excluido = false;
                especiais.Chave.GenerateSlug();

                db.Entry(especiais).State = EntityState.Modified;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, especiais.Id);
                return RedirectToAction("Index");
            }
            return View(especiais);
        }

        //
        // GET: /Especiais/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Especiais especiais = db.Especiais.Find(id);
            if (especiais == null)
            {
                return HttpNotFound();
            }
            return View(especiais);
        }

        //
        // POST: /Especiais/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Especiais especiais = db.Especiais.Find(id);

            especiais.Excluido = true;
            db.Entry(especiais).State = EntityState.Modified;
            

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, especiais.Id);
            return RedirectToAction("Index");
        }

        /*
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        */
    }
}




