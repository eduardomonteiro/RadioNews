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

namespace Administrativo.Controllers
{
    public class ApoioPADController : BaseController
    {
        int areaADM = 44;
        protected const string pathOriginal = "~/Conteudo/ApoioPAD/{0}/";
        //
        // GET: /ApoioPAD/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));
        }

        private IPagedList<ApoioPADs> GetListItens(int? page, string search, string sortField, string order)
        {
            var list = db.ApoioPADs.Where(t => !t.Excluido);

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

            return pagedList.ToPagedList<ApoioPADs>(pageNumber, 10);
        }

        private void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public ActionResult Details(int id = 0)
        {
            ApoioPADs apoio = db.ApoioPADs.Find(id);
            if (apoio == null)
            {
                return HttpNotFound();
            }
            return View(apoio);
        }
        public ActionResult Edit(int id)
        {
            ApoioPADs apoio = db.ApoioPADs.Find(id);
            if (apoio == null)
            {
                return HttpNotFound();
            }

            return View(apoio);
        }
        [HttpPost]
        public ActionResult Edit(ApoioPADs apoio, HttpPostedFileBase fotoUpload)
        {
            Validacao(apoio);
            if (ModelState.IsValid)
            {
                apoio.DataCadastro = DateTime.Now;
                apoio.Excluido = false;

                #region uploads

                if (fotoUpload != null)
                {
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, apoio.Id));

                    apoio.Logo = Utils.SaveFileBase(fileOriginal, fotoUpload);
                }

                db.Entry(apoio).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, apoio.Id);
                return RedirectToAction("Details", new { id = apoio.Id });
            }
            return View(apoio);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(ApoioPADs apoio, HttpPostedFileBase logo)
        {
            Validacao(apoio);
            if (logo == null)
            {
                ModelState.AddModelError("Logo", "O Apoiador deve ter um logo.");
            }
            if (ModelState.IsValid)
            {
                apoio.Excluido = false;
                apoio.DataCadastro = DateTime.Now;

                db.ApoioPADs.Add(apoio);

                if (db.SaveChanges() > 0)
                {
                    #region uploads
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, apoio.Id));
                    apoio.Logo = Utils.SaveFileBase(fileOriginal, logo);

                    db.Entry(apoio).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, apoio.Id);
                return RedirectToAction("Index");
            }
            return View(apoio);
        }
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            ApoioPADs apoio = db.ApoioPADs.Find(id);
            if (apoio == null)
            {
                return HttpNotFound();
            }
            return View(apoio);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            ApoioPADs apoio = db.ApoioPADs.Find(id);

            apoio.Excluido = true;
            db.Entry(apoio).State = EntityState.Modified;


            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, apoio.Id);
            return RedirectToAction("Index");
        }

        private void Validacao(ApoioPADs apoio)
        {
            if (string.IsNullOrWhiteSpace(apoio.Nome))
                ModelState.AddModelError("Nome", "O Nome deve ser preenchido.");
            else if (apoio.Nome.Length > 350)
                ModelState.AddModelError("Nome", "O Nome não deve ter mais do que 350 caracteres.");
        }
    }
}
