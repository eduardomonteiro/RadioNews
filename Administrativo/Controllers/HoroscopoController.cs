using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrativo.Models;
using PagedList;
using Administrativo.Commons;
using WebMatrix.WebData;
using Administrativo.Enums;
using System.IO;
using Administrativo.Helpers;
using System.Data;
using System.Data.Entity;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Horoscopo")]
    public class HoroscopoController : BaseController
    {
        public int areaADM = 28;
        //
        // GET: /Horoscopos/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.HoroscopoActive = "active";
            ViewBag.HoroscopoSubActive = "active";
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

        public IPagedList<Horoscopoes> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Horoscopoes.ToList();

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(m => m.Id == id || m.Descricao.Contains(searchParam) || m.Audio.Contains(searchParam) || m.Signo.Contains(searchParam));

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
                case "Descricao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Descricao);
                        ViewBag.SortClassDescricao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Descricao);
                        ViewBag.SortClassDescricao = "sorting_desc";
                    }
                    break;
                case "Audio":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Audio);
                        ViewBag.SortClassAudio = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Audio);
                        ViewBag.SortClassAudio = "sorting_desc";
                    }
                    break;
                case "Signo":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.Signo);
                        ViewBag.SortClassSigno = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.Signo);
                        ViewBag.SortClassSigno = "sorting_desc";
                    }
                    break;
                //case "Ativo":
                //    if (order == "ASC")
                //    {
                //        pagedList = pagedList.OrderBy(m => m.Ativo);
                //        ViewBag.SortClassAtivo = "sorting_asc";

                //    }
                //    else
                //    {
                //        pagedList = pagedList.OrderByDescending(m => m.Ativo);
                //        ViewBag.SortClassAtivo = "sorting_desc";
                //    }
                //    break;
                //case "Excluido":
                //    if (order == "ASC")
                //    {
                //        pagedList = pagedList.OrderBy(m => m.Excluido);
                //        ViewBag.SortClassExcluido = "sorting_asc";

                //    }
                //    else
                //    {
                //        pagedList = pagedList.OrderByDescending(m => m.Excluido);
                //        ViewBag.SortClassExcluido = "sorting_desc";
                //    }
                //    break;
                case "DataAtualizacao":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(m => m.DataAtualizacao);
                        ViewBag.SortClassDataAtualizacao = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(m => m.DataAtualizacao);
                        ViewBag.SortClassDataAtualizacao = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(m => m.Id);
                    ViewBag.SortClassId = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Horoscopoes>(pageNumber, 12);
        }

        public ActionResult Edit(int id = 0)
        {
            var signo = db.Horoscopoes.Find(id);
            if (signo == null)
            {
                return HttpNotFound();
            }
            return View(signo);
        }

        //
        // POST: /MidiaKit/Edit/5

        [HttpPost]
        public ActionResult Edit(Horoscopoes horoscopo, HttpPostedFileBase AudioF, HttpPostedFileBase MiniaturaF)
        {
            if (ModelState.IsValid)
            {
                horoscopo.DataAtualizacao = DateTime.Now;
                db.Entry(horoscopo).State = EntityState.Modified;
                //db.SaveChanges();

                if (AudioF != null && AudioF.ContentLength > 0)
                {
                    var extension = Path.GetExtension(AudioF.FileName);

                    string[] permitidos = { ".mp3", ".mp3 ", ".aac" };

                    if (!permitidos.Contains(extension.ToLower()))
                    {
                        ModelState.AddModelError("", "Extensão não permitida!");
                    }
                    else
                    {
                        string AudioAtual = horoscopo.Audio;

                        string caminho = Server.MapPath("~/conteudo/horoscopo/" + horoscopo.Id + "/");

                        if (System.IO.File.Exists(caminho + AudioAtual))
                        {
                            System.IO.File.Delete(caminho + AudioAtual);
                        }

                        if (!Directory.Exists(caminho))
                        {
                            Directory.CreateDirectory(caminho);
                        }

                        AudioF.SaveAs(caminho + AudioF.FileName);
                        horoscopo.Audio = AudioF.FileName;

                    }
                }

                //if (MiniaturaF != null && MiniaturaF.ContentLength > 0)
                //{
                //    var extension = Path.GetExtension(MiniaturaF.FileName);

                //    string[] permitidos = { ".jpg", ".jpeg", ".gif", ".png" };

                //    if (!permitidos.Contains(extension.ToLower()))
                //    {
                //        ModelState.AddModelError("", "Extensão não permitida!");
                //    }
                //    else
                //    {
                //        var Temp = Server.MapPath("~/conteudo/Temp/");
                //        var pathArquivoFinal = Server.MapPath("~/conteudo/MidiaKit/miniaturas/");

                //        MiniaturaF.SaveAs(System.IO.Path.Combine(Temp, MiniaturaF.FileName));
                //        midiakit.Miniatura = Utils.resizeImageAndSave(System.IO.Path.Combine(Temp, MiniaturaF.FileName), 300, 300, pathArquivoFinal);

                //    }
                //}

                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, horoscopo.Id);
                
                return RedirectToAction("Index");
            }
            return View(horoscopo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Download(string filename, int id)
        {
            var document = Server.MapPath("~/conteudo/horoscopo/" + id + "/" + filename);
            byte[] fileBytes = System.IO.File.ReadAllBytes(document);
            string fileName = filename;
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


    }
}
