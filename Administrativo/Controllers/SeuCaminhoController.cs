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

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Seu Caminho")]
    public class SeuCaminhoController : BaseController
    {
        protected const string pathFoto700x460 = "~/Conteudo/noticias/{0}/700x460/";
        protected const string pathFoto620x415 = "~/Conteudo/noticias/{0}/620x415/";
        protected const string pathFoto620x200 = "~/Conteudo/noticias/{0}/620x200/";
        protected const string pathFoto300x200 = "~/Conteudo/noticias/{0}/300x200/";
        protected const string pathFoto93x93 = "~/Conteudo/noticias/{0}/93x93/";
        protected const string pathOriginal = "~/Conteudo/noticias/{0}/original/";

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.pathOriginal = pathOriginal;
        }
        //
        // GET: /EditorialNoticias/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.NoticiasActive = "active";
            ViewBag.NoticiasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClasstitulo = "sorting";
            ViewBag.SortClassregiao = "sorting";
            ViewBag.SortClassvisualizacao = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Noticias> GetListItens( int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Noticias.Where(a => !a.excluido && a.transito && a.idColunista == null);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(n => n.id == id || n.titulo.ToLower(). Contains(searchParam.ToLower()) || n.texto.Contains(searchParam) || n.NoticiasRegioes.Titulo.ToLower() == searchParam.ToLower() || n.CategoriasMapa.Titulo.ToLower().Contains(searchParam.ToLower()) || n.url == searchParam);

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
                case "regiao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.NoticiasRegioes.Titulo);
                        ViewBag.SortClassregiao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.NoticiasRegioes.Titulo);
                        ViewBag.SortClassregiao = "sorting_desc";
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

        public ActionResult Details(int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
           
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
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome");
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo");
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
          

            return View();
        }

        //
        // POST: /EditorialNoticias/Create

        [HttpPost]
        public ActionResult Create(Noticias noticias, HttpPostedFileBase audio, HttpPostedFileBase foto, int[] TagsIds)
        {
           
            if (ModelState.IsValid)
            {

                
                noticias.dataCadastro = DateTime.Now;
                noticias.dataAtualizacao = DateTime.Now;
                
                noticias.transito = true;

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
                        var path93x93 = Server.MapPath(string.Format(pathFoto93x93, noticias.id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));


                        noticias.foto = Utils.SaveFileBase(fileOriginal,foto);
                        Utils.SaveFileBase(path93x93, foto, noticias.foto);

                        noticias.fotoMini = noticias.foto;

                        Utils.SaveAndCropImage(foto, path700x460, 0, 0, 700, 460, true, noticias.foto);
                        Utils.SaveAndCropImage(foto, path620x415, 0, 0, 620, 415, true, noticias.foto);
                        Utils.SaveAndCropImage(foto, path620x200, 0, 0, 620, 200, true, noticias.foto);
                        Utils.SaveAndCropImage(foto, path300x200, 0, 0, 300, 200, true, noticias.foto);

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

                
                return RedirectToAction("Index");
            }



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
            
            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Edit/5

        [HttpPost]
        public ActionResult Edit( Noticias noticias, HttpPostedFileBase audioUpload, HttpPostedFileBase fotoUpload, int[] TagsIds)
        {
           if (ModelState.IsValid)
            {

                #region uploads

                if (fotoUpload != null)
                {
                    var path700x460 = Server.MapPath(string.Format(pathFoto700x460, noticias.id));
                    var path620x415 = Server.MapPath(string.Format(pathFoto620x415, noticias.id));
                    var path620x200 = Server.MapPath(string.Format(pathFoto620x200, noticias.id));
                    var path300x200 = Server.MapPath(string.Format(pathFoto300x200, noticias.id));
                    var path93x93 = Server.MapPath(string.Format(pathFoto93x93, noticias.id));

                    var fileOriginal = Server.MapPath(string.Format(pathOriginal, noticias.id));


                    noticias.foto = Utils.SaveFileBase(fileOriginal, fotoUpload);
                    Utils.SaveFileBase(path93x93, fotoUpload, noticias.foto);

                    noticias.fotoMini = noticias.foto;

                    Utils.SaveAndCropImage(fotoUpload, path700x460, 0, 0, 700, 460, true, noticias.foto);
                    Utils.SaveAndCropImage(fotoUpload, path620x415, 0, 0, 620, 415, true, noticias.foto);
                    Utils.SaveAndCropImage(fotoUpload, path620x200, 0, 0, 620, 200, true, noticias.foto);
                    Utils.SaveAndCropImage(fotoUpload, path300x200, 0, 0, 300, 200, true, noticias.foto);

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


                noticias.dataAtualizacao = DateTime.Now;
                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

               
                db.Entry(noticias).Collection("Tags").Load();
                noticias.Tags.Clear();
                

                if (TagsIds != null)
                {
                    foreach (int TagId in TagsIds)
                    {
                        Tags NewTags = db.Tags.Find(TagId);
                        noticias.Tags.Add(NewTags);
                    }
                }
                
               
                
                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

           ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            
           

            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Delete/5

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
        public ActionResult DeleteConfirmed( int id)
        {
            Noticias noticias = db.Noticias.Find(id);
            noticias.excluido = true;
            db.Entry(noticias).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }


    }
}




