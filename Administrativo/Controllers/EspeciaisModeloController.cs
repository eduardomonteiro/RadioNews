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
    [CustomAuthorize(Roles = "Administrador,Especiais")]
    public class EspeciaisModeloController : BaseController
    {
        //
        // GET: /EspeciaisModelo/

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
        // GET: /EspecialParlamentoAberto/
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int EspecialId = 0)
        {
            ViewBag.NoticiasActive = "active";
            ViewBag.NoticiasSubActive = "active";
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


                if (especial.Especiais_Modelos.Tipo.ToLower().Contains("carnaval"))
                {
                    ViewBag.Carnaval = true;
                }

            }

            allClassOffSort();

            return View(GetListItens(page, search, sortField, order, EspecialId));

        }


        public void allClassOffSort()
        {
            ViewBag.SortClassid = "sorting";
            ViewBag.SortClasstitulo = "sorting";
            ViewBag.SortClassdestaqueHome = "sorting";
            ViewBag.SortClassvisualizacao = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";
        }

        public IPagedList<Noticias> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", int EspecialId = 0)
        {

            var list = db.Noticias.Where(a => !a.excluido && !a.transito && a.idColunista == null);

            if (EspecialId > 0)
            {
                list = list.Where(a => a.Editoriais.Any(x => x.id == EspecialId));
            }

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {

                pagedList = pagedList.Where(n => n.titulo.ToLower().Contains(searchParam.ToLower()) || n.texto.Contains(searchParam));

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.titulo);
                        ViewBag.SortClasstitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.titulo);
                        ViewBag.SortClasstitulo = "sorting_desc";
                    }
                    break;
                case "destaqueHome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.destaqueHome);
                        ViewBag.SortClassdestaqueHome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.destaqueHome);
                        ViewBag.SortClassdestaqueHome = "sorting_desc";
                    }
                    break;
                case "visualizacao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.visualizacao);
                        ViewBag.SortClassvisualizacao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.visualizacao);
                        ViewBag.SortClassvisualizacao = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(n => n.dataCadastro);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Noticias>(pageNumber, 10);
        }


        public ActionResult Create(string EditorialId = "")
        {
            int id = Convert.ToInt32(EditorialId);
            var editorial = db.Editoriais.FirstOrDefault(x => x.id == id);
            ViewBag.EspecialNome = editorial.nome;
            ViewBag.EditorialId = EditorialId;
            ViewBag.TemSecao = editorial.Especiais_Modelos.TemSecao;
            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == id).ToList(), "Id", "Titulo");

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Noticias noticias, HttpPostedFileBase foto, HttpPostedFileBase audio, string fotoExistente = "", string EditorialId = "", int CategoriaId = 0)
        {
            ModelState["dataAtualizacao"]?.Errors.Clear();

            if (ModelState.IsValid)
            {
                var categoria = db.Categorias.FirstOrDefault(a => a.Id == CategoriaId);
                noticias.Categorias.Add(categoria);

                noticias.dataCadastro = DateTime.Now;
                if (noticias.dataAtualizacao != DateTime.MinValue || noticias.dataAtualizacao != null)
                {
                    noticias.dataAtualizacao = DateTime.Now;

                }

                noticias.transito = false;

                db.Noticias.Add(noticias);

                if (db.SaveChanges() > 0)
                {

                    if (foto != null)
                    {
                        var path744x500 = Server.MapPath(string.Format(pathFoto744x500, noticias.id));
                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, noticias.id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, noticias.id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, noticias.id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));

                        noticias.foto = Utils.SaveFileBase(fileOriginal, foto);

                        noticias.fotoMini = noticias.foto;

                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 744, 500, path744x500);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 260, 173, path260x173);
                    }

                    int suffix = 0;

                    do
                    {
                        noticias.url = noticias.titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Noticias.Where(o => o.url == noticias.url).Count() > 0);


                    if (!string.IsNullOrEmpty(fotoExistente))
                    {
                        noticias.foto = fotoExistente;
                    }


                    if (audio != null)
                    {
                        var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                        noticias.audio = Utils.SaveFileBase(path, audio);
                    }

                    if (!string.IsNullOrEmpty(EditorialId))
                    {
                        int idEditorial = Convert.ToInt32(EditorialId);
                        var editorial = db.Editoriais.FirstOrDefault(x => x.id == idEditorial);
                        if (editorial != null)
                        {
                            noticias.Editoriais.Add(editorial);
                            db.SaveChanges();
                        }
                    }
                    db.Entry(noticias).State = EntityState.Modified;
                    db.SaveChanges();
                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), getAreaADM(Convert.ToInt32(EditorialId)), TipoAcesso.Insercao, noticias.id);
                return RedirectToAction("Index", new { EspecialId = EditorialId });
            }


            int id = Convert.ToInt32(EditorialId);
            var editorial1 = db.Editoriais.FirstOrDefault(x => x.id == id);
            ViewBag.EspecialNome = editorial1.nome;
            ViewBag.EditorialId = EditorialId;
            ViewBag.TemSecao = editorial1.Especiais_Modelos.TemSecao;
            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == id).ToList(), "Id", "Titulo");

            return View(noticias);
        }


        public ActionResult Edit(int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);

            if (noticias == null)
            {

                return HttpNotFound();
            }
            int editorialId = noticias.Editoriais.FirstOrDefault().id;
            var editorial = db.Editoriais.FirstOrDefault(x => x.id == editorialId);
            ViewBag.TemSecao = editorial.Especiais_Modelos.TemSecao;
            ViewBag.EspecialNome = editorial.nome;
            ViewBag.EditorialId = editorial.id;

            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editorialId).ToList(), "Id", "Titulo", noticias.Categorias.FirstOrDefault().Id);
            return View(noticias);
        }

        public ActionResult Details(int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);

            if (noticias == null)
            {
                return HttpNotFound();
            }

            return View(noticias);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Noticias noticias, HttpPostedFileBase fotoUpload, HttpPostedFileBase audioUpload, string fotoExistente = "", string EditorialId = "", int CategoriaId = 0)
        {
            if (ModelState.IsValid)
            {
                noticias.dataCadastro = DateTime.Now;
                if (noticias.dataAtualizacao == DateTime.MinValue || noticias.dataAtualizacao == null)
                {
                    noticias.dataAtualizacao = DateTime.Now;
                }
                noticias.transito = false;
                db.Noticias.Add(noticias);

                db.Entry(noticias).State = EntityState.Modified;

                if (db.SaveChanges() > 0)
                {

                    if (fotoUpload != null)
                    {
                        var path744x500 = Server.MapPath(string.Format(pathFoto744x500, noticias.id));
                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, noticias.id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, noticias.id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, noticias.id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));

                        noticias.foto = Utils.SaveFileBase(fileOriginal, fotoUpload);

                        noticias.fotoMini = noticias.foto;

                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 744, 500, path744x500);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, noticias.foto), 260, 173, path260x173);


                    }

                    int suffix = 0;

                    do
                    {
                        noticias.url = noticias.titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Noticias.Where(o => o.url == noticias.url).Count() > 0);


                    if (!string.IsNullOrEmpty(fotoExistente))
                    {
                        noticias.foto = fotoExistente;
                    }

                    if (audioUpload != null)
                    {
                        var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                        noticias.audio = Utils.SaveFileBase(path, audioUpload);
                    }


                    #region categorias
                    if (CategoriaId > 0)
                    {
                        db.Entry(noticias).Collection("Categorias").Load();
                        noticias.Categorias.Clear();

                        int idCategoria = Convert.ToInt32(CategoriaId);
                        var editorial = db.Categorias.FirstOrDefault(x => x.Id == idCategoria);
                        if (editorial != null)
                        {
                            noticias.Categorias.Add(editorial);

                        }
                    }
                    #endregion

                    #region Editoriais
                    if (!string.IsNullOrEmpty(EditorialId))
                    {
                        db.Entry(noticias).Collection("Editoriais").Load();
                        noticias.Editoriais.Clear();

                        int idEditorial = Convert.ToInt32(EditorialId);
                        var editorial = db.Editoriais.FirstOrDefault(x => x.id == idEditorial);
                        if (editorial != null)
                        {
                            noticias.Editoriais.Add(editorial);

                        }
                    }
                    #endregion

                    db.SaveChanges();

                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), getAreaADM(Convert.ToInt32(EditorialId)), TipoAcesso.Edicao, noticias.id);
                return RedirectToAction("Index", new { EspecialId = EditorialId });
            }

            return View(noticias);
        }




        public ActionResult Delete(int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }

            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Noticias noticias = db.Noticias.Find(id);
            noticias.excluido = true;
            db.Entry(noticias).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), getAreaADM(noticias.Editoriais.FirstOrDefault().id), TipoAcesso.Edicao, noticias.id);
            return RedirectToAction("Index", new { EspecialId = noticias.Editoriais.FirstOrDefault().id });
        }


        public int getAreaADM(int EspecialId)
        {
            switch(EspecialId)
            {
                case 3: // id editoria especial 
                  return 28; // value areaADM tabela

                case 4:
                    return 29;

                case 6:
                    return 37;
                case 8:
                    return 31;

                case 9:
                    return 32;

                case 10:
                    return 33;
                case 42:
                    return 41;
                case 46:
                    return 48;
                default:
                    return 0;
            }
        }

    }


}

