using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;
using System.IO;

namespace Administrativo.Controllers
{
    public class TimesPADController : BaseController
    {
        int areaADM = 43;
        protected const string pathOriginal = "~/Conteudo/timesPAD/{0}/";
        protected const string pathLogo100x100 = "~/Conteudo/timesPAD/{0}/100x100/";
        //
        // GET: /TimesPAD/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));
        }

        private IPagedList<TimesPADs> GetListItens(int? page, string search, string sortField, string order)
        {
            var list = db.TimesPADs.Where(t => !t.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                int outDate = 0;
                int id = int.TryParse(search, out outDate) ? int.Parse(search) : 0;

                if (id > 0)
                    pagedList = pagedList.Where(e => e.Id == id);
                else
                    pagedList = pagedList.Where(e => e.Nome.Contains(search));

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
                case "Nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Nome);
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
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<TimesPADs>(pageNumber, 10);
        }

        private void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public ActionResult Details(int id = 0)
        {
            TimesPADs time = db.TimesPADs.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }
        public ActionResult Edit(int id)
        {
            TimesPADs time = db.TimesPADs.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }

            return View(time);
        }
        [HttpPost]
        public ActionResult Edit(TimesPADs time, HttpPostedFileBase fotoUpload)
        {
            Validacao(time);
            if (ModelState.IsValid)
            {
                time.DataCadastro = DateTime.Now;
                time.Excluido = false;

                #region uploads

                if (fotoUpload != null)
                {
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, time.Id));
                    var path100x100 = Server.MapPath(string.Format(pathLogo100x100, time.Id));

                    time.Logo = Utils.SaveFileBase(fileOriginal, fotoUpload);
                    Utils.resizeImageAndSave3(Path.Combine(fileOriginal, time.Logo), 100, 100, path100x100);
                }

                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, time.Id);
                return RedirectToAction("Details", new { id = time.Id });
            }
            return View(time);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(TimesPADs time, HttpPostedFileBase logo)
        {
            Validacao(time);
            if (logo == null)
            {
                ModelState.AddModelError("Logo","O time deve ter um logo.");
            }
            if (ModelState.IsValid)
            {
                time.Excluido = false;
                time.DataCadastro = DateTime.Now;

                db.TimesPADs.Add(time);

                if (db.SaveChanges() > 0)
                {
                    #region uploads
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, time.Id));
                    var path100x100 = Server.MapPath(string.Format(pathLogo100x100, time.Id));

                    time.Logo = Utils.SaveFileBase(fileOriginal, logo);
                    Utils.resizeImageAndSave3(Path.Combine(fileOriginal, time.Logo), 100, 100, path100x100);

                    db.Entry(time).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, time.Id);
                return RedirectToAction("Index");
            }
            return View(time);
        }
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            TimesPADs time = db.TimesPADs.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            TimesPADs time = db.TimesPADs.Find(id);

            time.Excluido = true;
            db.Entry(time).State = EntityState.Modified;


            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, time.Id);
            return RedirectToAction("Index");
        }

        private void Validacao(TimesPADs time)
        {
            if (string.IsNullOrWhiteSpace(time.Nome))
                ModelState.AddModelError("Nome", "O Nome deve ser preenchido.");
            else if (time.Nome.Length > 150)
                ModelState.AddModelError("Nome", "O Nome não deve ter mais do que 150 caracteres.");
        }
    }
}
