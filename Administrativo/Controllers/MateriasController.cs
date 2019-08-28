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
using WebMatrix.WebData;
using Administrativo.Helpers;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Colunistas")]
    public class MateriasController : BaseController
    {
        protected const string pathFoto744x500 = "~/Conteudo/noticias/{0}/744x500/";
        protected const string pathFoto405x270 = "~/Conteudo/noticias/{0}/405x270/";
        protected const string pathFoto365x240 = "~/Conteudo/noticias/{0}/365x240/";
        protected const string pathFoto260x173 = "~/Conteudo/noticias/{0}/260x173/";
        protected const string pathOriginal = "~/Conteudo/noticias/{0}/original/";
        public int areaADM = 16;
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto744x500;
        }
        //
        // GET: /EditorialNoticias/

        public ActionResult Index(int? page, int? colunistaId, string search = "", string sortField = "", string order = "")
        {
            ViewBag.colunistaId = colunistaId;
            ViewBag.NoticiasActive = "active";
            ViewBag.NoticiasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;

            ViewBag.Colunistas = new SelectList(db.Colunista.Where(c => c.liberado).ToList(), "Id", "Nome");


            allClassOffSort();

            return View(GetListItens(page, colunistaId, search, sortField, order));

        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClasstitulo = "sorting";
            ViewBag.SortClassregiao = "sorting";
            ViewBag.SortClasscolunista = "sorting";
            ViewBag.SortClassvisualizacao = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Noticias> GetListItens(int? page, int? colunistaId, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.Noticias.Where(a => !a.excluido && !a.transito && a.idColunista != null && !a.Colunista.excluido);
            var colunistaUserId = WebSecurity.GetUserId(User.Identity.Name);


            var userColunista = _db.UserProfiles.FirstOrDefault(a => a.UserId == colunistaUserId);

            if (userColunista.ColunistaId.HasValue)
            {
                list = list.Where(a => a.idColunista == userColunista.ColunistaId);
            }

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(n => n.id == id || n.titulo.ToLower().Contains(searchParam.ToLower()) || n.texto.Contains(searchParam.ToLower()) || n.NoticiasRegioes.Titulo.ToLower() == searchParam.ToLower() || n.CategoriasMapa.Titulo.ToLower().Contains(searchParam.ToLower()) || n.url == searchParam.ToLower() || n.Colunista.nome.ToLower().Contains(searchParam.ToLower()));

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
                case "colunista":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.idColunista);
                        ViewBag.SortClasscolunista = "sorting_asc";
                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.idColunista);
                        ViewBag.SortClasscolunista = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(n => n.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            if (colunistaId != null)
            {
                pagedList = pagedList.Where(p => p.idColunista == colunistaId);
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
            ViewBag.idEditoria = new SelectList(db.Editoriais.Where(x => !x.excluido && !x.especial), "id", "nome");

            if (User.IsInRole("Colunistas"))
            {
                var colunistaUserId = WebSecurity.GetUserId(User.Identity.Name);
                var codColunista = _db.UserProfiles.FirstOrDefault(x => x.UserId == colunistaUserId).ColunistaId;

                ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido && a.id == codColunista), "id", "nome");
            }
            else
            {
                ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome");
            }


            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo");
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();


            return View();
        }

        //
        // POST: /EditorialNoticias/Create
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Noticias noticias, HttpPostedFileBase audio, HttpPostedFileBase foto, string ListaTags = "", string fotoExistente = "", string idEditoria = "")
        {
            ModelState["dataAtualizacao"]?.Errors.Clear();

            if (!string.IsNullOrEmpty(fotoExistente))
            {
                noticias.foto = fotoExistente;
                ModelState.Remove("foto");
            }

            if (User.IsInRole("Colunistas"))
            {
                var colunistaUserId = WebSecurity.GetUserId(User.Identity.Name);

                var userColunista = _db.UserProfiles.FirstOrDefault(a => a.UserId == colunistaUserId);

                noticias.idColunista = userColunista.ColunistaId;
            }
            else if (noticias.idColunista == null)
            {
                ModelState.AddModelError("idColunista", "A matéria deve conter um colunista associado.");
            }

            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(noticias.videoYoutube))
                {
                    if (noticias.videoYoutube.Contains("="))
                    {
                        string[] keyYoutube = noticias.videoYoutube.Split(new char[] { '=' });
                        noticias.videoYoutube = keyYoutube[1];
                    }
                }

                noticias.dataCadastro = DateTime.Now;
                if (noticias.dataAtualizacao == DateTime.MinValue || noticias.dataAtualizacao == null)
                {
                    noticias.dataAtualizacao = DateTime.Now;

                }


                noticias.transito = false;

                db.Noticias.Add(noticias);


                #region Tags
                if (!string.IsNullOrEmpty(ListaTags))
                {
                    noticias.Tags = new List<Tags>();
                    string[] tags = ListaTags.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        var hasTag = db.Tags.FirstOrDefault(x => x.Titulo.ToLower() == item.ToLower());

                        if (hasTag != null)
                        {
                            noticias.Tags.Add(hasTag);
                        }
                        else
                        {
                            var obj = new Tags
                            {
                                chave = item.GenerateSlug(),
                                DataCadastro = DateTime.Now,
                                Excluido = false,
                                Titulo = item
                            };

                            db.Tags.Add(obj);
                            db.SaveChanges();

                            noticias.Tags.Add(obj);
                        }

                    }


                }
                #endregion


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
                        noticias.url = (!string.IsNullOrWhiteSpace(noticias.chamada) ?
                            noticias.chamada.GenerateSlug() :
                            !string.IsNullOrWhiteSpace(noticias.TituloCapa) ?
                            noticias.TituloCapa.GenerateSlug() : noticias.titulo.GenerateSlug()) + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Noticias.Where(o => o.url == noticias.url).Count() > 0);


                    if (audio != null)
                    {
                        var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                        noticias.audio = Utils.SaveFileBase(path, audio);

                    }

                    db.Entry(noticias).State = EntityState.Modified;
                    db.SaveChanges();


                    if (!string.IsNullOrEmpty(idEditoria))
                    {
                        int idEditorial = Convert.ToInt32(idEditoria);
                        var editorial = db.Editoriais.FirstOrDefault(x => x.id == idEditorial);
                        if (editorial != null)
                        {
                            noticias.Editoriais.Add(editorial);
                            db.SaveChanges();
                        }
                    }

                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, noticias.id);
                return RedirectToAction("Index");
            }


            ViewBag.idEditoria = new SelectList(db.Editoriais.Where(x => !x.excluido && !x.especial), "id", "nome");
            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();


            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Edit/5

        public ActionResult Edit(string editorial, int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);

            if (!string.IsNullOrEmpty(noticias.videoYoutube))
            {
                noticias.videoYoutube = "https://www.youtube.com/watch?v=" + noticias.videoYoutube;
            }

            if (noticias == null)
            {
                return HttpNotFound();
            }
            ViewBag.Editorial = db.Editoriais.FirstOrDefault(a => a.chave == editorial);

            ViewBag.idEditoria = new SelectList(db.Editoriais.Where(x => !x.excluido && !x.especial), "id", "nome", noticias.Editoriais != null && noticias.Editoriais.Any() ? noticias.Editoriais.FirstOrDefault().id : 0);

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            var tagsNoticia = noticias.Tags.Select(x => x.Titulo).ToArray();

            ViewBag.tagsNoticia = string.Join(",", tagsNoticia);

            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Edit/5

        [HttpPost]
        public ActionResult Edit(Noticias noticias, HttpPostedFileBase audioUpload, HttpPostedFileBase fotoUpload, string ListaTags = "", string fotoExistente = "", string idEditoria = "")
        {
            if (!string.IsNullOrEmpty(fotoExistente))
            {
                noticias.foto = fotoExistente;
                ModelState.Remove("fotoUpload");
            }

            if (ModelState.IsValid)
            {

                #region uploads

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
                    noticias.url = (!string.IsNullOrWhiteSpace(noticias.chamada) ?
                            noticias.chamada.GenerateSlug() :
                            !string.IsNullOrWhiteSpace(noticias.TituloCapa) ?
                            noticias.TituloCapa.GenerateSlug() : noticias.titulo.GenerateSlug()) + (suffix > 0 ? suffix.ToString() : "");
                    suffix++;
                } while (db.Noticias.Where(o => o.url == noticias.url && o.id != noticias.id).Count() > 0);


                if (audioUpload != null)
                {
                    var path = Server.MapPath("~/conteudo/noticias/" + noticias.id + "/audio/");
                    noticias.audio = Utils.SaveFileBase(path, audioUpload);

                }


                if (!string.IsNullOrEmpty(fotoExistente))
                {
                    noticias.foto = fotoExistente;
                }

                //noticias.dataAtualizacao = DateTime.Now;
                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                #endregion

                if (noticias.videoYoutube != null)
                {
                    if (noticias.videoYoutube.Contains("="))
                    {
                        string[] keyYoutube = noticias.videoYoutube.Split(new char[] { '=' });
                        noticias.videoYoutube = keyYoutube[1];
                    }
                }

                #region Tags
                db.Entry(noticias).Collection("Tags").Load();
                noticias.Tags.Clear();


                if (!string.IsNullOrEmpty(ListaTags))
                {
                    noticias.Tags = new List<Tags>();
                    string[] tags = ListaTags.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        var hasTag = db.Tags.FirstOrDefault(x => x.Titulo.ToLower() == item.ToLower());

                        if (hasTag != null)
                        {
                            noticias.Tags.Add(hasTag);
                        }
                        else
                        {
                            var obj = new Tags
                            {
                                chave = item.GenerateSlug(),
                                DataCadastro = DateTime.Now,
                                Excluido = false,
                                Titulo = item
                            };

                            db.Tags.Add(obj);
                            db.SaveChanges();

                            noticias.Tags.Add(obj);
                        }

                    }


                }
                #endregion


                #region Editoriais
                if (!string.IsNullOrEmpty(idEditoria))
                {
                    db.Entry(noticias).Collection("Editoriais").Load();
                    noticias.Editoriais.Clear();

                    int idEditorial = Convert.ToInt32(idEditoria);
                    var editorial = db.Editoriais.FirstOrDefault(x => x.id == idEditorial);
                    if (editorial != null)
                    {
                        noticias.Editoriais.Add(editorial);
                        //  db.SaveChanges();
                    }
                }
                #endregion


                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                Site.Services.RedisService.FlushAll(noticias.url);
                Site.Services.RedisService.FlushAll(noticias.id.ToString());
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, noticias.id);
                return RedirectToAction("Index");
            }

            ViewBag.idEditoria = new SelectList(db.Editoriais.Where(x => !x.excluido && !x.especial), "id", "nome");

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();




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
        public ActionResult DeleteConfirmed(int id)
        {
            Noticias noticias = db.Noticias.Find(id);
            noticias.excluido = true;
            db.Entry(noticias).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, noticias.id);
            return RedirectToAction("Index");
        }


    }
}




