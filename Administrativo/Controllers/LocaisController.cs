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
    public class LocaisController : BaseController
    {
        public int areaADM = 35;
        protected const string pathFoto405x270 = "~/Conteudo/locais/{0}/405x270/";
        protected const string pathFoto365x240 = "~/Conteudo/locais/{0}/365x240/";
        protected const string pathFoto260x173 = "~/Conteudo/locais/{0}/260x173/";

        protected const string pathOriginal = "~/Conteudo/locais/{0}/original/";
        //
        // GET: /Locais/
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            ViewBag.pathOriginal = pathFoto405x270;
        }
        //
        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "", int EspecialId = 0, int SecaoId = 0)
        {
            ViewBag.Secoes_LocaisActive = "active";
            ViewBag.Secoes_LocaisSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;

            ViewBag.EspecialId = EspecialId;
            ViewBag.SecaoId = SecaoId;

            allClassOffSort();

            return View(GetListItens(page, search, sortField, order, EspecialId, SecaoId));
        }


        public void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassSecaoId = "sorting";
            ViewBag.SortClassTipo = "sorting";
            ViewBag.SortClassTitulo = "sorting";
            ViewBag.SortClassDescricao = "sorting";
            ViewBag.SortClassEndereco = "sorting";
            ViewBag.SortClassLatitude = "sorting";
            ViewBag.SortClassLongitude = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassExcluido = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public IPagedList<Secoes_Locais> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "", int EditorialId = 0, int SecaoId = 0)
        {
            var list = db.Secoes_Locais.Where(x => !x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (EditorialId > 0)
            {
                pagedList = pagedList.Where(x => x.Especiais_Secoes.EditoriaId == EditorialId);
            }

            if (SecaoId > 0)
            {
                pagedList = pagedList.Where(x => x.SecaoId == SecaoId);
            }


            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(s => s.Id == id || s.Titulo.Contains(searchParam) || s.Tipo.Contains(searchParam) || s.Endereco.Contains(searchParam));

            }


            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;
                case "SecaoId":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.SecaoId);
                        ViewBag.SortClassSecaoId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.SecaoId);
                        ViewBag.SortClassSecaoId = "sorting_desc";
                    }
                    break;
                case "Tipo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Tipo);
                        ViewBag.SortClassTipo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Tipo);
                        ViewBag.SortClassTipo = "sorting_desc";
                    }
                    break;
                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Titulo);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Titulo);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;
                case "Descricao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Descricao);
                        ViewBag.SortClassDescricao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Descricao);
                        ViewBag.SortClassDescricao = "sorting_desc";
                    }
                    break;
                case "Endereco":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Endereco);
                        ViewBag.SortClassEndereco = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Endereco);
                        ViewBag.SortClassEndereco = "sorting_desc";
                    }
                    break;
                case "Latitude":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Latitude);
                        ViewBag.SortClassLatitude = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Latitude);
                        ViewBag.SortClassLatitude = "sorting_desc";
                    }
                    break;
                case "Longitude":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Longitude);
                        ViewBag.SortClassLongitude = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Longitude);
                        ViewBag.SortClassLongitude = "sorting_desc";
                    }
                    break;
                case "Ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Ativo);
                        ViewBag.SortClassAtivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Ativo);
                        ViewBag.SortClassAtivo = "sorting_desc";
                    }
                    break;
                case "Excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.Excluido);
                        ViewBag.SortClassExcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.Excluido);
                        ViewBag.SortClassExcluido = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(s => s.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(s => s.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(s => s.Id);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Secoes_Locais>(pageNumber, 10);
        }

        //
        // GET: /Locais/Details/5

        public ActionResult Details(int id = 0)
        {
            Secoes_Locais secoes_locais = db.Secoes_Locais.Find(id);
            if (secoes_locais == null)
            {
                return HttpNotFound();
            }
            return View(secoes_locais);
        }

        //
        // GET: /Locais/Create

        public ActionResult Create(int EspecialId = 0)
        {
            if (EspecialId > 0)
            {
                var especial = db.Editoriais.FirstOrDefault(x => x.id == EspecialId);
                ViewBag.EspecialNome = especial.nome;
                ViewBag.EspecialId = especial.id;
            }


            ViewBag.SecaoId = new SelectList(db.Especiais_Secoes.Where(x => x.EditoriaId == EspecialId && !x.Excluido ), "Id", "Titulo");
            return View();
        }

        //
        // POST: /Locais/Create

        [HttpPost]
        public ActionResult Create(Secoes_Locais local, HttpPostedFileBase Imagem, string fotoExistente = "")
        {
            if (ModelState.IsValid)
            {
                local.DataCadastro = DateTime.Now;
                local.Excluido = false;

                db.Secoes_Locais.Add(local);

         
                 var editoriaId = db.Editoriais.FirstOrDefault(x => x.Especiais_Secoes.Any(w => w.Id == local.SecaoId)).id;

                if (db.SaveChanges() > 0)
                {

                    if (Imagem != null)
                    {

                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, local.Id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, local.Id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, local.Id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, local.Id));

                        local.Imagem = Utils.SaveFileBase(fileOriginal, Imagem);


                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 260, 173, path260x173);


                    }

                    int suffix = 0;

                    do
                    {
                        local.Chave = local.Titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Secoes_Locais.Where(o => o.Chave == local.Chave).Count() > 0);


                    if (!string.IsNullOrEmpty(fotoExistente))
                    {
                        local.Imagem = fotoExistente;
                    }

                    db.Entry(local).State = EntityState.Modified;
                    db.SaveChanges();

                }


                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, local.Id);
                return RedirectToAction("Index", new { EspecialId = editoriaId });
            }
            var especialId = db.Editoriais.FirstOrDefault(x => x.Especiais_Secoes.Any(w => w.Id == local.SecaoId)).id;

            ViewBag.SecaoId = new SelectList(db.Especiais_Secoes.Where(x=>x.EditoriaId == especialId && !x.Excluido), "Id", "Titulo", local.SecaoId);
            return View(local);
        }

        //
        // GET: /Locais/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Secoes_Locais secoes_locais = db.Secoes_Locais.Find(id);
            if (secoes_locais == null)
            {
                return HttpNotFound();
            }

            ViewBag.EspecialNome = secoes_locais.Especiais_Secoes.Editoriais.nome;
            ViewBag.EspecialId = secoes_locais.Especiais_Secoes.EditoriaId;

            ViewBag.SecaoId = new SelectList(db.Especiais_Secoes.Where(x=>x.EditoriaId == secoes_locais.Especiais_Secoes.EditoriaId ), "Id", "Titulo", secoes_locais.SecaoId);
            return View(secoes_locais);
        }

        //
        // POST: /Locais/Edit/5

        [HttpPost]
        public ActionResult Edit(Secoes_Locais local, HttpPostedFileBase ImagemUpload, string fotoExistente = "")
        {
            if (ModelState.IsValid)
            {
                local.Excluido = false;
                db.Entry(local).State = EntityState.Modified;
                local.DataCadastro = DateTime.Now;

                if (db.SaveChanges() > 0)
                {

                    if (ImagemUpload != null)
                    {

                        var path405x270 = Server.MapPath(string.Format(pathFoto405x270, local.Id));
                        var path365x240 = Server.MapPath(string.Format(pathFoto365x240, local.Id));
                        var path260x173 = Server.MapPath(string.Format(pathFoto260x173, local.Id));

                        var fileOriginal = Server.MapPath(string.Format(pathOriginal, local.Id));

                        local.Imagem = Utils.SaveFileBase(fileOriginal, ImagemUpload);


                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 405, 270, path405x270);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 365, 240, path365x240);
                        Utils.resizeImageAndSave3(Path.Combine(fileOriginal, local.Imagem), 260, 173, path260x173);


                    }

                    int suffix = 0;

                    do
                    {
                        local.Chave = local.Titulo.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                        suffix++;
                    } while (db.Secoes_Locais.Where(o => o.Chave == local.Chave).Count() > 0);


                    if (!string.IsNullOrEmpty(fotoExistente))
                    {
                        local.Imagem = fotoExistente;
                    }
                    local.DataCadastro = DateTime.Now;
                    db.Entry(local).State = EntityState.Modified;
                    db.SaveChanges();

                }

                var editoriaId = db.Editoriais.FirstOrDefault(x => x.Especiais_Secoes.Any(w => w.Id == local.SecaoId)).id;
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, local.Id);
                return RedirectToAction("Index", new { EspecialId = editoriaId });
            }
            ViewBag.SecaoId = new SelectList(db.Especiais_Secoes.Where(x=>x.EditoriaId == local.Especiais_Secoes.EditoriaId), "Id", "Titulo", local.SecaoId);
            return View(local);
        }

        //
        // GET: /Locais/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Secoes_Locais secoes_locais = db.Secoes_Locais.Find(id);
            if (secoes_locais == null)
            {
                return HttpNotFound();
            }
            return View(secoes_locais);
        }

        //
        // POST: /Locais/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Secoes_Locais secoes_locais = db.Secoes_Locais.Find(id);
            //db.Secoes_Locais.Remove(secoes_locais);
            secoes_locais.Excluido = true;
            db.Entry(secoes_locais).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, secoes_locais.Id);
            return RedirectToAction("Index");
        }

    }
}




