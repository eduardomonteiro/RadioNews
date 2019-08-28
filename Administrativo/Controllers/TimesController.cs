using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Times")]
    public class TimesController : BaseController
    {
        protected const string pathFoto48x48 = "~/Conteudo/times/{0}/48x48/";
        protected const string pathFoto28x28 = "~/Conteudo/times/{0}/28x28/";
        protected const string pathOriginal = "~/Conteudo/times/{0}/original/";

        public int areaADM = 22;
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto48x48;
        }
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.BlocosActive = "active";
            ViewBag.BlocosSubActive = "active";
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
            ViewBag.SortClassNome = "sorting";
            ViewBag.SortClassImagem = "sorting";
            ViewBag.SortClassLocal = "sorting";
            ViewBag.SortClassData = "sorting";
            ViewBag.SortClassDescricao = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassExcluido = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Times> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {

            var list = db.Times.Where(x => !x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(b => b.Id == id);

            }


            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;
                case "Nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Nome);
                        ViewBag.SortClassNome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Nome);
                        ViewBag.SortClassNome = "sorting_desc";
                    }
                    break;
                case "Imagem":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Escudo);
                        ViewBag.SortClassImagem = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Escudo);
                        ViewBag.SortClassImagem = "sorting_desc";
                    }
                    break;
             
                case "Data":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.DataCadastro);
                        ViewBag.SortClassData = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.DataCadastro);
                        ViewBag.SortClassData = "sorting_desc";
                    }
                    break;
              
                case "Ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Ativo);
                        ViewBag.SortClassAtivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Ativo);
                        ViewBag.SortClassAtivo = "sorting_desc";
                    }
                    break;
                case "Excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Excluido);
                        ViewBag.SortClassExcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Excluido);
                        ViewBag.SortClassExcluido = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(b => b.Id);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Times>(pageNumber, 10);
        }



        //
        // GET: /Blocos/Details/5

        public ActionResult Details(int id = 0)
        {
            Times time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        //
        // GET: /Blocos/Create

        //public ActionResult Create()
        //{
        //    ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido && a.esportes), "id", "nome");
        //    return View();
        //}

        ////
        //// POST: /Times/Create

        //[HttpPost]
        //public ActionResult Create(Times times, HttpPostedFileBase Escudo)
        //{
            
        //    if (ModelState.IsValid)
        //    {
        //        times.Chave = times.Nome.GenerateSlug();
        //        times.Excluido = false;
        //        times.DataCadastro = DateTime.Now;

        //        db.Times.Add(times);

        //        if (db.SaveChanges() > 0)
        //        {
        //            #region image e slug
        //            if (Escudo != null)
        //            {
                        
        //                var path48x48 = Server.MapPath(string.Format(pathFoto48x48, times.Id));
        //                var path28x28 = Server.MapPath(string.Format(pathFoto28x28, times.Id));

        //                var fileOriginal = Server.MapPath(string.Format(pathOriginal, times.Id));

        //                times.Escudo = Utils.SaveFileBase(fileOriginal, Escudo);

                        
        //                Utils.resizeImageAndSave3(Path.Combine(fileOriginal, times.Escudo), 48, 48, path48x48);
        //                Utils.resizeImageAndSave3(Path.Combine(fileOriginal, times.Escudo), 28, 28, path28x28);

        //            }

        //            int suffix = 0;

        //            do
        //            {
        //                times.Chave = times.Nome.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
        //                suffix++;
        //            } while (db.Times.Where(o => o.Chave == times.Chave).Count() > 0);
        //            #endregion

        //            db.Entry(times).State = EntityState.Modified;
        //            db.SaveChanges();

        //        }

        //        GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, times.Id);
        //        return RedirectToAction("Index");
        //    }
        //    ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido && a.esportes), "id", "nome");
        //    return View(times);
        //}

        //
        // GET: /Blocos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Times time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido && a.esportes), "id", "nome",time.EditoriaId);
            return View(time);
        }

        //
        // POST: /Blocos/Edit/5

        [HttpPost]
        public ActionResult Edit(Times time, HttpPostedFileBase EscudoUpload)
        {
           
            if (ModelState.IsValid)
            {
                time.Chave = time.Nome.GenerateSlug();
                time.DataCadastro = DateTime.Now;

                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();

                if (db.SaveChanges() > 0)
                {
                    #region image
                    if (EscudoUpload != null)
                    {
                        var path48x48 = Server.MapPath(string.Format(pathFoto48x48, time.Id));
                        var path28x28 = Server.MapPath(string.Format(pathFoto28x28, time.Id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, time.Id));

                        time.Escudo = Utils.SaveFileBase(fileOriginal, EscudoUpload);
                        
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, time.Escudo), 48, 48, path48x48);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, time.Escudo), 28, 28, path28x28);
                        

                    }
                    #endregion

                    #region slug
                    int suffix = 0;

                    do
                    {
                        time.Chave = time.Nome.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Times.Where(o => o.Chave == time.Chave).Count() > 0);
                    #endregion

                    db.Entry(time).State = EntityState.Modified;
                    db.SaveChanges();

                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, time.Id);
                return RedirectToAction("Index");
            }

            ViewBag.EditoriaId = new SelectList(db.Editoriais.Where(a => !a.excluido && a.esportes), "id", "nome", time.EditoriaId);
            return View(time);
        }

        //
        // GET: /Blocos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Times time = db.Times.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        //
        // POST: /Blocos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Times time = db.Times.Find(id);
            time.Excluido = true;
            db.Entry(time).State = EntityState.Modified;
            db.Entry(time).Property(x => x.DataCadastro).IsModified = false;
            db.Entry(time).Property(x => x.Ativo).IsModified = false;
            db.Entry(time).Property(x => x.Chave).IsModified = false;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, time.Id);
            return RedirectToAction("Index");
        }



    }
}
