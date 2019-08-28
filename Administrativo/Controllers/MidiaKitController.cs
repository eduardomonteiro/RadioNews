using System;
using System.Data;
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
using System.Data.Entity;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,MidiaKit")]
    public class MidiaKitController : BaseController
    {
        public int areaADM = 17;
        //private RadioCompanyNameContext db = new RadioCompanyNameContext();

        //
        // GET: /MidiaKit/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.MidiaKitActive = "active";
            ViewBag.MidiaKitSubActive = "active";
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
            ViewBag.SortClassArquivo = "sorting";
            ViewBag.SortClassMiniatura = "sorting";
            ViewBag.SortClassAtivo = "sorting";
            ViewBag.SortClassExcluido = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";

        }

        public IPagedList<MidiaKit> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.MidiaKit.Where(x => !x.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(m => m.Id == id || m.Titulo.Contains(searchParam) || m.Arquivo.Contains(searchParam));

            }


            switch (sortField)
            {
                case "Id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Id);
                        ViewBag.SortClassId = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Id);
                        ViewBag.SortClassId = "sorting_desc";
                    }
                    break;
                case "Titulo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Titulo);
                        ViewBag.SortClassTitulo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Titulo);
                        ViewBag.SortClassTitulo = "sorting_desc";
                    }
                    break;
                case "Arquivo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Arquivo);
                        ViewBag.SortClassArquivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Arquivo);
                        ViewBag.SortClassArquivo = "sorting_desc";
                    }
                    break;
                case "Miniatura":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Miniatura);
                        ViewBag.SortClassMiniatura = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Miniatura);
                        ViewBag.SortClassMiniatura = "sorting_desc";
                    }
                    break;
                case "Ativo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Ativo);
                        ViewBag.SortClassAtivo = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Ativo);
                        ViewBag.SortClassAtivo = "sorting_desc";
                    }
                    break;
                case "Excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Excluido);
                        ViewBag.SortClassExcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Excluido);
                        ViewBag.SortClassExcluido = "sorting_desc";
                    }
                    break;
                case "DataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.DataCadastro);
                        ViewBag.SortClassDataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(m => m.Id);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<MidiaKit>(pageNumber, 10);
        }

        //
        // GET: /MidiaKit/Details/5

        public ActionResult Details(int id = 0)
        {
            MidiaKit midiakit = db.MidiaKit.Find(id);
            if (midiakit == null)
            {
                return HttpNotFound();
            }
            return View(midiakit);
        }

        //
        // GET: /MidiaKit/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MidiaKit/Create

        [HttpPost]
        public ActionResult Create(MidiaKit midiakit, HttpPostedFileBase Arquivo, HttpPostedFileBase Miniatura)
        {
            if (ModelState.IsValid)
            {
                midiakit.Excluido = false;
                midiakit.DataCadastro = DateTime.Now;

                db.MidiaKit.Add(midiakit);
                db.SaveChanges();

                if (Arquivo != null)
                {
                    string caminho = Server.MapPath("~/conteudo/MidiaKit/");

                    var extension = Path.GetExtension(Arquivo.FileName);

                    string[] permitidos = { ".jpg", ".jpeg", ".gif", ".js", ".zip", ".png", ".rar", ".doc", ".docx", ".txt", ".mp3 ", ".pdf", ".mpeg", ".mp4", ".ogg" };

                    if (!permitidos.Contains(extension.ToLower()))
                    {
                        ModelState.AddModelError("", "Extensão não permitida!");
                    }
                    else
                    {
                        Arquivo.SaveAs(caminho + Arquivo.FileName);
                        midiakit.Arquivo = Arquivo.FileName;
                    }

                }


                if (Miniatura != null)
                {
                    var extension = Path.GetExtension(Arquivo.FileName);

                    string[] permitidos = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (!permitidos.Contains(extension.ToLower()))
                    {
                        ModelState.AddModelError("", "Extensão não permitida!");
                    }
                    else
                    {
                        var pathArquivoFinal = Server.MapPath("~/conteudo/MidiaKit/miniaturas/");

                        Miniatura.SaveAs(Path.Combine(pathArquivoFinal, Miniatura.FileName));
                        midiakit.Miniatura = Miniatura.FileName;
                    }

                }

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, midiakit.Id);
                return RedirectToAction("Index");
            }

            return View(midiakit);
        }

        //
        // GET: /MidiaKit/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MidiaKit midiakit = db.MidiaKit.Find(id);
            if (midiakit == null)
            {
                return HttpNotFound();
            }
            return View(midiakit);
        }

        //
        // POST: /MidiaKit/Edit/5

        [HttpPost]
        public ActionResult Edit(MidiaKit midiakit, HttpPostedFileBase ArquivoF, HttpPostedFileBase MiniaturaF)
        {
            if (ModelState.IsValid)
            {
                midiakit.DataCadastro = DateTime.Now;
                db.Entry(midiakit).State = EntityState.Modified;
                db.SaveChanges();

                if (ArquivoF != null && ArquivoF.ContentLength > 0)
                {
                    var extension = Path.GetExtension(ArquivoF.FileName);

                    string[] permitidos = { ".jpg", ".jpeg", ".gif", ".js", ".zip", ".png", ".rar", ".doc", ".docx", ".txt", ".mp3 ", ".pdf", ".mpeg", ".mp4", ".ogg" };

                    if (!permitidos.Contains(extension.ToLower()))
                    {
                        ModelState.AddModelError("", "Extensão não permitida!");
                    }
                    else
                    {
                        string caminho = Server.MapPath("~/conteudo/MidiaKit/");
                        ArquivoF.SaveAs(caminho + ArquivoF.FileName);
                        midiakit.Arquivo = ArquivoF.FileName;

                    }
                }

                if (MiniaturaF != null && MiniaturaF.ContentLength > 0)
                {
                    var extension = Path.GetExtension(MiniaturaF.FileName);

                    string[] permitidos = { ".jpg", ".jpeg", ".gif", ".png" };

                    if (!permitidos.Contains(extension.ToLower()))
                    {
                        ModelState.AddModelError("", "Extensão não permitida!");
                    }
                    else
                    {
                        var Temp = Server.MapPath("~/conteudo/Temp/");
                        var pathArquivoFinal = Server.MapPath("~/conteudo/MidiaKit/miniaturas/");

                        MiniaturaF.SaveAs(System.IO.Path.Combine(Temp, MiniaturaF.FileName));
                        midiakit.Miniatura = Utils.resizeImageAndSave(System.IO.Path.Combine(Temp, MiniaturaF.FileName), 300, 300, pathArquivoFinal);

                    }
                }

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, midiakit.Id);
                return RedirectToAction("Index");
            }
            return View(midiakit);
        }

        //
        // GET: /MidiaKit/Delete/5

        public ActionResult Delete(int id = 0)
        {
            MidiaKit midiakit = db.MidiaKit.Find(id);
            if (midiakit == null)
            {
                return HttpNotFound();
            }
            return View(midiakit);
        }

        //
        // POST: /MidiaKit/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            MidiaKit midiakit = db.MidiaKit.Find(id);

            midiakit.Excluido = true;
            db.Entry(midiakit).State = EntityState.Modified;
            db.Entry(midiakit).Property(b => b.DataCadastro).IsModified = false;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, midiakit.Id);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


        public ActionResult Download(string filename)
        {
            var document = Server.MapPath("~/conteudo/MidiaKit/" + filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(document);
            string fileName = filename;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


    }
}




