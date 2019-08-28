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
    public class EGolPADController : BaseController
    {
        int areaADM = 46;
        protected const string pathOriginal = "~/Conteudo/EGolPAD/{0}/";
        //
        // GET: /NoticiasPAD/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {
            ViewBag.Search = search;
            ViewBag.Order = order;
            ViewBag.SortField = sortField;
            ViewBag.Page = page;
            allClassOffSort();

            return View(GetListItens(page, search, sortField, order));
        }

        private IPagedList<EGolPADs> GetListItens(int? page, string search, string sortField, string order)
        {
            var list = db.EGolPADs.Where(t => !t.Excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(search))
            {
                int outDate = 0;
                int id = int.TryParse(search, out outDate) ? int.Parse(search) : 0;

                if (id > 0)
                    pagedList = pagedList.Where(e => e.Id == id);
                else
                    pagedList = pagedList.Where(e => e.Jogador.Contains(search) || e.TimesPADs.Nome.Contains(search) || e.TimesPADs1.Nome.Contains(search));

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
                case "Jogador":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.Jogador);
                        ViewBag.SortClassJogador = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.Jogador);
                        ViewBag.SortClassJogador = "sorting_desc";
                    }
                    break;
                case "Mandante":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.TimesPADs);
                        ViewBag.SortClassMandante = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.TimesPADs);
                        ViewBag.SortClassMandante = "sorting_desc";
                    }
                    break;
                case "Visitante":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.TimesPADs1);
                        ViewBag.SortClassVisitante = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.TimesPADs1);
                        ViewBag.SortClassVisitante = "sorting_desc";
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

            return pagedList.ToPagedList<EGolPADs>(pageNumber, 10);
        }

        private void allClassOffSort()
        {
            ViewBag.SortClassId = "sorting";
            ViewBag.SortClassJogador = "sorting";
            ViewBag.SortClassMandante = "sorting";
            ViewBag.SortClassVisitante = "sorting";
            ViewBag.SortClassDataCadastro = "sorting";
        }

        public ActionResult Details(int id = 0)
        {
            EGolPADs gol = db.EGolPADs.Find(id);
            if (gol == null)
            {
                return HttpNotFound();
            }
            return View(gol);
        }
        public ActionResult Edit(int id)
        {
            EGolPADs gol = db.EGolPADs.Find(id);
            if (gol == null)
            {
                return HttpNotFound();
            }

            SetViewBag(gol);

            return View(gol);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EGolPADs gol)
        {
            Validacao(gol);
            if (ModelState.IsValid)
            {
                gol.DataCadastro = DateTime.Now;
                gol.Excluido = false;

                db.Entry(gol).State = EntityState.Modified;
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, gol.Id);
                return RedirectToAction("Details", new { id = gol.Id });
            }

            SetViewBag(gol);

            return View(gol);
        }
        public ActionResult Create()
        {
            SetViewBag();

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EGolPADs gol)
        {
            Validacao(gol);
            if (ModelState.IsValid)
            {
                gol.Excluido = false;
                gol.DataCadastro = DateTime.Now;

                db.EGolPADs.Add(gol);
                db.SaveChanges();

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, gol.Id);
                return RedirectToAction("Details", new { id = gol.Id });
            }

            SetViewBag(gol);

            return View(gol);
        }
        public ActionResult Delete(int id = 0)
        {
            if (id == 0)
            {
                return HttpNotFound();
            }
            EGolPADs gol = db.EGolPADs.Find(id);
            if (gol == null)
            {
                return HttpNotFound();
            }
            return View(gol);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            EGolPADs gol = db.EGolPADs.Find(id);

            gol.Excluido = true;
            db.Entry(gol).State = EntityState.Modified;

            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, gol.Id);
            return RedirectToAction("Index");
        }

        private void SetViewBag(EGolPADs gol = null)
        {
            ApoioViewBag(gol);
            TimesViewBag(gol);
            PlacarViewBag(gol);
            BgColorViewBag(gol);
        }
        private void BgColorViewBag(EGolPADs gol)
        {
            //Método usado para gerar a listbox de cores.
            var colorlist = new Dictionary<string, string>();
            colorlist.Add("black", "Preto");
            colorlist.Add("blue", "Azul");
            colorlist.Add("green", "Verde");

            if (gol != null)
            {
                ViewBag.BgColor = new SelectList(colorlist, "key", "value", gol.BgColor);
            }
            else
            {
                ViewBag.BgColor = new SelectList(colorlist, "key", "value");
            }

        }
        private void ApoioViewBag(EGolPADs gol)
        {
            //Método usado para gerar a listbox de apoiadores na view.
            var apoiadores = db.ApoioPADs.Where(a => !a.Excluido).ToList();
            if (gol != null)
            {
                ViewBag.ApoioId = new SelectList(apoiadores, "id", "nome", gol.ApoioId);
            } else
            {
                ViewBag.ApoioId = new SelectList(apoiadores, "id", "nome");
            }

        }
        private void TimesViewBag(EGolPADs gol)
        {
            //Método usado para gerar as listbox de times na view.
            var times = db.TimesPADs.Where(a => !a.Excluido).ToList();
            if (gol != null)
            {
                ViewBag.VisitanteId = new SelectList(times, "id", "nome", gol.VisitanteId);
                ViewBag.MandanteId = new SelectList(times, "id", "nome", gol.MandanteId);
            } else
            {
                ViewBag.VisitanteId = new SelectList(times, "id", "nome");
                ViewBag.MandanteId = new SelectList(times, "id", "nome");
            }
        }
        private void PlacarViewBag(EGolPADs gol)
        {
            if (gol == null) return;
            if (gol.Placar != null)
            {
                var pontos = gol.Placar.Split(new char[] { 'x' });
                ViewBag.GolsMandante = pontos[0];
                ViewBag.GolsVisitante = pontos[1];
            } else
            {
                ViewBag.GolsMandante = 0;
                ViewBag.GolsVisitante = 0;
            }
        }

        private void Validacao(EGolPADs gol)
        {
            if (string.IsNullOrWhiteSpace(gol.Placar))
                ModelState.AddModelError("Placar", "O Placar deve ser preenchido.");

            if (string.IsNullOrWhiteSpace(gol.Jogador))
                ModelState.AddModelError("Jogador", "O Jogador deve ser preenchido.");
            else if (gol.Jogador.Length > 100)
                ModelState.AddModelError("Jogador", "O Jogador não deve ter mais do que 100 caracteres.");

            if (string.IsNullOrWhiteSpace(gol.HoraDoGol))
                ModelState.AddModelError("HoraDoGol", "A Hora do Gol deve ser preenchida.");
            else if (gol.HoraDoGol.Length > 30)
                ModelState.AddModelError("HoraDoGol", "A Hora do Gol não deve ter mais do que 30 caracteres.");

            if (string.IsNullOrWhiteSpace(gol.BgColor))
                ModelState.AddModelError("BgColor", "O BgColor deve ser preenchido.");
        }
    }
}
