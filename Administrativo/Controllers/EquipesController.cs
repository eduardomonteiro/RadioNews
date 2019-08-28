using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Administrativo.Models;
using System.IO;
using Administrativo.Commons;
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Equipes")]
    public class EquipesController : BaseController
    {
        private int Width = 234;
        private int Height = 120;

        public int areaADM = 10;

        //
        // GET: /Equipes/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.EquipeActive = "active";
            ViewBag.EquipeSubActive = "active";
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));

        }

        public ActionResult ordenacao()
        {

            return View(db.Equipe.Where(a => !a.excluido).OrderBy(a => a.ordem).ToList());

        }

        [HttpPost]
        public bool ReOrder(int[] ids)
        {
            if (ids != null)
            {

                foreach (var id in ids.Select((value, i) => new { i, value }))
                {
                    Equipe equipe = db.Equipe.Find(id.value);
                    equipe.ordem = id.i;
                    db.Entry(equipe).State = EntityState.Modified;

                }
                db.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }


        public void allClassOffSort()
        {

            ViewBag.SortClassid = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClasstexto = "sorting";
            ViewBag.SortClassfuncao = "sorting";
            ViewBag.SortClassimagem = "sorting";
            ViewBag.SortClassinstagram = "sorting";
            ViewBag.SortClassfacebook = "sorting";
            ViewBag.SortClasstwitter = "sorting";
            ViewBag.SortClassexcluido = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<Equipe> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Equipe.Where(a => !a.excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(e => e.id == id || e.nome.ToLower().Contains(searchParam.ToLower()));

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "texto":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.texto);
                        ViewBag.SortClasstexto = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.texto);
                        ViewBag.SortClasstexto = "sorting_desc";
                    }
                    break;
                case "funcao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.funcao);
                        ViewBag.SortClassfuncao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.funcao);
                        ViewBag.SortClassfuncao = "sorting_desc";
                    }
                    break;
                case "imagem":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.imagem);
                        ViewBag.SortClassimagem = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.imagem);
                        ViewBag.SortClassimagem = "sorting_desc";
                    }
                    break;
                case "instagram":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.instagram);
                        ViewBag.SortClassinstagram = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.instagram);
                        ViewBag.SortClassinstagram = "sorting_desc";
                    }
                    break;
                case "facebook":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.facebook);
                        ViewBag.SortClassfacebook = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.facebook);
                        ViewBag.SortClassfacebook = "sorting_desc";
                    }
                    break;
                case "twitter":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.twitter);
                        ViewBag.SortClasstwitter = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.twitter);
                        ViewBag.SortClasstwitter = "sorting_desc";
                    }
                    break;
                case "excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.excluido);
                        ViewBag.SortClassexcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.excluido);
                        ViewBag.SortClassexcluido = "sorting_desc";
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
                    pagedList = pagedList.OrderBy(e => e.nome);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Equipe>(pageNumber, 10);
        }

        //
        // GET: /Equipes/Details/5

        public ActionResult Details(int id = 0)
        {
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        //
        // GET: /Equipes/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Equipes/Create

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Equipe equipe, HttpPostedFileBase Arquivo)
        {
            if (ModelState.IsValid)
            {
                db.Equipe.Add(equipe);

                equipe.DataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {
                    equipe.chave = equipe.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == equipe.chave).Count() > 0);

                db.SaveChanges();

                string caminho = Server.MapPath("~/conteudo/equipe/165x165/");
                equipe.imagem = Utils.SaveAndCropImage(Arquivo, caminho, 0, 0, 165, 165);

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, equipe.id);
                return RedirectToAction("Index");
            }

            return View(equipe);
        }

        //
        // GET: /Equipes/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        //
        // POST: /Equipes/Edit/5

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Equipe equipe, HttpPostedFileBase Arquivo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(equipe).State = EntityState.Modified;
                db.SaveChanges();

                if (Arquivo != null)
                {
                    string caminho = Server.MapPath("~/conteudo/equipe/165x165/");
                    equipe.imagem = Utils.SaveAndCropImage(Arquivo, caminho, 0, 0, 165, 165);
                }

                int suffix = 0;

                do
                {
                    equipe.chave = equipe.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Materia.Where(o => o.chave == equipe.chave).Count() > 0);

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, equipe.id);
                return RedirectToAction("Index");
            }
            return View(equipe);
        }

        //
        // GET: /Equipes/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Equipe equipe = db.Equipe.Find(id);
            if (equipe == null)
            {
                return HttpNotFound();
            }
            return View(equipe);
        }

        //
        // POST: /Equipes/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Equipe equipe = db.Equipe.Find(id);
            //db.Equipe.Remove(equipe);
            equipe.excluido = true;
            db.Entry(equipe).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, equipe.id);
            return RedirectToAction("Index");
        }


        private string SalvarImagem(HttpPostedFileBase Arquivo, string VirtualPathTemp, Equipe equipes)
        {
            var physicalTemp = Server.MapPath(VirtualPathTemp);

            string filename = Arquivo.FileName;

            string ext = System.IO.Path.GetExtension(Arquivo.FileName);

            string imageFinalName = equipes.id + ext;

            if (!Directory.Exists(physicalTemp))
            {
                Directory.CreateDirectory(physicalTemp);
            }
            Request.Files[0].SaveAs(physicalTemp + imageFinalName);

            var physicalfinal = HttpContext.Server.MapPath(VirtualPathTemp);

            if (!Directory.Exists(physicalfinal))
            {
                Directory.CreateDirectory(physicalfinal);
            }

            AMImageLIB.Imagem amlib = new AMImageLIB.Imagem(physicalTemp + imageFinalName);
            amlib = new AMImageLIB.Imagem(physicalTemp + imageFinalName);
            amlib.qualidade = AMImageLIB.Imagem.Apoio.qualidade.alta;
            amlib.redimensionar(Width, Height);
            amlib.salvar(physicalfinal + "\\" + imageFinalName);
            return imageFinalName;
        }
    }
}




