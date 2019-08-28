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
using System.Drawing;
using Newtonsoft.Json;
using System.Data.Entity.Validation;
using Administrativo.Helpers;
using WebMatrix.WebData;
using Administrativo.Enums;

namespace Administrativo.Controllers
{
    [CustomAuthorize(Roles = "Administrador,Enquetes")]
    public class EnqueteController : BaseController
    {
        public int areaADM = 9;
        public int areaADM2 = 25;
        //
        // GET: /Enquete/

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.EnqueteActive = "active";
            ViewBag.EnqueteSubActive = "active";
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
            ViewBag.SortClasspergunta = "sorting";
            ViewBag.SortClassativa = "sorting";
            ViewBag.SortClassexcluido = "sorting";
            ViewBag.SortClassdestaque = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Enquete> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Enquete;

            var pagedList = from obj in list where !obj.excluido select obj;
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
                    pagedList = pagedList.Where(g => g.pergunta.ToLower().Contains(searchParam.ToLower()));
                };

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
                case "pergunta":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.pergunta);
                        ViewBag.SortClasspergunta = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.pergunta);
                        ViewBag.SortClasspergunta = "sorting_desc";
                    }
                    break;
                case "ativa":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.ativa);
                        ViewBag.SortClassativa = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.ativa);
                        ViewBag.SortClassativa = "sorting_desc";
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
                case "destaque":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.destaque);
                        ViewBag.SortClassdestaque = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.destaque);
                        ViewBag.SortClassdestaque = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(e => e.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(e => e.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(e => e.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Enquete>(pageNumber, 10);
        }

        //
        // GET: /Enquete/Details/5

        //Classes para o json
        public class JsonLabels
        {
            public string label { get; set; }
            public int value { get; set; }
        }

        //Classes para o json
        public class JsonEnquete
        {
            public string element { get; set; }
            public bool resize { get; set; }
            public List<string> colors { get; set; }
            public List<JsonLabels> data { get; set; }
            public string hideHover { get; set; }
        }

        public ActionResult Details(int id = 0)
        {
            //Busco a enquete
            Enquete enquete = db.Enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }

            //Crio as list com as respostas e as cores do grafico
            var listLabel = new List<JsonLabels>();
            var listCor = new List<string>();
            var verifica = 0;
            var random = new Random();


            //Preencho com os valores das cores e respostas
            foreach (var i in enquete.Respostas)
            {
                JsonLabels labels = new JsonLabels
                {
                    label = i.resposta,
                    value = i.votos
                };
                //caso nenhum voto "verifica" fica com 0
                verifica += i.votos;
                listLabel.Add(labels);
                var color = String.Format("#{0:X6}", random.Next(0x1000000));
                listCor.Add(color);
            }

            //Coloco na classe enquete
            var Jenquete = new JsonEnquete()
            {
                element = "sales-chart",
                resize = true,
                colors = listCor,
                data = listLabel,
                hideHover = "auto"
            };

            ViewData["json"] = JsonConvert.SerializeObject(Jenquete);
            ViewBag.verifica = verifica;
            return View(enquete);
        }

        //
        // GET: /Enquete/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Enquete/Create

        [HttpPost]
        public ActionResult Create(Enquete enquete, string[] resposta, string[] correta)
        {
            if (ModelState.IsValid)
            {
                db.Enquete.Add(enquete);
                enquete.dataCadastro = DateTime.Now;

                int suffix = 0;

                do
                {

                    enquete.chave = enquete.pergunta.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                    suffix++;
                } while (db.Enquete.Where(o => o.chave == enquete.chave).Count() > 0);

                try
                {
                    db.SaveChanges();
                }
                catch (DbEntityValidationException e)
                {
                    #region just-debug
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                            eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                        //throw;
                    }
                    #endregion
                }

                for (int cont = 0; cont < resposta.Count(); cont++)
                {
                    if (resposta[cont] != null && resposta[cont] != "")
                    {
                        Respostas respostas = new Respostas();

                        respostas.idEnquete = enquete.id;
                        respostas.resposta = resposta[cont];
                        respostas.votos = 0;

                        respostas.excluido = false;

                        db.Respostas.Add(respostas);
                        db.SaveChanges();
                    }
                }
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, enquete.id);
                return RedirectToAction("Index");
            }

            return View(enquete);
        }

        //
        // GET: /Enquete/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Enquete enquete = db.Enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            return View(enquete);
        }

        //
        // POST: /Enquete/Edit/5

        [HttpPost]
        public ActionResult Edit(Enquete enquete)
        {
            if (ModelState.IsValid)
            {
                db.Entry(enquete).State = EntityState.Modified;

                int suffix = 0;

                do
                {
                    enquete.chave = enquete.pergunta.GenerateSlug() + (suffix > 0 ? suffix.ToString() : "");
                    suffix++;
                } while (db.Enquete.Where(o => o.chave == enquete.chave && o.id != enquete.id).Count() > 0);


                //List<Respostas> resp = db.Respostas.Where(x => x.idEnquete == enquete.id).ToList();
                //foreach (var x in resp)
                //{
                //    db.Respostas.Remove(x);
                //}

                //if (resposta != null)
                //{
                //    for (int cont = 0; cont < resposta.Count(); cont++)
                //    {
                //        if (resposta[cont] != null && resposta[cont] != "")
                //        {
                //            Respostas respostas = new Respostas();

                //            respostas.idEnquete = enquete.id;
                //            respostas.resposta = resposta[cont];
                //            respostas.votos = 0;

                //            respostas.excluido = false;

                //            db.Respostas.Add(respostas);
                //            //db.SaveChanges();

                //        }
                //    }
                //}

                db.SaveChanges();
                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, enquete.id);
                return RedirectToAction("Index");
            }
            return View(enquete);
        }


        [HttpPost]
        public ActionResult SalvaRespostas(int idEnquete, string[] resposta, int[] idR)
        {
            if (ModelState.IsValid)
            {
                var enquete = db.Enquete.FirstOrDefault(e => e.id == idEnquete);
                if (enquete != null)
                {
                    List<Respostas> respBanco = db.Respostas.Where(x => x.idEnquete == enquete.id).ToList();
                    var respostasToExclude = respBanco.Where(r => !idR.Contains(r.id));
                    foreach (var x in respostasToExclude)
                    {
                        db.Respostas.Remove(x);
                    }

                    if (idR.Any())
                    {
                        for (int cont = 0; cont < resposta.Count(); cont++)
                        {
                            if (resposta[cont] != null && resposta[cont] != "")
                            {
                                var resp = respBanco.FirstOrDefault(r => r.id == idR[cont]);

                                if (resp != null)
                                {
                                    resp.resposta = resposta[cont];
                                    resp.excluido = false;

                                    db.Entry(resp).State = EntityState.Modified;
                                }
                                else
                                {
                                    Respostas respostas = new Respostas();

                                    respostas.idEnquete = enquete.id;
                                    respostas.resposta = resposta[cont];
                                    respostas.votos = 0;

                                    respostas.excluido = false;

                                    db.Respostas.Add(respostas);

                                }
                            }
                        }
                    }

                    db.SaveChanges();
                    GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, enquete.id);
                    return RedirectToAction("Index");
                }
                return View(enquete);
            }
            return View();
        }

        //
        // GET: /Enquete/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Enquete enquete = db.Enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            return View(enquete);
        }

        //
        // POST: /Enquete/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Enquete enquete = db.Enquete.Find(id);
            //db.Entry(enquete).Collection("Resposta").Load();
            List<Respostas> resposta = db.Respostas.Where(x => x.idEnquete == id).ToList();
            foreach (var resp in resposta)
            {
                db.Respostas.Remove(resp);
            }
            //db.Enquete.Remove(enquete);
            enquete.excluido = true;
            db.Entry(enquete).State = EntityState.Modified;
            db.SaveChanges();

            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, enquete.id);
            return RedirectToAction("Index");
        }


        //
        // GET: /Enquete/DeleteResposta/5

        public ActionResult DeleteResposta(int id = 0)
        {
            Respostas resposta = db.Respostas.Find(id);
            if (resposta == null)
            {
                return HttpNotFound();
            }
            return View(resposta);
        }

        //
        // POST: /Enquete/DeleteResposta/5

        [HttpPost, ActionName("DeleteResposta")]
        public ActionResult DeleteConfirmedResposta(int id)
        {
            Respostas resposta = db.Respostas.Find(id);
            var idEnquete = resposta.idEnquete;
            db.Respostas.Remove(resposta);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Exclusao, resposta.id);
            return RedirectToAction("Edit/" + idEnquete);
        }

        //
        // GET: /Enquete/CadastraResposta/id
        public ActionResult CadastraResposta(int id)
        {
            Enquete enquete = db.Enquete.Find(id);
            if (enquete == null)
            {
                return HttpNotFound();
            }
            return View(enquete);
        }

        //
        // POST: /Enquete/CadastraResposta

        [HttpPost, ActionName("CadastraResposta")]
        public ActionResult CadastraRespostaOk(int id, string resposta)
        {
            Enquete enquete = db.Enquete.Find(id);
            var resp = new Respostas
            {
                votos = 0,
                idEnquete = enquete.id,
                excluido = false,
                Enquete = enquete,
                resposta = resposta
            };
            db.Respostas.Add(resp);
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Insercao, resp.id);
            return RedirectToAction("Edit/" + enquete.id);
        }


        //
        // GET: /Enquete/CadastraResposta/id
        public ActionResult AlterarResposta(int id)
        {
            Respostas resp = db.Respostas.Find(id);
            if (resp == null)
            {
                return HttpNotFound();
            }
            return View(resp);
        }

        //
        // POST: /Enquete/CadastraResposta

        [HttpPost, ActionName("AlterarResposta")]
        public ActionResult AlterarRespostaOk(int id, string resposta, int correta = 0)
        {
            Respostas resp = db.Respostas.Find(id);
            resp.certa = false;
            if (correta != 0)
            {
                List<Respostas> respostas = db.Respostas.Where(x => x.idEnquete == resp.idEnquete).ToList();
                foreach (var x in respostas)
                {
                    x.certa = false;
                    db.Entry(x).State = EntityState.Modified;
                }

                resp.certa = true;
            }
            resp.resposta = resposta;
            db.Entry(resp).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM2, TipoAcesso.Edicao, resp.id);
            return RedirectToAction("Edit/" + resp.idEnquete);
        }

    }
}




