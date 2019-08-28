using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Times")]
    public class ElencosController : BaseController
    {
        protected const string pathFoto130x100 = "~/Conteudo/times/{0}/elenco/130x100/";
        protected const string pathFoto70x40 = "~/Conteudo/times/{0}/elenco/70x40/";
        protected const string pathOriginal = "~/Conteudo/times/{0}/original/";
        public int areaADM = 26;
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto130x100;
        }
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int timeId = 0)
        {

            ViewBag.BlocosActive = "active";
            ViewBag.BlocosSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            ViewBag.TimeId = timeId;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order, timeId));

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

        public IPagedList<Elencos> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", int timeId = 0)
        {

            var list = db.Elencos.Where(x => !x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);


            if (timeId > 0)
            {
                pagedList = pagedList.Where(b => b.TimeId == timeId);
            }


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
                        pagedList = pagedList.OrderBy(b => b.Foto);
                        ViewBag.SortClassImagem = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(b => b.Foto);
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

            return pagedList.ToPagedList<Elencos>(pageNumber, 10);
        }



        //
        // GET: /Blocos/Details/5

        public ActionResult Details(int id = 0)
        {
            Elencos time = db.Elencos.Find(id);
            if (time == null)
            {
                return HttpNotFound();
            }
            return View(time);
        }

        //
        // GET: /Blocos/Create

        public ActionResult Create(int timeId = 0)
        {
            ViewBag.idTime = timeId;
            ViewBag.TimeId = new SelectList(db.Times.Where(a => !a.Excluido && a.Ativo), "Id", "Nome", (timeId > 0 ? timeId : 0));
            return View();
        }

        //
        // POST: /Times/Create

        [HttpPost]
        public ActionResult Create(Elencos jogador, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                jogador.Excluido = false;
                jogador.DataCadastro = DateTime.Now;
                db.Elencos.Add(jogador);

                if (db.SaveChanges() > 0)
                {
                    #region image e slug
                    if (Foto != null)
                    {

                        var path48x48 = Server.MapPath(string.Format(pathFoto130x100, jogador.TimeId));
                        var path28x28 = Server.MapPath(string.Format(pathFoto70x40, jogador.TimeId));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, jogador.TimeId));

                        jogador.Foto = Utils.SaveFileBase(fileOriginal, Foto);


                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, jogador.Foto), 130, 100, path48x48);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, jogador.Foto), 70, 40, path28x28);

                    }
                    
                    #endregion

                    db.Entry(jogador).State = EntityState.Modified;
                    db.SaveChanges();

                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, jogador.Id);
                return RedirectToAction("Index", new { timeId = jogador.TimeId });
            }
            ViewBag.idTime = jogador.TimeId;
            ViewBag.TimeId = new SelectList(db.Times.Where(a => !a.Excluido && a.Ativo), "Id", "Nome", (jogador.TimeId > 0 ? jogador.TimeId : 0));

            return View(jogador);
        }

        //
        // GET: /Blocos/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Elencos jogador = db.Elencos.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }

            ViewBag.idTime = jogador.TimeId;
            ViewBag.TimeId = new SelectList(db.Times.Where(a => !a.Excluido && a.Ativo), "Id", "Nome", (jogador.TimeId > 0 ? jogador.TimeId : 0));

            return View(jogador);
        }


        [HttpPost]
        public ActionResult Edit(Elencos jogador, HttpPostedFileBase FotoUpload)
        {

            if (ModelState.IsValid)
            {
                jogador.DataCadastro = DateTime.Now;
                db.Entry(jogador).State = EntityState.Modified;


                if (db.SaveChanges() > 0)
                {
                    #region image
                    if (FotoUpload != null)
                    {
                        var path48x48 = Server.MapPath(string.Format(pathFoto130x100, jogador.TimeId));
                        var path28x28 = Server.MapPath(string.Format(pathFoto70x40, jogador.TimeId));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, jogador.TimeId));

                        jogador.Foto = Utils.SaveFileBase(fileOriginal, FotoUpload);

                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, jogador.Foto), 130, 100, path48x48);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, jogador.Foto), 70, 40, path28x28);


                    }
                    #endregion


                    db.Entry(jogador).State = EntityState.Modified;
                    db.SaveChanges();

                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, jogador.Id);
                return RedirectToAction("Index", new { timeId = jogador.TimeId });
            }

            ViewBag.idTime = jogador.TimeId;
            ViewBag.TimeId = new SelectList(db.Times.Where(a => !a.Excluido && a.Ativo), "Id", "Nome", (jogador.TimeId > 0 ? jogador.TimeId : 0));

            return View(jogador);
        }

        //
        // GET: /Blocos/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Elencos jogador = db.Elencos.Find(id);
            if (jogador == null)
            {
                return HttpNotFound();
            }
            return View(jogador);
        }

        //
        // POST: /Blocos/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Elencos jogador = db.Elencos.Find(id);
            jogador.Excluido = true;
            db.Entry(jogador).State = EntityState.Modified;
            db.Entry(jogador).Property(x => x.DataCadastro).IsModified = false;
            db.Entry(jogador).Property(x => x.Ativo).IsModified = false;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, jogador.Id);
            return RedirectToAction("Index", new { timeId = jogador.TimeId });
        }



    }
}
