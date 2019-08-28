using Administrativo.Commons;
using Administrativo.Enums;
using Administrativo.Helpers;
using Administrativo.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebMatrix.WebData;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador")]
    public class GruposPushController : BaseController
    {
        int areaADM = 42;
        //
        // GET: /GruposPush/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
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
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassChave = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }
        public IPagedList<GruposPush> GetListItens(int? page, string search = "", string sortField = "", string order = "")
        {
            var list = db.GruposPush.Where(g => !g.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                int outDate = 0;
                int id = int.TryParse(search, out outDate) ? int.Parse(search) : 0;

                pagedList = pagedList.Where(e => e.Id == id);

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
                case "Title":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Title);
                        ViewBag.SortClassTitle = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Title);
                        ViewBag.SortClassTitle = "sorting_desc";
                    }
                    break;
                case "Liberado":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Liberado);
                        ViewBag.SortClassLiberado = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Liberado);
                        ViewBag.SortClassLiberado = "sorting_desc";
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

            return pagedList.ToPagedList<GruposPush>(pageNumber, 10);
        }
        public ActionResult Details(int id = 0)
        {
            GruposPush grupo = db.GruposPush.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }
        public ActionResult Create()
        {
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            return View();
        }
        [HttpPost]
        public ActionResult Create(GruposPush grupo, string ListaTags = "")
        {
            if (ModelState.IsValid)
            {

                #region Tags
                if (!string.IsNullOrEmpty(ListaTags))
                {
                    grupo.Tags = new List<Tags>();
                    string[] tags = ListaTags.Split(new char[] { ',' });

                    foreach (var item in tags)
                    {
                        var hasTag = db.Tags.FirstOrDefault(x => x.Titulo.ToLower() == item.ToLower());

                        if (hasTag != null)
                        {
                            grupo.Tags.Add(hasTag);
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

                            grupo.Tags.Add(obj);
                        }

                    }


                }
                #endregion

                grupo.Excluido = false;
                grupo.DataCadastro = DateTime.Now;

                db.GruposPush.Add(grupo);
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, grupo.Id);
                return RedirectToAction("Index");
            }

            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            return View(grupo);
        }
        public ActionResult Edit(int id = 0)
        {
            GruposPush grupo = db.GruposPush.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            var tags = grupo.Tags.Select(x => x.Titulo).ToArray();
            ViewBag.tagsNoticia = string.Join(",", tags);

            return View(grupo);
        }
        [HttpPost]
        public ActionResult Edit(GruposPush grupo, string ListaTags = "")
        {
            if (ModelState.IsValid)
            {

                #region Tags
                db.Entry(grupo).State = EntityState.Modified;

                db.Entry(grupo).Collection("Tags").Load();
                grupo.Tags.Clear();

                if (!string.IsNullOrEmpty(ListaTags))
                {
                    grupo.Tags = new List<Tags>();
                    string[] tagList = ListaTags.Split(new char[] { ',' });

                    foreach (var item in tagList)
                    {
                        var hasTag = db.Tags.FirstOrDefault(x => x.Titulo.ToLower() == item.ToLower());

                        if (hasTag != null)
                        {
                            grupo.Tags.Add(hasTag);
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

                            grupo.Tags.Add(obj);
                        }

                    }


                }
                #endregion

                grupo.DataCadastro = DateTime.Now;
                grupo.Excluido = false;

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, grupo.Id);
                return RedirectToAction("Index");
            }

            ViewBag.MultiSelectTags = new MultiSelectList(db.Tags.Where(a => !a.Excluido), "Id", "Titulo");
            ViewBag.AutoCompleteTags = db.Tags.Where(a => !a.Excluido).Select(x => x.Titulo).ToArray();

            var tags = grupo.Tags.Select(x => x.Titulo).ToArray();
            ViewBag.tagsNoticia = string.Join(",", tags);

            return View(grupo);
        }
        public ActionResult Delete(int id = 0)
        {
            GruposPush grupo = db.GruposPush.Find(id);
            if (grupo == null)
            {
                return HttpNotFound();
            }
            return View(grupo);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            GruposPush grupo = db.GruposPush.Find(id);

            grupo.Excluido = true;
            db.Entry(grupo).State = EntityState.Modified;


            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, grupo.Id);
            return RedirectToAction("Index");
        }
    }
}
