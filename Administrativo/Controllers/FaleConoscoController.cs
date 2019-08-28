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
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Fale Conosco")]
    public class FaleConoscoController : BaseController
    {
        public int areaADM = 13;
        //
        // GET: /FaleConosco/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.FaleConoscoActive = "active";
            ViewBag.FaleConoscoSubActive = "active";
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
            ViewBag.SortClassResposta = "sorting";
            ViewBag.SortClassassunto = "sorting";
            ViewBag.SortClassnome = "sorting";
            ViewBag.SortClassemail = "sorting";
            ViewBag.SortClassestado = "sorting";
            ViewBag.SortClasscidade = "sorting";
            ViewBag.SortClasscelular = "sorting";
            ViewBag.SortClasstelefone = "sorting";
            ViewBag.SortClassmesagem = "sorting";
            ViewBag.SortClassexcluido = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<FaleConosco> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.FaleConosco.Where(x=>!x.excluido);

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
                    pagedList = pagedList.Where(g => g.nome.ToLower().Contains(searchParam.ToLower()) || g.assunto.ToLower().Contains(searchParam.ToLower()));
                };

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "dataResposta":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.dataResposta);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.dataResposta);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "assunto":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.assunto);
                        ViewBag.SortClassassunto = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.assunto);
                        ViewBag.SortClassassunto = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "email":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.email);
                        ViewBag.SortClassemail = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.email);
                        ViewBag.SortClassemail = "sorting_desc";
                    }
                    break;
                case "estado":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.estado);
                        ViewBag.SortClassestado = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.estado);
                        ViewBag.SortClassestado = "sorting_desc";
                    }
                    break;
                case "cidade":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.cidade);
                        ViewBag.SortClasscidade = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.cidade);
                        ViewBag.SortClasscidade = "sorting_desc";
                    }
                    break;
                case "celular":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.celular);
                        ViewBag.SortClasscelular = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.celular);
                        ViewBag.SortClasscelular = "sorting_desc";
                    }
                    break;
                case "telefone":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.telefone);
                        ViewBag.SortClasstelefone = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.telefone);
                        ViewBag.SortClasstelefone = "sorting_desc";
                    }
                    break;
                case "mesagem":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.mensagem);
                        ViewBag.SortClassmesagem = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.mensagem);
                        ViewBag.SortClassmesagem = "sorting_desc";
                    }
                    break;
                case "excluido":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.excluido);
                        ViewBag.SortClassexcluido = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.excluido);
                        ViewBag.SortClassexcluido = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(f => f.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(f => f.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(f => f.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<FaleConosco>(pageNumber, 10);
        }

        //
        // GET: /FaleConosco/Details/5

        public ActionResult Details(int id = 0)
        {
            FaleConosco faleconosco = db.FaleConosco.Find(id);
            if (faleconosco == null)
            {
                return HttpNotFound();
            }

            //Marco como lida.
            faleconosco.lida = 1;
            
            db.Entry(faleconosco).State = EntityState.Modified;
            db.SaveChanges();

            return View(faleconosco);
        }

        //
        // GET: /FaleConosco/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /FaleConosco/Create

        [HttpPost]
        public ActionResult Create(FaleConosco faleconosco)
        {
            if (ModelState.IsValid)
            {
                db.FaleConosco.Add(faleconosco);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(faleconosco);
        }

        //
        // GET: /FaleConosco/Edit/5

        public ActionResult Responder(int id = 0)
        {
            FaleConosco faleconosco = db.FaleConosco.Find(id);
            if (faleconosco == null)
            {
                return HttpNotFound();
            }

            //Marco como lida.
            faleconosco.lida = 1;
            db.Entry(faleconosco).State = EntityState.Modified;
            db.SaveChanges();

            return View(faleconosco);
        }

        //
        // POST: /FaleConosco/Edit/5

        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Responder(FaleConosco faleconosco)
        {
            if (ModelState.IsValid)
            {
                faleconosco.lida = 1;
                db.Entry(faleconosco).State = EntityState.Modified;

                faleconosco.dataResposta = DateTime.Now;
                faleconosco.assunto = "";

                String body = System.IO.File.ReadAllText(Server.MapPath("~/ModelosEmail/modeloemail.asp"));

                var emailModel = new MailModel();

                body = body.Replace("##nome##", faleconosco.nome);
                body = body.Replace("##mensagem##", faleconosco.mensagem);
                body = body.Replace("##resposta##", faleconosco.resposta);
                body = body.Replace("../", "http://" + Request.Url.Authority + "/");

                body = body.Replace("##data##", DateTime.Now.ToString("dd/MM/yyyy"));

                emailModel.Body = body;
                emailModel.Subject = "Rádio CompanyName - Fale Conosco";
                emailModel.To = faleconosco.email;

                emailModel.SendEmail();

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, faleconosco.id);
                return RedirectToAction("Index");
            }
            return View(faleconosco);
        }

        //
        // GET: /FaleConosco/Delete/5

        public ActionResult Delete(int id = 0)
        {
            FaleConosco faleconosco = db.FaleConosco.Find(id);
            if (faleconosco == null)
            {
                return HttpNotFound();
            }
            return View(faleconosco);
        }

        //
        // POST: /FaleConosco/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            FaleConosco faleconosco = db.FaleConosco.Find(id);
            faleconosco.excluido = true;

            db.Entry(faleconosco).State = EntityState.Modified;

            //db.FaleConosco.Remove(faleconosco);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, faleconosco.id);
            return RedirectToAction("Index");
        }
    }
}




