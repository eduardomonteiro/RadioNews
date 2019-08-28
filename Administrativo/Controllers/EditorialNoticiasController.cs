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
using AMImageLIB;
using System.IO;
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;
using System.Web.Hosting;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Editorias")]
    public class EditorialNoticiasController : BaseController
    {
        protected const string pathFoto700x460 = "~/Conteudo/noticias/{0}/700x460/";
        protected const string pathFoto620x415 = "~/Conteudo/noticias/{0}/620x415/";
        protected const string pathFoto620x200 = "~/Conteudo/noticias/{0}/620x200/";
        protected const string pathFoto300x200 = "~/Conteudo/noticias/{0}/300x200/";
        protected const string pathFoto205x215 = "~/Conteudo/noticias/{0}/205x215/";
        protected const string pathFoto150x90 = "~/Conteudo/noticias/{0}/150x90/";
        protected const string pathFoto93x93 = "~/Conteudo/noticias/{0}/93x93/";
        protected const string pathFoto500x300 = "~/Conteudo/noticias/{0}/500x300/";
        protected const string pathOriginal = "~/Conteudo/noticias/{0}/original/";

        public int areaADM = 7;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.pathOriginal = pathOriginal;
        }
        //
        // GET: /EditorialNoticias/

        public ActionResult Index(string editorial, int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.NoticiasActive = "active";
            ViewBag.NoticiasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.EditorialChave = db.Editoriais.FirstOrDefault(a => a.chave == editorial).chave;
            allClassOffSort();

            return View(GetListItens(editorial, page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClasstitulo = "sorting";
            ViewBag.SortClassdestaqueHome = "sorting";
            ViewBag.SortClassvisualizacao = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Noticias> GetListItens(string editorial, int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Noticias.Where(a => !a.excluido && !a.transito && a.idColunista == null && a.Editoriais.Any(x => x.chave == editorial));

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(n => n.id == id || n.titulo.ToLower().Contains(searchParam.ToLower()) || n.texto.ToLower().Contains(searchParam.ToLower()));

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
                    pagedList = pagedList.OrderByDescending(n => n.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Noticias>(pageNumber, 10);
        }

        //
        // GET: /EditorialNoticias/Details/5

        public ActionResult Details(string editorial, int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.EditorialChave = db.Editoriais.FirstOrDefault(a => a.chave == editorial).chave;
            if (noticias == null)
            {
                return HttpNotFound();
            }
            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Create

        public ActionResult Create(string editorial)
        {
            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a=>!a.excluido), "id", "nome");
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo");
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes, "Id", "Titulo");
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");

            
            var editoria = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a=> !a.Excluido && a.EditoriaId == editoria.id).ToList(), "Id", "Titulo");

            ViewBag.Editorial = editoria;
            ViewBag.EditorialChave = editoria.chave;

            return View();
        }

        //
        // POST: /EditorialNoticias/Create

        [HttpPost]
        public ActionResult Create(string editorial, Noticias noticias, HttpPostedFileBase audio, HttpPostedFileBase foto, int[] TagsIds, int CategoriaId = 0)
        {
            ModelState["dataAtualizacao"]?.Errors.Clear();

            var editoria = db.Editoriais.FirstOrDefault(a => a.chave == editorial);

            if (CategoriaId == 0)
            {
                ModelState.AddModelError("CategoriaId", "Selecione uma categoria");
                
            }
            else if (ModelState.IsValid)
            {

                var categoria = db.Categorias.FirstOrDefault(a => a.Id == CategoriaId);
                noticias.Categorias.Add(categoria);
                noticias.dataCadastro = DateTime.Now;
                if (noticias.dataAtualizacao == DateTime.MinValue || noticias.dataAtualizacao == null)
                {
                    noticias.dataAtualizacao = DateTime.Now;

                }

                noticias.transito = false;

                db.Noticias.Add(noticias);

                if (TagsIds != null)
                {
                    noticias.Tags = new List<Tags>();
                    foreach (var tagId in TagsIds)
                    {
                        var tag = db.Tags.Find(tagId);
                        noticias.Tags.Add(tag);
                    }
                }

                if (db.SaveChanges() > 0)
                {
                    #region uploads

                    if (foto != null)
                    {
                        var path700x460 = Server.MapPath(string.Format(pathFoto700x460, noticias.id));
                        var path620x415 = Server.MapPath(string.Format(pathFoto620x415, noticias.id));
                        var path620x200 = Server.MapPath(string.Format(pathFoto620x200, noticias.id));
                        var path300x200 = Server.MapPath(string.Format(pathFoto300x200, noticias.id));
                        var path205x125 = Server.MapPath(string.Format(pathFoto205x215, noticias.id));
                        var path150x90 = Server.MapPath(string.Format(pathFoto150x90, noticias.id));
                        var path500x300 = Server.MapPath(string.Format(pathFoto500x300, noticias.id));
                        var path93x93 = Server.MapPath(string.Format(pathFoto93x93, noticias.id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));


                        noticias.foto = Utils.SaveFileBase(fileOriginal,foto);
                        //Utils.SaveFileBase(path93x93, foto, noticias.foto);

                        noticias.fotoMini = path205x125 + noticias.foto;

                        //Utils.SaveAndCropImage(foto, path700x460, 0, 0, 700, 460, true, noticias.foto);
                        //Utils.SaveAndCropImage(foto, path620x415, 0, 0, 620, 415, true, noticias.foto);
                        //Utils.SaveAndCropImage(foto, path620x200, 0, 0, 620, 200, true, noticias.foto);
                        //Utils.SaveAndCropImage(foto, path300x200, 0, 0, 300, 200, true, noticias.foto);
                        //Utils.SaveAndCropImage(foto, path205x125, 0, 0, 205, 125, true, noticias.foto);
                        //    Utils.SaveAndCropImage(foto, path150x90, 0, 0, 150, 90, true, noticias.foto);

                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 700, 460, path700x460);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 620, 415, path620x415);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 620, 200, path620x200);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 300, 200, path300x200);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 205, 125, path205x125);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 500, 300, path205x125);
                        Utils.resizeImageAndSave(fileOriginal + noticias.foto, 150, 90, path150x90);

                        if (!Directory.Exists(path93x93))
                        {
                            Directory.CreateDirectory(path93x93);
                        }
                        Utils.resizeImageAndSave(path300x200 + noticias.foto, 93, 93, path93x93);
                        

                    };

                    int suffix = 0;

                    do
                    {
                        
                        noticias.url = noticias.titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Noticias.Where(o => o.url == noticias.url).Count() > 0);


                    if (audio != null)
                    {
                        var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                        noticias.audio = Utils.SaveFileBase(path, audio);

                    }



                    db.Entry(noticias).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, noticias.id);
                return RedirectToAction("Index", new { editorial = editorial });
            }

            
            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editoria.id).ToList(), "Id", "Titulo");

            ViewBag.Editorial = editoria;
            ViewBag.EditorialChave = editoria.chave;

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Edit/5

        public ActionResult Edit(string editorial, int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo", noticias.Tags.Select(n => n.Id));

            var editoria = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editoria.id).ToList(), "Id", "Titulo", noticias.Categorias.FirstOrDefault().Id);

            ViewBag.Editorial = editoria;
            ViewBag.EditorialChave = editoria.chave;
            
            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Edit/5

        [HttpPost]
        public ActionResult Edit(string editorial, Noticias noticias, HttpPostedFileBase audioUpload, HttpPostedFileBase fotoUpload, int[] TagsIds, int CategoriaId = 0)
        {
            var editoria = db.Editoriais.FirstOrDefault(a => a.chave == editorial);

            if (CategoriaId == 0)
            {
                ModelState.AddModelError("CategoriaId", "Selecione uma categoria");

            }
            else if (ModelState.IsValid)
            {

                #region uploads

                if (fotoUpload != null)
                {
                    var path700x460 = Server.MapPath(string.Format(pathFoto700x460, noticias.id));
                    var path620x415 = Server.MapPath(string.Format(pathFoto620x415, noticias.id));
                    var path620x200 = Server.MapPath(string.Format(pathFoto620x200, noticias.id));
                    var path300x200 = Server.MapPath(string.Format(pathFoto300x200, noticias.id));
                    var path205x125 = Server.MapPath(string.Format(pathFoto205x215, noticias.id));
                    var path150x90 = Server.MapPath(string.Format(pathFoto150x90, noticias.id));
                    var path93x93 = Server.MapPath(string.Format(pathFoto93x93, noticias.id));
                    var path500x300 = Server.MapPath(string.Format(pathFoto500x300, noticias.id));

                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));
                    //var file2 = new System.Drawing.Bitmap(fileOriginal);


                    noticias.foto = Utils.SaveFileBase(fileOriginal, fotoUpload);
                    Utils.SaveFileBase(path93x93, fotoUpload, noticias.foto);

                    noticias.fotoMini = noticias.foto;

                    //Utils.SaveAndCropImage(fotoUpload, path700x460, 0, 0, 700, 460, true, noticias.foto);
                    //Utils.SaveAndCropImage(fotoUpload, path620x415, 0, 0, 620, 415, true, noticias.foto);
                    //Utils.SaveAndCropImage(fotoUpload, path620x200, 0, 0, 620, 200, true, noticias.foto);
                    //Utils.SaveAndCropImage(fotoUpload, path300x200, 0, 0, 300, 200, true, noticias.foto);
                    //Utils.SaveAndCropImage(fotoUpload, path205x125, 0, 0, 205, 125, true, noticias.foto);
                    //Utils.SaveAndCropImage(fotoUpload, path150x90, 0, 0, 150, 90, true, noticias.foto);

                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 700, 460, path700x460);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 620, 415, path620x415);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 620, 200, path620x200);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 300, 200, path300x200);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 205, 125, path205x125);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 150, 90, path150x90);
                    Utils.resizeImageAndSave(fileOriginal + noticias.foto, 500, 300, path500x300);
                    if (!Directory.Exists(path93x93))
                    {
                        Directory.CreateDirectory(path93x93);
                    }
                    Utils.resizeImageAndSave(path300x200 + noticias.foto, 93, 93, path93x93);


                };

                int suffix = 0;

                do
                {

                    noticias.url = noticias.titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                    suffix++;
                } while (db.Noticias.Where(o => o.url == noticias.url && o.id != noticias.id).Count() > 0);


                if (audioUpload != null)
                {
                    var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                    noticias.audio = Utils.SaveFileBase(path, audioUpload);

                }


                //noticias.dataAtualizacao = DateTime.Now;
                if (noticias.dataAtualizacao == null)
                {
                    noticias.dataAtualizacao = DateTime.Now;
                }

                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                var categoria = db.Categorias.Find(CategoriaId);

                
                db.Entry(noticias).Collection("Categorias").Load();
                db.Entry(noticias).Collection("Tags").Load();
                noticias.Tags.Clear();
                noticias.Categorias.Clear();

                if (TagsIds != null)
                {
                    foreach (int TagId in TagsIds)
                    {
                        Tags NewTags = db.Tags.Find(TagId);
                        noticias.Tags.Add(NewTags);
                    }
                }
                
                noticias.Categorias.Add(categoria);

                
                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                Site.Services.RedisService.FlushAll(noticias.url);
                Site.Services.RedisService.FlushAll(noticias.id.ToString());
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, noticias.id);
                return RedirectToAction("Index", new { editorial = editorial });
            }
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo", noticias.Tags.Select(n => n.Id));

            ViewBag.CategoriaId = new SelectList(db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editoria.id).ToList(), "Id", "Titulo", noticias.Categorias.FirstOrDefault().Id);

            ViewBag.Editorial = editoria;
            ViewBag.EditorialChave = editoria.chave;

            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Delete/5

        public ActionResult Delete(string editorial, int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }
            var editor = db.Editoriais.FirstOrDefault(a => a.chave == editorial);
            ViewBag.Editorial = editor;
            ViewBag.EditorialChave = editor.chave;
            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(string editorial, int id)
        {
            Noticias noticias = db.Noticias.Find(id);
            noticias.excluido = true;
            db.Entry(noticias).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, noticias.id);
            return RedirectToAction("Index", new { editorial = editorial });
        }
    }
}




