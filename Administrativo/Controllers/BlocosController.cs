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
using System.IO;
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Especiais")]
    public class BlocosController : BaseController
    {
        public int areaADM = 38;
        //
        // GET: /Blocos/
        //   protected const string pathFoto1400x629 = "~/Conteudo/blocos/{0}/1400x629/";
        protected const string pathFoto744x500 = "~/Conteudo/blocos/{0}/744x500/";
        protected const string pathFoto405x270 = "~/Conteudo/blocos/{0}/405x270/";
        protected const string pathFoto365x240 = "~/Conteudo/blocos/{0}/365x240/";
        protected const string pathFoto260x173 = "~/Conteudo/blocos/{0}/260x173/";
        protected const string pathOriginal = "~/Conteudo/blocos/{0}/original/";

        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto744x500;
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

        public IPagedList<Blocos> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Blocos.Where(x => !x.Excluido);

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
                        pagedList = pagedList.OrderBy(b => b.Imagem);
                        ViewBag.SortClassImagem = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Imagem);
                        ViewBag.SortClassImagem = "sorting_desc";
                    }
                    break;
                case "Local":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Local);
                        ViewBag.SortClassLocal = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Local);
                        ViewBag.SortClassLocal = "sorting_desc";
                    }
                    break;
                case "Data":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Data);
                        ViewBag.SortClassData = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Data);
                        ViewBag.SortClassData = "sorting_desc";
                    }
                    break;
                case "Descricao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(b => b.Descricao);
                        ViewBag.SortClassDescricao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Descricao);
                        ViewBag.SortClassDescricao = "sorting_desc";
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

            return pagedList.ToPagedList<Blocos>(pageNumber, 10);
        }

        //
        // GET: /Blocos/Details/5

        public ActionResult Details(int id = 0)
        {
            Blocos blocos = db.Blocos.Find(id);
            if (blocos == null)
            {
                return HttpNotFound();
            }
            return View(blocos);
        }

        //
        // GET: /Blocos/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Blocos/Create

        [HttpPost]
        public ActionResult Create(Blocos blocos, HttpPostedFileBase Imagem, string fotoExistente = "")
        {
            if (!string.IsNullOrEmpty(fotoExistente))
            {
                blocos.Imagem = fotoExistente;
                ModelState.Remove("Imagem");
            }

            if (ModelState.IsValid)
            {
                blocos.Excluido = false;
                blocos.DataCadastro = DateTime.Now;

                db.Blocos.Add(blocos);

                if (db.SaveChanges() > 0)
                {
                    #region image e slug
                    if (Imagem != null)
                    {
                        //   var path1400x629 = Server.MapPath(string.Format(pathFoto1400x629, blocos.Id));
                        var path744x500 = Server.MapPath(string.Format(pathFoto744x500, blocos.Id));
                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, blocos.Id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, blocos.Id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, blocos.Id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, blocos.Id));

                        blocos.Imagem = Utils.SaveFileBase(fileOriginal, Imagem);
                        
                        //Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 1400, 629, path1400x629);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 744, 500, path744x500);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 260, 173, path260x173);
                        
                    }

                    int suffix = 0;

                    do
                    {
                        blocos.Chave = blocos.Nome.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Blocos.Where(o => o.Chave == blocos.Chave).Count() > 0);
                    #endregion

                    db.Entry(blocos).State = EntityState.Modified;
                    db.SaveChanges();

                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, blocos.Id);
                return RedirectToAction("Index");
            }

            return View(blocos);
        }

        //
        // GET: /Blocos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Blocos blocos = db.Blocos.Find(id);
            if (blocos == null)
            {
                return HttpNotFound();
            }
            return View(blocos);
        }

        //
        // POST: /Blocos/Edit/5

        [HttpPost]
        public ActionResult Edit(Blocos blocos, HttpPostedFileBase ImagemUpload, string fotoExistente = "")
        {
            if (!string.IsNullOrEmpty(fotoExistente))
            {
                blocos.Imagem = fotoExistente;
                ModelState.Remove("ImagemUpload");
            }

            if (ModelState.IsValid)
            {
                blocos.Chave = blocos.Nome.GenerateSlug();

                db.Entry(blocos).State = EntityState.Modified;
                db.SaveChanges();

                if (db.SaveChanges() > 0)
                {
                    #region image
                    if (ImagemUpload != null)
                    {
                        //   var path1400x629 = Server.MapPath(string.Format(pathFoto1400x629, blocos.Id));
                        var path744x500 = Server.MapPath(string.Format(pathFoto744x500, blocos.Id));
                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, blocos.Id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, blocos.Id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, blocos.Id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, blocos.Id));

                        blocos.Imagem = Utils.SaveFileBase(fileOriginal, ImagemUpload);
                        //Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 1400, 629, path1400x629);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 744, 500, path744x500);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, blocos.Imagem), 260, 173, path260x173);


                    }
                    #endregion

                    #region slug
                    int suffix = 0;

                    do
                    {
                        blocos.Chave = blocos.Nome.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Blocos.Where(o => o.Chave == blocos.Chave).Count() > 0);
                    #endregion

                    db.Entry(blocos).State = EntityState.Modified;
                    db.SaveChanges();

                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, blocos.Id);
                return RedirectToAction("Index");
            }
            return View(blocos);
        }

        //
        // GET: /Blocos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Blocos blocos = db.Blocos.Find(id);
            if (blocos == null)
            {
                return HttpNotFound();
            }
            return View(blocos);
        }

        //
        // POST: /Blocos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Blocos blocos = db.Blocos.Find(id);
            //db.Blocos.Remove(blocos);
            blocos.Excluido = true;
            db.Entry(blocos).State = EntityState.Modified;
            db.Entry(blocos).Property(x => x.DataCadastro).IsModified = false;
            db.Entry(blocos).Property(x => x.Ativo).IsModified = false;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, blocos.Id);
            return RedirectToAction("Index");
        }


    }
}




