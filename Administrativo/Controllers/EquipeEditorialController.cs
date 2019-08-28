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

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Equipes")]
    public class EquipeEditorialController : BaseController
    {
        private const string caminhoFoto = "~/conteudo/equipe_editorial/235x120/";
        private const string caminhoFotoVertical = "~/conteudo/equipe_editorial/140x270/";

        //
        // GET: /EquipeEditorial/

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.CaminhoFoto = caminhoFoto;
            ViewBag.CaminhoFotoVertical = caminhoFotoVertical;
        }

        public ActionResult Index(int editorial,int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.Editoriais_EquipeActive = "active";
            ViewBag.Editoriais_EquipeSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            ViewBag.editorial = db.Editoriais.FirstOrDefault(a => a.id == editorial);


            return View(GetListItens(editorial, page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassfuncao = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Editoriais_Equipe> GetListItens(int editorial, int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Editoriais_Equipe.Where(a => !a.excluido && a.EditoriaisId == editorial);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = 0;
                if (int.TryParse(searchParam, out outDate))
                {
                    int.Parse(searchParam);
                    pagedList = pagedList.Where(g => g.id == id);
                }
                else
                {
                    pagedList = pagedList.Where(g => g.nome.ToLower().Contains(searchParam.ToLower()) || g.texto.ToLower().Contains(searchParam.ToLower()) || g.funcao.ToLower().Contains(searchParam.ToLower()));
                };

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
                case "funcao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.funcao);
                        ViewBag.SortClassfuncao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.funcao);
                        ViewBag.SortClassfuncao = "sorting_desc";
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

            return pagedList.ToPagedList<Editoriais_Equipe>(pageNumber, 10);
        }

        //
        // GET: /EquipeEditorial/Details/5

        public ActionResult Details(int id = 0)
        {
            Editoriais_Equipe editoriais_equipe = db.Editoriais_Equipe.Find(id);
            if (editoriais_equipe == null)
            {
                return HttpNotFound();
            }
            return View(editoriais_equipe);
        }

        //
        // GET: /EquipeEditorial/Create

        public ActionResult Create(int editorial)
        {
            var edit = db.Editoriais.FirstOrDefault(a => a.id == editorial);
            ViewBag.editorial = edit;
            
            ViewBag.EditoriaisId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", edit.id);
            return View();
        }

        //
        // POST: /EquipeEditorial/Create

        [HttpPost]
        public ActionResult Create(int editorial, Editoriais_Equipe editoriais_equipe, HttpPostedFileBase imagem, HttpPostedFileBase imagemVertical)
        {
            var edit = db.Editoriais.FirstOrDefault(a => a.id == editorial);
            ViewBag.editorial = edit;
            if (ModelState.IsValid)
            {
                db.Editoriais_Equipe.Add(editoriais_equipe);
                editoriais_equipe.DataCadastro = DateTime.Now;
                editoriais_equipe.Editoriais = db.Editoriais.Find(editoriais_equipe.EditoriaisId);

                if (imagem != null)
                {

                    var PathFoto = Server.MapPath(caminhoFoto);

                    editoriais_equipe.imagem = Utils.SaveAndCropImage(imagem, PathFoto, 0, 0, 235, 120);

                };

                if (imagemVertical != null)
                {

                    var PathFotoVertical = Server.MapPath(caminhoFotoVertical);

                    editoriais_equipe.imagemVertical = Utils.SaveAndCropImage(imagemVertical, PathFotoVertical, 0, 0, 140, 270);

                };

                int suffix = 0;

                do
                {
                    editoriais_equipe.chave = editoriais_equipe.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == editoriais_equipe.chave).Count() > 0);

                db.SaveChanges();

                ViewBag.editorial = db.Editoriais.FirstOrDefault(a => a.id == editorial);
                return RedirectToAction("Index", new { editorial = ViewBag.editorial.id });
            }

            ViewBag.EditoriaisId = new SelectList(db.Editoriais.Where(a=>!a.excluido), "id", "nome", editoriais_equipe.EditoriaisId);
            
            return View(editoriais_equipe);
        }

        //
        // GET: /EquipeEditorial/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Editoriais_Equipe editoriais_equipe = db.Editoriais_Equipe.Find(id);
            if (editoriais_equipe == null)
            {
                return HttpNotFound();
            }
            ViewBag.EditoriaisId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", editoriais_equipe.EditoriaisId);
            return View(editoriais_equipe);
        }

        //
        // POST: /EquipeEditorial/Edit/5

        [HttpPost]
        public ActionResult Edit(Editoriais_Equipe editoriais_equipe, HttpPostedFileBase imagemUpload, HttpPostedFileBase imagemVerticalUpload)
        {
            if (ModelState.IsValid)
            {

                if (imagemUpload != null)
                {
                    var PathFoto = Server.MapPath(caminhoFoto);
                    editoriais_equipe.imagem = Utils.SaveAndCropImage(imagemUpload, PathFoto, 0, 0, 235, 120);
                };

                if (imagemVerticalUpload != null)
                {

                    var PathFoto = Server.MapPath(caminhoFotoVertical);
                    editoriais_equipe.imagemVertical = Utils.SaveAndCropImage(imagemVerticalUpload, PathFoto, 0, 0, 140, 270);
                };

                int suffix = 0;

                do
                {
                    editoriais_equipe.chave = editoriais_equipe.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == editoriais_equipe.chave).Count() > 0);
                editoriais_equipe.Editoriais = db.Editoriais.Find(editoriais_equipe.EditoriaisId);
                db.Entry(editoriais_equipe).State = EntityState.Modified;
                db.Entry(editoriais_equipe).Property("DataCadastro").IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index", new { editorial = editoriais_equipe.EditoriaisId });
            }
            ViewBag.EditoriaisId = new SelectList(db.Editoriais.Where(a => !a.excluido), "id", "nome", editoriais_equipe.EditoriaisId);
            return View(editoriais_equipe);
        }

        //
        // GET: /EquipeEditorial/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Editoriais_Equipe editoriais_equipe = db.Editoriais_Equipe.Find(id);
            if (editoriais_equipe == null)
            {
                return HttpNotFound();
            }
            return View(editoriais_equipe);
        }

        //
        // POST: /EquipeEditorial/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Editoriais_Equipe editoriais_equipe = db.Editoriais_Equipe.Find(id);
            var edId = editoriais_equipe.EditoriaisId;
            db.Editoriais_Equipe.Remove(editoriais_equipe);
            
            db.SaveChanges();
            return RedirectToAction("Index", new { editorial = edId });
        }

    }
}




