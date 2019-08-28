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
using System.Web.Mvc.Html;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Esportes")]
    public class NoticiaEsportesController : BaseController
    {
        protected const string pathFoto744x500 = "~/Conteudo/noticias/{0}/744x500/";
        protected const string pathFoto405x270 = "~/Conteudo/noticias/{0}/405x270/";
        protected const string pathFoto365x240 = "~/Conteudo/noticias/{0}/365x240/";
        protected const string pathFoto260x173 = "~/Conteudo/noticias/{0}/260x173/";
        protected const string pathOriginal = "~/Conteudo/noticias/{0}/original/";
        public int areaADM = 18;
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto744x500;
        }
        //
        // GET: /EditorialNoticias/

        public ActionResult Index(string editorial, int? page, string search = "", string sortField = "", string order = "", int EditoriaId = 0)
        {

            ViewBag.NoticiasActive = "active";
            ViewBag.NoticiasSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            //ViewBag.EditorialId = EditoriaId;

            allClassOffSort();

            #region select editoriais

            var editoriais = db.Editoriais.Where(a => !a.excluido && a.esportes).ToList();

            if (EditoriaId > 0)
            {
                ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome", EditoriaId);
            }
            else if (!string.IsNullOrEmpty(editorial))
            {
                var selectedEditorial = db.Editoriais.FirstOrDefault(a => !a.excluido && a.chave == editorial);
                ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome", selectedEditorial.id);
                ViewBag.EditorialId = selectedEditorial.id;
                ViewBag.EditoriaEspecialNome = selectedEditorial.nome;
            }
            else
            {
                ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome");
            }
            #endregion

            return View(GetListItens(editorial, page, search, sortField, order, EditoriaId));

        }

        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClasschamada = "sorting";
            ViewBag.SortClassdestaqueHome = "sorting";
            ViewBag.SortClassvisualizacao = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";
            ViewBag.SortClassativo = "sorting";
            ViewBag.SortClasstipoDestaque = "sorting";

        }

        public IPagedList<Noticias> GetListItens(string editorial, int? page, string searchParam = "", string sortField = "", string order = "", int EditoriaId = 0)
        {

            var list = db.Noticias
                .Where(a => !a.excluido && !a.transito && a.idColunista == null &&
                a.Editoriais.Any(x => x.esportes));

            if (EditoriaId > 0)
            {
                list = list.Where(a => a.Editoriais.Any(x => x.id == EditoriaId));
            }

            if (!string.IsNullOrEmpty(editorial))
            {
                list = list.Where(a => a.Editoriais.Any(x => x.chave == editorial));
            }


            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                pagedList = pagedList.Where(n => n.titulo.Contains(searchParam) || n.chamada.Contains(searchParam) || n.TituloCapa.Contains(searchParam) || n.texto.Contains(searchParam));

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
                case "chamada":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.chamada);
                        ViewBag.SortClasschamada = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.chamada);
                        ViewBag.SortClasschamada = "sorting_desc";
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
                case "tipoDestaque":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.TipoDestaque);
                        ViewBag.SortClasstipoDestaque = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.TipoDestaque);
                        ViewBag.SortClasstipoDestaque = "sorting_desc";
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
                case "ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(n => n.liberado);
                        ViewBag.SortClassativo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(n => n.liberado);
                        ViewBag.SortClassativo = "sorting_desc";
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

        public ActionResult Create(int EditoriaId = 0)
        {
            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome");
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo");
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes, "Id", "Titulo");
            ViewBag.TiposDestaques = (from Enum item in Enum.GetValues(typeof(TipoDestaque))
                                      select new SelectListItem
                                      {
                                          Value = ((int)(object)item).ToString(),
                                          Text = item.GetDescription()
                                      }).ToList();

            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");

            var times = db.Times.Where(time => time.Ativo && !time.Excluido);
            var editoriais = db.Editoriais.Where(a => !a.excluido && a.ativo && a.esportes && a.id != 1 && !times.Any(time => time.EditoriaId == a.id));

            int selected = EditoriaId;

            if (EditoriaId == 0)
            {
                selected = editoriais.FirstOrDefault().id;
            }
            else
            {
                ViewBag.EditoriaEspecialNome = editoriais.FirstOrDefault(x => x.id == EditoriaId).nome;
            }

            ViewBag.Times = times.Select(time => new SelectListItem { Value = time.EditoriaId.ToString(), Text = time.Nome }).ToList();
            ViewBag.Campeonatos = editoriais.Select(editorial => new SelectListItem { Value = editorial.id.ToString(), Text = editorial.nome }).ToList();
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            return View();
        }

        //
        // POST: /EditorialNoticias/Create

        [HttpPost]
        public ActionResult Create(Noticias noticias, HttpPostedFileBase audio, HttpPostedFileBase foto, int[] EditoriaId, string ListaTags = "", string fotoExistente = "")
        {
            ModelState["dataAtualizacao"]?.Errors.Clear();
            if (!string.IsNullOrEmpty(fotoExistente))
            {
                noticias.foto = fotoExistente;
                ModelState.Remove("foto");
            }
            if (audio != null && !audio.IsExtension("mp3", "aac"))
            {
                ModelState.AddModelError("audio", "A extensão do áudio deve ser apenas nos formatos: .mp3 e .aac .");
            }
            if (foto == null && (noticias.TipoDestaque.Value == (int)TipoDestaque.Normal1 || noticias.TipoDestaque.Value == (int)TipoDestaque.Normal2 || noticias.TipoDestaque.Value == (int)TipoDestaque.Normal3 || noticias.TipoDestaque.Value == (int)TipoDestaque.Urgente1130 || noticias.TipoDestaque.Value == (int)TipoDestaque.Urgente360))
            {
                ModelState.AddModelError("TipoDestaque", "A notícia deve ter uma foto para poder ser colocada em destaque.");
            }
            else if (ModelState.IsValid)
            {

                noticias.dataCadastro = DateTime.Now;
                if(noticias.dataAtualizacao!=DateTime.MinValue || noticias.dataAtualizacao != null)
                {
                    noticias.dataAtualizacao = DateTime.Now;
                
                }
                noticias.transito = false;

                if (!string.IsNullOrEmpty(noticias.videoYoutube))
                {
                    if (noticias.videoYoutube.Contains("="))
                    {
                        string[] keyYoutube = noticias.videoYoutube.Split(new char[] { '=' });
                        noticias.videoYoutube = keyYoutube[1];
                    }
                }

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
                    #region uploads

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

                    #region editoriais
                    if (EditoriaId.Count() > 0)
                    {
                        foreach (var id in EditoriaId)
                        {
                            int idEditorial = Convert.ToInt32(id);
                            var editorial = db.Editoriais.FirstOrDefault(x => x.id == id);
                            if (editorial != null)
                            {
                                noticias.Editoriais.Add(editorial);
                            }
                        }
                    }
                    #endregion


                    db.Entry(noticias).State = EntityState.Modified;
                    db.SaveChanges();
                    #endregion
                }

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, noticias.id);
                return RedirectToAction("Index");
            }

            foreach (var item in EditoriaId)
            {
                int idEditorial = Convert.ToInt32(item);
                var editorial = db.Editoriais.FirstOrDefault(x => x.id == item);
                if (editorial != null)
                {
                    noticias.Editoriais.Add(editorial);
                }
            }

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.TiposDestaques = (from Enum item in Enum.GetValues(typeof(TipoDestaque))
                                      select new SelectListItem
                                      {
                                          Value = ((int)(object)item).ToString(),
                                          Text = item.GetDescription()
                                      }).ToList();

            var times = db.Times.Where(time => time.Ativo && !time.Excluido);
            var editoriais = db.Editoriais.Where(a => !a.excluido && a.ativo && a.esportes && a.id != 1 && !times.Any(time => time.EditoriaId == a.id));

            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();



            var tagsNoticia = noticias.Tags.Select(x => x.Titulo).ToArray();
            var editoriaIds = noticias.Editoriais.Select(editoria => editoria.id).ToArray();
            ViewBag.Times = times.Select(time => new SelectListItem { Value = time.EditoriaId.ToString(), Text = time.Nome, Selected = editoriaIds.Any(editoria => editoria == time.EditoriaId) }).ToList();
            ViewBag.Campeonatos = editoriais.Select(editorial => new SelectListItem { Value = editorial.id.ToString(), Text = editorial.nome, Selected = editoriaIds.Any(editoria => editoria == editorial.id) }).ToList();

            ViewBag.tagsNoticia = ListaTags;

            return View(noticias);
        }

        //
        // GET: /EditorialNoticias/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Noticias noticias = db.Noticias.Find(id);
            if (noticias == null)
            {
                return HttpNotFound();
            }

            if (!string.IsNullOrEmpty(noticias.videoYoutube))
            {
                noticias.videoYoutube = "https://www.youtube.com/watch?v=" + noticias.videoYoutube;
            }

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.TiposDestaques = (from Enum item in Enum.GetValues(typeof(TipoDestaque))
                                      select new SelectListItem
                                      {
                                          Value = ((int)(object)item).ToString(),
                                          Text = item.GetDescription()
                                      }).ToList();
            
            var times = db.Times.Where(time => time.Ativo && !time.Excluido);
            var editoriais = db.Editoriais.Where(a => !a.excluido && a.ativo && a.esportes && a.id != 1 && !times.Any(time => time.EditoriaId == a.id));

            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            var tagsNoticia = noticias.Tags.Select(x => x.Titulo).ToArray();
            var editoriaIds = noticias.Editoriais.Select(editoria => editoria.id).ToArray();
            ViewBag.Times = times.Select(time => new SelectListItem { Value = time.EditoriaId.ToString(), Text = time.Nome, Selected = editoriaIds.Any(editoria => editoria == time.EditoriaId) }).ToList();
            ViewBag.Campeonatos = editoriais.Select(editorial => new SelectListItem { Value = editorial.id.ToString(), Text = editorial.nome, Selected = editoriaIds.Any(editoria => editoria == editorial.id) }).ToList();

            ViewBag.tagsNoticia = string.Join(",", tagsNoticia);

            return View(noticias);
        }

        //
        // POST: /EditorialNoticias/Edit/5

            private void Log(string l)
        {
            var s = l;
        }

        [HttpPost]
        public ActionResult Edit(Noticias noticias, HttpPostedFileBase audioUpload, HttpPostedFileBase fotoUpload, int[] EditoriaId, string ListaTags = "", string fotoExistente = "")
        {
            
            if (!string.IsNullOrEmpty(fotoExistente))
            {
                noticias.foto = fotoExistente;
                ModelState.Remove("fotoUpload");
            }
            if (audioUpload != null && !audioUpload.IsExtension("mp3", "aac"))
            {
                ModelState.AddModelError("audio", "A extensão do áudio deve ser apenas nos formatos: .mp3 e .aac .");
            }
            if ((fotoUpload == null && string.IsNullOrWhiteSpace(noticias.foto)) && (noticias.TipoDestaque.Value == (int)TipoDestaque.Normal1 || noticias.TipoDestaque.Value == (int)TipoDestaque.Normal2 || noticias.TipoDestaque.Value == (int)TipoDestaque.Normal3 || noticias.TipoDestaque.Value == (int)TipoDestaque.Urgente1130 || noticias.TipoDestaque.Value == (int)TipoDestaque.Urgente360))
            {
                ModelState.AddModelError("TipoDestaque", "A notícia deve ter uma foto para poder ser colocada em destaque.");
            }
            else if (ModelState.IsValid)
            {

                if (!string.IsNullOrEmpty(noticias.videoYoutube))
                {
                    if (noticias.videoYoutube.Contains("="))
                    {
                        string[] keyYoutube = noticias.videoYoutube.Split(new char[] { '=' });
                        noticias.videoYoutube = keyYoutube[1];
                    }
                }

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

                var bdDataAtualizacao = db.Noticias.AsNoTracking().FirstOrDefault(noticia => noticia.id == noticias.id).dataAtualizacao;

                //noticias.dataAtualizacao = DateTime.Now;
                if (noticias.dataAtualizacao == null || bdDataAtualizacao == noticias.dataAtualizacao)
                {
                    noticias.dataAtualizacao = DateTime.Now;
                }

                db.Entry(noticias).State = EntityState.Modified;
                db.SaveChanges();
                #endregion


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
                if (EditoriaId.Count() > 0)
                {
                    db.Entry(noticias).Collection("Editoriais").Load();
                    noticias.Editoriais.Clear();

                    foreach (var id in EditoriaId)
                    {
                        int idEditorial = Convert.ToInt32(id);
                        var editorial = db.Editoriais.FirstOrDefault(x => x.id == id);
                        if (editorial != null)
                        {
                            noticias.Editoriais.Add(editorial);
                        }
                    }
                }
                #endregion
                db.Entry(noticias).State = EntityState.Modified;

                db.Database.Log = m => Log(m);
                db.SaveChanges();
                Site.Services.RedisService.FlushAll(noticias.url);
                Site.Services.RedisService.FlushAll(noticias.id.ToString());
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, noticias.id);
                return RedirectToAction("Index");
            }

            foreach (var item in EditoriaId)
            {
                int idEditorial = Convert.ToInt32(item);
                var editorial = db.Editoriais.FirstOrDefault(x => x.id == item);
                if (editorial != null)
                {
                    noticias.Editoriais.Add(editorial);
                }
            }

            ViewBag.CategoriaMapaId = new SelectList(db.CategoriasMapa.Where(a => !a.Excluido), "Id", "Titulo", noticias.CategoriaMapaId);
            ViewBag.idColunista = new SelectList(db.Colunista.Where(a => !a.excluido), "id", "nome", noticias.idColunista);
            ViewBag.idGaleria = new SelectList(db.Galeria.Where(a => !a.excluido && !a.Fixa).OrderByDescending(a => a.dataCadastro), "id", "titulo", noticias.idGaleria);
            ViewBag.RegiaoId = new SelectList(db.NoticiasRegioes.Where(a => !a.Excluido), "Id", "Titulo", noticias.RegiaoId);
            ViewBag.TiposDestaques = (from Enum item in Enum.GetValues(typeof(TipoDestaque))
                                      select new SelectListItem
                                      {
                                          Value = ((int)(object)item).ToString(),
                                          Text = item.GetDescription()
                                      }).ToList();

            var times = db.Times.Where(time => time.Ativo && !time.Excluido);
            var editoriais = db.Editoriais.Where(a => !a.excluido && a.ativo && a.esportes && a.id != 1 && !times.Any(time => time.EditoriaId == a.id));

            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();



            var tagsNoticia = noticias.Tags.Select(x => x.Titulo).ToArray();
            var editoriaIds = noticias.Editoriais.Select(editoria => editoria.id).ToArray();
            ViewBag.Times = times.Select(time => new SelectListItem { Value = time.EditoriaId.ToString(), Text = time.Nome, Selected = editoriaIds.Any(editoria => editoria == time.EditoriaId) }).ToList();
            ViewBag.Campeonatos = editoriais.Select(editorial => new SelectListItem { Value = editorial.id.ToString(), Text = editorial.nome, Selected = editoriaIds.Any(editoria => editoria == editorial.id) }).ToList();

            ViewBag.tagsNoticia = ListaTags;

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

        [HttpPost]
        public ActionResult GetCategorias(int editorialId = 0)
        {
            var categorias = db.Categorias.Where(a => !a.Excluido && a.EditoriaId == editorialId);
            ViewBag.CategoriaId = new SelectList(categorias, "Id", "Titulo");
            return View();
        }

    }
}
