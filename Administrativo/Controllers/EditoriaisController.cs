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
using Administrativo.ViewModel;
using System.IO;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Editorias")]
    public class EditoriaisController : BaseController
    {
        public int areaADM = 8;
        protected const string pathOriginal = "~/Conteudo/editoriais/{0}/";

        //
        //
        // GET: /Editoriais/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", string especial = "")
        {

            ViewBag.EditoriaisActive = "active";
            ViewBag.EditoriaisSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order, especial));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Editoriais> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", string especial = "")
        {
            var list = db.Editoriais.Where(x => !x.excluido && !x.esportes);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(especial))
            {
                bool opcao = Convert.ToBoolean(Convert.ToInt32(especial));               
                pagedList = pagedList.Where(x => x.especial == opcao);
            }

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(e => e.id == id || e.nome.ToLower().Contains(searchParam.ToLower()));

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.nome);
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
                    pagedList = pagedList.OrderByDescending(e => e.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Editoriais>(pageNumber, 10);
        }

        //
        // GET: /Editoriais/Details/5

        public ActionResult Details(int id = 0)
        {
            Editoriais editoriais = db.Editoriais.Include("Categorias").FirstOrDefault(a => a.id == id);
            if (editoriais == null)
            {
                return HttpNotFound();
            }
            return View(editoriais);
        }

        //
        // GET: /Editoriais/Create

        public ActionResult Create()
        {
            ViewBag.CategoriasId = new MultiSelectList(db.Categorias.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.ModelosEspeciaisId = new SelectList(db.Especiais_Modelos.Where(e => e.especial), "Id", "Tipo");

            return View();
        }

        //
        // POST: /Editoriais/Create

        [HttpPost]
        public ActionResult Create(Editoriais editoriais, HttpPostedFileBase foto, int[] CategoriasId)
        {
            if (ModelState.IsValid)
            {
                editoriais.DataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {
                    editoriais.chave = editoriais.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Editoriais.Where(o => o.chave == editoriais.chave).Count() > 0);

                db.Editoriais.Add(editoriais);

                if (db.SaveChanges() > 0)
                {
                    #region uploads

                    if (foto != null)
                    {
                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, editoriais.id));
                        var path365x240 = Server.MapPath(string.Format(pathOriginal + "365x240/", editoriais.id));

                        editoriais.FotoCapa = Utils.SaveFileBase(fileOriginal, foto);

                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, editoriais.FotoCapa), 365, 240, path365x240);
                    }


                    db.Entry(editoriais).State = EntityState.Modified;
                    #endregion
                }

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, editoriais.id);
                return RedirectToAction("Index");
            }
            ViewBag.ModelosEspeciaisId = new MultiSelectList(db.Especiais_Modelos, "Id", "Tipo");
            ViewBag.CategoriasId = new SelectList(db.Categorias.Where(a => !a.Excluido), "Id", "Titulo");
            return View(editoriais);
        }

        //
        // GET: /Editoriais/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Editoriais editoriais = db.Editoriais.Find(id);
            if (editoriais == null)
            {
                return HttpNotFound();
            }

            ViewBag.modeloEspecial = new SelectList(db.Especiais_Modelos.Where(e => e.especial), "Id", "Tipo", (editoriais.modeloEspecial.HasValue?editoriais.modeloEspecial.Value:0));
            ViewBag.pathOriginal = pathOriginal;

            return View(editoriais);
        }

        //
        // POST: /Editoriais/Edit/5

        [HttpPost]
        public ActionResult Edit(Editoriais editoriais, HttpPostedFileBase fotoUpload, string fotoExistente = "")
        {
            if (ModelState.IsValid)
            {

                int suffix = 0;

                do
                {
                    editoriais.chave = editoriais.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Editoriais.Where(o => o.chave == editoriais.chave && o.id != editoriais.id).Count() > 0);

                if (!string.IsNullOrEmpty(fotoExistente))
                {
                    editoriais.FotoCapa = fotoExistente;
                    ModelState.Remove("fotoUpload");
                }

                #region uploads

                if (fotoUpload != null)
                {
                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, editoriais.id));
                    var path365x240 = Server.MapPath(string.Format(pathOriginal + "365x240/", editoriais.id));

                    editoriais.FotoCapa = Utils.SaveFileBase(fileOriginal, fotoUpload);

                    Utils.resizeImageAndSave3(Path.Combine(fileOriginal, editoriais.FotoCapa), 365, 240, path365x240);
                }

                db.Entry(editoriais).State = EntityState.Modified;
                db.Entry(editoriais).Property("DataCadastro").IsModified = false;
                #endregion

                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, editoriais.id);

                return RedirectToAction("Index");
            }
            return View(editoriais);
        }

        //
        // GET: /Editoriais/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Editoriais editoriais = db.Editoriais.Find(id);
            if (editoriais == null)
            {
                return HttpNotFound();
            }
            return View(editoriais);
        }

        //
        // POST: /Editoriais/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Editoriais editoriais = db.Editoriais.Find(id);

            editoriais.excluido = true;
            db.Entry(editoriais).State = EntityState.Modified;

            //db.Editoriais.Remove(editoriais);

            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, editoriais.id);

            return RedirectToAction("Index");
        }
    }
}




