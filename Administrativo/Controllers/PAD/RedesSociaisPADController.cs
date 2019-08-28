using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using WebMatrix.WebData;

namespace Administrativo.Controllers.PAD
{
    public class RedesSociaisPADController : BaseController
    {
        int areaADM = 47;
        protected const string pathOriginal = "~/Conteudo/RedesSociaisPAD/{0}/";
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

        private IPagedList<RedesSociaisPADs> GetListItens(int? page, string search, string sortField, string order)
        {
            var list = db.RedesSociaisPADs.Where(t => !t.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                int outDate = 0;
                int id = int.TryParse(search, out outDate) ? int.Parse(search) : 0;

                if (id > 0)
                    pagedList = pagedList.Where(e => e.Id == id);
                else
                    pagedList = pagedList.Where(e => e.Chamada.Contains(search) || e.Hashtag.Contains(search));

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
                case "Hashtag":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Hashtag);
                        ViewBag.SortClassHashtag = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Hashtag);
                        ViewBag.SortClassHashtag = "sorting_desc";
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

            return pagedList.ToPagedList<RedesSociaisPADs>(pageNumber, 10);
        }

        private void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassHashtag = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public ActionResult Details(int id = 0)
        {
            RedesSociaisPADs noticia = db.RedesSociaisPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        public ActionResult Edit(int id)
        {
            RedesSociaisPADs noticia = db.RedesSociaisPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }

            ApoioViewBag(noticia);

            return View(noticia);
        }

        [HttpPost]
        public ActionResult Edit(RedesSociaisPADs noticia, HttpPostedFileBase fotoUpload, int cropX = 0, int cropY = 0, int cropWidth = 0, int cropHeight = 0)
        {
            Validacao(noticia);
            if (ModelState.IsValid)
            {
                noticia.DataCadastro = DateTime.Now;
                noticia.Excluido = false;

                if (fotoUpload != null)
                {
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticia.Id));

                    if (cropWidth > 0 && cropHeight > 0)
                        noticia.Foto = "/Admin/Conteudo/RedesSociaisPAD/" + noticia.Id + "/" + Utils.SaveAndCropColunista(fotoUpload, fileOriginal, cropX, cropY, cropWidth, cropHeight);
                    else
                        noticia.Foto = "/Admin/Conteudo/RedesSociaisPAD/" + noticia.Id + "/" + Utils.SaveFileBase(fileOriginal, fotoUpload);
                }

                db.Entry(noticia).State = EntityState.Modified;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, noticia.Id);
                return RedirectToAction("Details", new { id = noticia.Id });
            }

            ApoioViewBag(noticia);

            return View(noticia);
        }

        public ActionResult SelectPost(int? page, string search = "", string order = "")
        {
            ViewBag.Search = search;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListPosts(page,search,order));
        }

        private IPagedList<RedesSociais> GetListPosts(int? page, string search, string order)
        {
            var pagedList = db.RedesSociais.AsQueryable();

            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                pagedList = pagedList.Where(e => e.Texto.Contains(search));

            }

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

            return pagedList.ToList().ToPagedList<RedesSociais>(pageNumber, 10);
        }

        public ActionResult Create(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            RedesSociais post = db.RedesSociais.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            var noticia = new RedesSociaisPADs()
            {
                Chamada = post.Texto,
                Foto = post.Imagem,
                TipoRede = post.TipoRede
            };

            ApoioViewBag(null);

            return View(noticia);
        }

        [HttpPost]
        public ActionResult Create(RedesSociaisPADs noticia)
        {
            Validacao(noticia);
            if (ModelState.IsValid)
            {
                noticia.Excluido = false;
                noticia.DataCadastro = DateTime.Now;

                db.RedesSociaisPADs.Add(noticia);
                db.SaveChanges();

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
            RedesSociaisPADs noticia = db.RedesSociaisPADs.Find(id);
            if (noticia == null)
            {
                return HttpNotFound();
            }
            return View(noticia);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RedesSociaisPADs noticia = db.RedesSociaisPADs.Find(id);

            noticia.Excluido = true;
            db.Entry(noticia).State = EntityState.Modified;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, noticia.Id);
            return RedirectToAction("Index");
        }

        private void ApoioViewBag(RedesSociaisPADs noticia)
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

        private void Validacao(RedesSociaisPADs rede)
        {
            if (string.IsNullOrWhiteSpace(rede.Chamada))
                ModelState.AddModelError("Chamada", "A Chamada deve ser preenchida.");
            else if (rede.Chamada.Length > 50)
                ModelState.AddModelError("Chamada", "A Chamada não deve ter mais do que 50 caracteres.");

            if (string.IsNullOrWhiteSpace(rede.Hashtag))
                ModelState.AddModelError("Hashtag", "A Hashtag deve ser preenchida.");
            else if (rede.Hashtag.Length > 30)
                ModelState.AddModelError("Hashtag", "A Hashtag não deve ter mais do que 30 caracteres.");
        }
    }
}
