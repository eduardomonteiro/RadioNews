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
    public class EspeciaisSecoesController : BaseController
    {
        public int areaADM = 34;
        //
        // GET: /EspeciaisSecoes/
        protected const string pathFoto744x500 = "~/Conteudo/noticias/{0}/744x500/";
        protected const string pathFoto405x270 = "~/Conteudo/noticias/{0}/405x270/";
        protected const string pathFoto365x240 = "~/Conteudo/noticias/{0}/365x240/";
        protected const string pathFoto260x173 = "~/Conteudo/noticias/{0}/260x173/";
        protected const string pathOriginal = "~/Conteudo/noticias/{0}/original/";

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto744x500;
        }
        //

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int EspecialId = 0)
        {

            ViewBag.Especiais_SecoesActive = "active";
            ViewBag.Especiais_SecoesSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            ViewBag.EspecialId = EspecialId;

            if (EspecialId > 0)
            {
                var especial = db.Editoriais.FirstOrDefault(x => x.id == EspecialId);
                ViewBag.TemSecao = especial.Especiais_Modelos.TemSecao;
                ViewBag.StatusEditoria = especial.ativo;
                ViewBag.EspecialNome = especial.nome;
            }

            allClassOffSort();

            return View(GetListItens(page, search, sortField, order, EspecialId));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassEditoriaId = "sorting";
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassExcluido = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Especiais_Secoes> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", int EspecialId = 0)
        {
            var list = db.Especiais_Secoes.Where(x=>!x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);


            if (EspecialId>0)
            {
                pagedList = pagedList.Where(x => x.EditoriaId == EspecialId);
            }

            if (!string.IsNullOrEmpty(searchParam))
            {
                pagedList = pagedList.Where(n => n.Titulo.ToLower().Contains(searchParam.ToLower()) || n.Editoriais.nome.Contains(searchParam));
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
                case "EditoriaId":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.EditoriaId);
                        ViewBag.SortClassEditoriaId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.EditoriaId);
                        ViewBag.SortClassEditoriaId = "sorting_desc";
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
                case "Excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Excluido);
                        ViewBag.SortClassExcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Excluido);
                        ViewBag.SortClassExcluido = "sorting_desc";
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

            return pagedList.ToPagedList<Especiais_Secoes>(pageNumber, 10);
        }

        //
        // GET: /EspeciaisSecoes/Details/5

        public ActionResult Details(int id = 0)
        {
            Especiais_Secoes especiais_secoes = db.Especiais_Secoes.Find(id);
            if (especiais_secoes == null)
            {
                return HttpNotFound();
            }
            ViewBag.EspecialNome = especiais_secoes.Editoriais.nome;
            ViewBag.EspecialId = especiais_secoes.Editoriais.id;

            return View(especiais_secoes);
        }

        //
        // GET: /EspeciaisSecoes/Create

        public ActionResult Create(int EspecialId = 0)
        {
            var especial = db.Editoriais.FirstOrDefault(x => x.id == EspecialId);           
            ViewBag.EspecialNome = especial.nome;
            ViewBag.EspecialId = EspecialId;
            ViewBag.EditoriaId = new SelectList(db.Editoriais, "id", "nome");
            return View();
        }

        //
        // POST: /EspeciaisSecoes/Create

        [HttpPost]
        public ActionResult Create(Especiais_Secoes especiais_secoes)
        {
            if (ModelState.IsValid)
            {
                especiais_secoes.DataCadastro = DateTime.Now;
                especiais_secoes.Excluido = false;

                db.Especiais_Secoes.Add(especiais_secoes);
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, especiais_secoes.Id);
                return RedirectToAction("Index", new { EspecialId = especiais_secoes.EditoriaId });
            }

            ViewBag.EditoriaId = new SelectList(db.Editoriais, "id", "nome", especiais_secoes.EditoriaId);
            return View(especiais_secoes);
        }

        //
        // GET: /EspeciaisSecoes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Especiais_Secoes especiais_secoes = db.Especiais_Secoes.Find(id);
            if (especiais_secoes == null)
            {
                return HttpNotFound();
            }

            ViewBag.EspecialNome = especiais_secoes.Editoriais.nome;
            ViewBag.EspecialId = especiais_secoes.Editoriais.id;
            ViewBag.EditoriaId = new SelectList(db.Editoriais, "id", "nome", especiais_secoes.EditoriaId);
            return View(especiais_secoes);
        }

        //
        // POST: /EspeciaisSecoes/Edit/5

        [HttpPost]
        public ActionResult Edit(Especiais_Secoes especiais_secoes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(especiais_secoes).State = EntityState.Modified;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, especiais_secoes.Id);
                return RedirectToAction("Index");
            }
            ViewBag.EditoriaId = new SelectList(db.Editoriais, "id", "nome", especiais_secoes.EditoriaId);
            return View(especiais_secoes);
        }

        //
        // GET: /EspeciaisSecoes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Especiais_Secoes especiais_secoes = db.Especiais_Secoes.Find(id);
            if (especiais_secoes == null)
            {
                return HttpNotFound();
            }

            return View(especiais_secoes);
        }

        //
        // POST: /EspeciaisSecoes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Especiais_Secoes especiais_secoes = db.Especiais_Secoes.Find(id);

            especiais_secoes.Excluido = true;
            db.Entry(especiais_secoes).State = EntityState.Modified;
            //db.Especiais_Secoes.Remove(especiais_secoes);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, especiais_secoes.Id);
            return RedirectToAction("Index", new { EditorialId = especiais_secoes.EditoriaId });
        }

       
    }
}




