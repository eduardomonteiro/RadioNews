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
using WebMatrix.WebData;
using Administrativo.Enums;
using Administrativo.Helpers;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Bastidores")]
    public class BastidorController : BaseController
    {
        public int areaADM = 4;
        public int areaADM2 = 24;

        #region bastidores

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.GaleriaActive = "active";
            ViewBag.GaleriaSubActive = "active";
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
            ViewBag.SortClasschamada = "sorting";
            ViewBag.SortClasstexto = "sorting";
            ViewBag.SortClassexcluido = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Bastidores> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.Bastidores.Where(x => !x.excluido && x.id>1);

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
                    pagedList = pagedList.Where(g => g.titulo.ToLower().Contains(searchParam.ToLower()) || g.texto.ToLower().Contains(searchParam.ToLower()) || g.chamada.ToLower().Contains(searchParam.ToLower()));
                };



            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.titulo);
                        ViewBag.SortClasstitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.titulo);
                        ViewBag.SortClasstitulo = "sorting_desc";
                    }
                    break;
                case "chamada":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.chamada);
                        ViewBag.SortClasschamada = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.chamada);
                        ViewBag.SortClasschamada = "sorting_desc";
                    }
                    break;
                case "texto":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.texto);
                        ViewBag.SortClasstexto = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.texto);
                        ViewBag.SortClasstexto = "sorting_desc";
                    }
                    break;
                case "excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.excluido);
                        ViewBag.SortClassexcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.excluido);
                        ViewBag.SortClassexcluido = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(g => g.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Bastidores>(pageNumber, 10);
        }



        public ActionResult Details(int id = 0)
        {
            Bastidores galeria = db.Bastidores.Find(id);
            if (galeria == null)
            {
                return HttpNotFound();
            }
            ViewBag.fotos = galeria.BastidoresMidias.Where(f => !f.excluido);
            return View(galeria);
        }


        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(Bastidores galeria)
        {
            if (ModelState.IsValid)
            {
                galeria.excluido = false;
                galeria.dataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {
                    galeria.chave = galeria.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == galeria.chave).Count() > 0);

                db.Bastidores.Add(galeria);
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, galeria.id);

                return RedirectToAction("Edit", new { id = galeria.id });
            }

            return View(galeria);
        }
        
        public ActionResult Edit(int id = 0)
        {
            Bastidores galeria = db.Bastidores.Find(id);
            if (galeria == null)
            {
                return HttpNotFound();
            }

            ViewBag.fotos = galeria.BastidoresMidias.Where(f => !f.excluido && f.video == false);
            ViewBag.videos = galeria.BastidoresMidias.Where(f => !f.excluido && f.video == true);

            return View(galeria);
        }

        [HttpPost]
        public ActionResult Edit(Bastidores galeria)
        {

            if (ModelState.IsValid)
            {
                int suffix = 0;

                do
                {
                    galeria.chave = galeria.titulo.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Bastidores.Where(o => o.chave == galeria.chave).Count() > 0);

                db.Entry(galeria).State = EntityState.Modified;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, galeria.id);

                return RedirectToAction("Index");
            }
            return View(galeria);
        }


        public ActionResult Delete(int id = 0)
        {
            Bastidores galeria = db.Bastidores.Find(id);

            if (galeria == null)
            {
                return HttpNotFound();
            }
            return View(galeria);
        }

        //
        // POST: /Galeria/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Bastidores galeria = db.Bastidores.Find(id);

            galeria.excluido = true;
            db.Entry(galeria).State = EntityState.Modified;

            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, galeria.id);

            return RedirectToAction("Index");
        }
        #endregion



        #region videos

        public IPagedList<BastidoresMidias> GetListItensVideos(int? page, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.BastidoresMidias.Where(x => !x.excluido && x.video == true);

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
                    pagedList = pagedList.Where(g => g.legenda.ToLower().Contains(searchParam.ToLower()));
                };


            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.legenda);
                        ViewBag.SortClasstitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.legenda);
                        ViewBag.SortClasstitulo = "sorting_desc";
                    }
                    break;
                case "excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.excluido);
                        ViewBag.SortClassexcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.excluido);
                        ViewBag.SortClassexcluido = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(g => g.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(g => g.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(g => g.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList(pageNumber, 10);

        }

        public ActionResult Videos(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.GaleriaActive = "active";
            ViewBag.GaleriaSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItensVideos(page, search, sortField, order));

        }

        public ActionResult VideosCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VideosCreate(BastidoresMidias dados)
        {
            if (ModelState.IsValid)
            {
                if (dados.midia.Contains("="))
                {
                    string[] keyYoutube = dados.midia.Split(new char[] { '=' });
                    dados.midia = keyYoutube[1];
                }

                dados.excluido = false;
                dados.video = true;
                dados.dataCadastro = DateTime.Now;

                db.BastidoresMidias.Add(dados);
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Insercao, dados.id);

                return RedirectToAction("VideosDetails", new { id = dados.id });

            }
            return View(dados);
        }

        public ActionResult VideosDetails(int id)
        {
            BastidoresMidias video = db.BastidoresMidias.Find(id);
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);

        }


        public ActionResult VideosEdit(int id)
        {
            BastidoresMidias video = db.BastidoresMidias.Find(id);
            video.midia = "https://www.youtube.com/watch?v=" + video.midia;
            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult VideosEdit(BastidoresMidias dados)
        {
            if (ModelState.IsValid)
            {
                if (dados.midia.Contains("="))
                {
                    string[] keyYoutube = dados.midia.Split(new char[] { '=' });
                    dados.midia = keyYoutube[1];
                }

                dados.dataCadastro = DateTime.Now;
                db.Entry(dados).State = EntityState.Modified;
                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Edicao, dados.id);
                return RedirectToAction("Videos");
            }

            return View(dados);
        }

        [HttpGet,ActionName("VideosDelete")]
        public ActionResult VideosDelete(int id)
        {
            BastidoresMidias video = db.BastidoresMidias.Find(id);

            if (video == null)
            {
                return HttpNotFound();
            }
            return View(video);
        }

        //
        // POST: /Galeria/Delete/5

        [HttpPost,ActionName("VideosDeleteConfirmed")]
        public ActionResult VideosDeleteConfirmed(int id)
        {
            BastidoresMidias video = db.BastidoresMidias.Find(id);
            video.excluido = true;
            db.Entry(video).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Exclusao, video.id);
            return RedirectToAction("Videos");
        }


        #endregion


    }

}




