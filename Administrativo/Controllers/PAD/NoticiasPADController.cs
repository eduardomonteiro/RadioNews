using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    public class NoticiasPADController : BaseController
    {
        int areaADM = 45;
        protected const string pathOriginal = "~/Conteudo/NoticiasPAD/{0}/";
        //
        // GET: /NoticiasPAD/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));
        }

        private IPagedList<NoticiasPADs> GetListItens(int? page, string search, string sortField, string order)
        {
            var list = db.NoticiasPADs.Where(t => !t.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                int outDate = 0;
                int id = int.TryParse(search, out outDate) ? int.Parse(search) : 0;

                if (id > 0)
                    pagedList = pagedList.Where(e => e.Id == id);
                else
                    pagedList = pagedList.Where(e => e.Chamada.Contains(search) || e.Categoria.Contains(search) || e.Titulo.Contains(search));

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
                case "Categoria":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Categoria);
                        ViewBag.SortClassCategoria = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Categoria);
                        ViewBag.SortClassCategoria = "sorting_desc";
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

            return pagedList.ToPagedList<NoticiasPADs>(pageNumber, 10);
        }

        private void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassCategoria = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public ActionResult Details(int id = 0)
        {
            NoticiasPADs noticia = db.NoticiasPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }
        public ActionResult Edit(int id)
        {
            NoticiasPADs noticia = db.NoticiasPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }

            ApoioViewBag(noticia);

            return View(noticia);
        }
        [HttpPost]
        public ActionResult Edit(NoticiasPADs noticia, HttpPostedFileBase fotoUpload, int cropX = 0, int cropY = 0, int cropWidth = 0, int cropHeight = 0)
        {
            Validacao(noticia);
            if (ModelState.IsValid)
            {
                noticia.DataCadastro = DateTime.Now;
                noticia.Excluido = false;

                #region uploads

                if (fotoUpload != null)
                {
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticia.Id));

                    if (cropWidth > 0 && cropHeight > 0)
                        noticia.Foto = Utils.SaveAndCropColunista(fotoUpload, fileOriginal, cropX, cropY, cropWidth, cropHeight);
                    else
                        noticia.Foto = Utils.SaveFileBase(fileOriginal, fotoUpload);
                }

                db.Entry(noticia).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, noticia.Id);
                return RedirectToAction("Details", new { id = noticia.Id });
            }

            ApoioViewBag(noticia);

            return View(noticia);
        }
        public ActionResult Create()
        {
            ApoioViewBag(null);

            return View();
        }
        [HttpPost]
        public ActionResult Create(NoticiasPADs noticia, HttpPostedFileBase logo, int cropX = 0, int cropY = 0, int cropWidth = 0, int cropHeight = 0)
        {
            Validacao(noticia);
            if (ModelState.IsValid)
            {
                noticia.Excluido = false;
                noticia.DataCadastro = DateTime.Now;

                db.NoticiasPADs.Add(noticia);

                if (db.SaveChanges() > 0 && logo != null)
                {
                    #region uploads
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticia.Id));

                    if (cropWidth > 0 && cropHeight > 0)
                        noticia.Foto = Utils.SaveAndCropColunista(logo, fileOriginal, cropX, cropY, cropWidth, cropHeight);
                    else
                        noticia.Foto = Utils.SaveFileBase(fileOriginal, logo);

                    db.Entry(noticia).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, noticia.Id);
                return RedirectToAction("Details", new { id = noticia.Id });
            }

            ApoioViewBag(noticia);

            return View(noticia);
        }
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            NoticiasPADs noticia = db.NoticiasPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            NoticiasPADs noticia = db.NoticiasPADs.Find(id);

            noticia.Excluido = true;
            db.Entry(noticia).State = EntityState.Modified;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, noticia.Id);
            return RedirectToAction("Index");
        }

        private void ApoioViewBag(NoticiasPADs noticia)
        {
            //Método usado para gerar a listbox de apoiadores na view.
            var apoiadores = db.ApoioPADs.Where(a => !a.Excluido).ToList();
            if (noticia != null)
            {
                ViewBag.ApoioId = new SelectList(apoiadores, "id", "nome", noticia.ApoioId);
            }
            else
            {
                ViewBag.ApoioId = new SelectList(apoiadores, "id", "nome");
            }

        }

        private void Validacao(NoticiasPADs noticia)
        {
            if (string.IsNullOrWhiteSpace(noticia.Titulo))
                ModelState.AddModelError("Titulo", "O Titulo deve ser preenchido.");
            else if (noticia.Titulo.Length > 80)
                ModelState.AddModelError("Titulo", "O Titulo não deve ter mais do que 80 caracteres.");

            if (!string.IsNullOrWhiteSpace(noticia.Categoria)) {
                if (noticia.Categoria.Length > 15)
                    ModelState.AddModelError("Categoria", "A Categoria não deve ter mais do que 100 caracteres.");
            }

            //if (!string.IsNullOrWhiteSpace(noticia.Chamada)) {
            //    if (noticia.Chamada.Length > 350)
            //        ModelState.AddModelError("Chamada", "A Chamada não deve ter mais do que 350 caracteres.");
            //}
        }
    }
}
