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
using Administrativo.Enums;
using WebMatrix.WebData;
using Administrativo.Helpers;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Denuncia dos Ouvintes")]
    public class OuvintesController : BaseController
    {
        public int areaId = 11;
        //
        // GET: /Ouvintes/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.OuvintesActive = "active";
            ViewBag.OuvintesSubActive = "active";
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
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassemail = "sorting";
            ViewBag.SortClassassunto = "sorting";
            ViewBag.SortClassdata = "sorting";
            ViewBag.SortClassarquivo = "sorting";
            ViewBag.SortClasstipoArquivo = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<Ouvintes> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {
            var list = db.Ouvintes.Where(x => !x.excluido);

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
                    pagedList = pagedList.Where(g => g.nome.ToLower().Contains(searchParam.ToLower()) || g.email.ToLower().Contains(searchParam.ToLower()) || g.assunto.ToLower().Contains(searchParam.ToLower()));
                };

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "email":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.email);
                        ViewBag.SortClassemail = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.email);
                        ViewBag.SortClassemail = "sorting_desc";
                    }
                    break;
                case "assunto":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.assunto);
                        ViewBag.SortClassassunto = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.assunto);
                        ViewBag.SortClassassunto = "sorting_desc";
                    }
                    break;
                case "data":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.data);
                        ViewBag.SortClassdata = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.data);
                        ViewBag.SortClassdata = "sorting_desc";
                    }
                    break;

                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(o => o.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(o => o.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(o => o.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Ouvintes>(pageNumber, 10);
        }

        //
        // GET: /Ouvintes/Details/5

        public ActionResult Details(int id = 0)
        {
            Ouvintes ouvintes = db.Ouvintes.Find(id);
            if (ouvintes == null)
            {
                return HttpNotFound();
            }

            return View(ouvintes);
        }

        public JsonResult AtivarDesativar(int id = 0)
        {
            Ouvintes ouvintes = db.Ouvintes.Find(id);

            if (ouvintes == null)
            {
                return Json(false,JsonRequestBehavior.AllowGet);
            }
            else
            {
                ouvintes.liberado = (ouvintes.liberado ? false : true);
                db.Entry(ouvintes).State = EntityState.Modified;

                db.Entry(ouvintes).Property(b => b.regiaoId).IsModified = false;
                db.Entry(ouvintes).Property(b => b.data).IsModified = false;
                db.Entry(ouvintes).Property(b => b.DataCadastro).IsModified = false;
                db.Entry(ouvintes).Property(b => b.assunto).IsModified = false;

                db.SaveChanges();

                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }


        //
        // GET: /Ouvintes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Ouvintes ouvintes = db.Ouvintes.Find(id);
            if (ouvintes == null)
            {
                return HttpNotFound();
            }
            return View(ouvintes);
        }

        //
        // POST: /Ouvintes/Edit/5

        [HttpPost]
        public ActionResult Edit(Ouvintes ouvintes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ouvintes).State = EntityState.Modified;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaId, TipoAcesso.Edicao, ouvintes.id);

                return RedirectToAction("Index");
            }
            return View(ouvintes);
        }

        //
        // GET: /Ouvintes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Ouvintes ouvintes = db.Ouvintes.Find(id);
            if (ouvintes == null)
            {
                return HttpNotFound();
            }
            return View(ouvintes);
        }

        //
        // POST: /Ouvintes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Ouvintes ouvintes = db.Ouvintes.Find(id);

            ouvintes.excluido = true;
            db.Entry(ouvintes).State = EntityState.Modified;

            db.Entry(ouvintes).Property(b => b.regiaoId).IsModified = false;
            db.Entry(ouvintes).Property(b => b.data).IsModified = false;
            db.Entry(ouvintes).Property(b => b.DataCadastro).IsModified = false;
            db.Entry(ouvintes).Property(b => b.assunto).IsModified = false;

            //db.Ouvintes.Remove(ouvintes);
            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaId, TipoAcesso.Exclusao, ouvintes.id);
            return RedirectToAction("Index");
        }


        public FileResult Download(int id)
        {
            var arquivo = db.OuvintesArquivos.Find(id);
            var path = "/admin/conteudo/ouvintes/arquivos/";
            var path2 = Server.MapPath(path + arquivo.Arquivo);
            if (System.IO.File.Exists(path2))
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(path2);
                string fileName = arquivo.Arquivo;
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            else
            {
                return null;
            }
        }


    }
}




