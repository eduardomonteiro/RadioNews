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
    [CustomAuthorize(Roles = "Administrador,Programacao")]
    public class ProgramacaoController : BaseController
    {
        public int areaADM = 19;
        //
        // GET: /Programacao/

        //protected override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    ViewBag.caminhoCapa = caminhoCapa;
        //    ViewBag.caminhoLogo = caminhoLogo;
        //}

        public ActionResult Index(int? page, string search = "", string sortField = "", string order = "")
        {

            ViewBag.ProgramacaoActive = "active";
            ViewBag.ProgramacaoSubActive = "active";
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
            ViewBag.SortClassdiaSemana = "sorting";
            ViewBag.SortClasshorario = "sorting";
            ViewBag.SortClassdataCadastro = "sorting";

        }

        public IPagedList<Programacao> GetListItens(int? page, string searchParam = "", string sortField = "", string order = "")
        {


            var list = db.Programacao.Where(a => !a.excluido);

            var pagedList = from obj in list select obj;
            int pageNumber = (page ?? 1);

            if (!string.IsNullOrEmpty(searchParam))
            {
                int outDate = 0;
                int id = int.TryParse(searchParam, out outDate) ? int.Parse(searchParam) : 0;

                pagedList = pagedList.Where(p => p.id == id || p.nome.ToLower().Contains(searchParam.ToLower()) || p.chave.ToLower() == searchParam.ToLower());

            }


            switch (sortField)
            {
                case "id":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(p => p.id);
                        ViewBag.SortClassid = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(p => p.id);
                        ViewBag.SortClassid = "sorting_desc";
                    }
                    break;
                case "nome":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(p => p.nome);
                        ViewBag.SortClassnome = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(p => p.nome);
                        ViewBag.SortClassnome = "sorting_desc";
                    }
                    break;
                case "diaSemana":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(p => p.diaSemana);
                        ViewBag.SortClassdiaSemana = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(p => p.diaSemana);
                        ViewBag.SortClassdiaSemana = "sorting_desc";
                    }
                    break;
                case "horario":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(p => p.horario);
                        ViewBag.SortClasshorario = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(p => p.horario);
                        ViewBag.SortClasshorario = "sorting_desc";
                    }
                    break;
                case "dataCadastro":
                    if (order == "ASC")
                    {
                        pagedList = pagedList.OrderBy(p => p.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_asc";

                    }
                    else
                    {
                        pagedList = pagedList.OrderByDescending(p => p.dataCadastro);
                        ViewBag.SortClassdataCadastro = "sorting_desc";
                    }
                    break;
                default:
                    pagedList = pagedList.OrderByDescending(p => p.id);
                    ViewBag.SortClassid = "sorting_asc";
                    break;
            }

            return pagedList.ToPagedList<Programacao>(pageNumber, 10);
        }

        //
        // GET: /Programacao/Details/5

        public ActionResult Details(int id = 0)
        {
            Programacao programacao = db.Programacao.Include("Apresentadores").FirstOrDefault(a => a.id == id);
            if (programacao == null)
            {
                return HttpNotFound();
            }
            return View(programacao);
        }

        //
        // GET: /Programacao/Create

        public ActionResult Create()
        {
            var dayList = from DayOfWeek t in Enum.GetValues(typeof(DayOfWeek))
                          select new { ID = (int)t, Name = Utils.WeekDayName(t) };

            ViewBag.ApresentadoresId = new MultiSelectList(db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == false), "id", "nome");
            ViewBag.ParticipantesId = new MultiSelectList(db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == true), "id", "nome");

            ViewBag.diaSemana = new SelectList(dayList, "ID", "Name");

            //var editoriais = db.Editoriais.Where(a => !a.excluido).ToList();
            //int firstId = editoriais.FirstOrDefault().id;
            //ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome", firstId);
            ViewBag.EditoriaIdProg = db.Editoriais.Where(ed => ed.chave == "programacao").Select(ed => ed.id).FirstOrDefault();

            return View();
        }

        //
        // POST: /Programacao/Create

        [HttpPost]
        public ActionResult Create(Programacao programacao, HttpPostedFileBase imagem, HttpPostedFileBase logo, int[] ApresentadoresId, int[] ParticipantesId, string horariosTotal, string[] diadaSemana)
        {
            var dayList = from DayOfWeek t in Enum.GetValues(typeof(DayOfWeek))
                          select new { ID = (int)t, Name = Utils.WeekDayName(t) };
                      
            if (ModelState.IsValid)
            {

                if (ApresentadoresId != null)
                {
                    foreach (var apresentador in ApresentadoresId)
                    {
                        var apresentadores = db.Apresentadores.Find(apresentador);

                        programacao.Apresentadores.Add(apresentadores);
                    }
                }

                if (ParticipantesId != null)
                {
                    foreach (var participante in ParticipantesId)
                    {
                        var participantes = db.Apresentadores.Find(participante);

                        programacao.Apresentadores.Add(participantes);
                    }
                }

                programacao.dataCadastro = DateTime.Now;
                db.Programacao.Add(programacao);

                /*
                if (imagem != null)
                {
                    var pathCapa = Server.MapPath(caminhoCapa);
                    var pathLogo = Server.MapPath(caminhoLogo);
                   // programacao.logo = Utils.SaveAndCropImage(logo, pathLogo, 0, 0, 122, 104);
                    programacao.imagem = Utils.SaveAndCropImage(imagem, pathCapa, 0, 0, 680, 180);

                }
                else
                {
                    ModelState.AddModelError("imagem", "É necessário uma capa.");
                 //   ModelState.AddModelError("logo", "É necessário uma logo para o programa.");
                    return View(programacao);
                }
                */

                int suffix = 0;

                do
                {
                    programacao.chave = programacao.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Programacao.Where(o => o.chave == programacao.chave).Count() > 0);


                db.SaveChanges();

                #region tratamento de horarios

                if (programacao.periodo == 2) // horario e dia indeterminado
                {
                    Horario_programacao horario = new Horario_programacao
                    {
                        diaSemana = null,
                        horario = null,
                        idPrograma = programacao.id,
                        Programacao = programacao
                    };
                    db.Horario_programacao.Add(horario);
                    db.SaveChanges();
                }
                else
                {
                    //========================================== TRATAMENTO DE HORARIOS ==========================================
                    string[] result;
                    string[] stringSeparators = new string[] { "/" };
                    string[] stringSeparators2 = new string[] { "," };

                    result = horariosTotal.Split(stringSeparators, StringSplitOptions.None);

                    //array de horarios pra cada dia
                    string[] segunda;
                    string[] terca;
                    string[] quarta;
                    string[] quinta;
                    string[] sexta;
                    string[] sabado;
                    string[] domingo;

                    if (diadaSemana == null)
                    {
                        diadaSemana[0] = string.Empty;
                    }

                    //alimentos com os horarios
                    segunda = result[0].Split(stringSeparators2, StringSplitOptions.None);
                    terca = result[1].Split(stringSeparators2, StringSplitOptions.None);
                    quarta = result[2].Split(stringSeparators2, StringSplitOptions.None);
                    quinta = result[3].Split(stringSeparators2, StringSplitOptions.None);
                    sexta = result[4].Split(stringSeparators2, StringSplitOptions.None);
                    sabado = result[5].Split(stringSeparators2, StringSplitOptions.None);
                    domingo = result[6].Split(stringSeparators2, StringSplitOptions.None);

                    if (!string.IsNullOrEmpty(segunda[0]) || diadaSemana.Contains("box-segunda"))
                    {
                        foreach (var hr in segunda)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 1,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }


                    }
                    if (!string.IsNullOrEmpty(terca[0]) || diadaSemana.Contains("box-terca"))
                    {
                        foreach (var hr in terca)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 2,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (!string.IsNullOrEmpty(quarta[0]) || diadaSemana.Contains("box-quarta"))
                    {
                        foreach (var hr in quarta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 3,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (!string.IsNullOrEmpty(quinta[0]) || diadaSemana.Contains("box-quinta"))
                    {
                        foreach (var hr in quinta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 4,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (!string.IsNullOrEmpty(sexta[0]) || diadaSemana.Contains("box-sexta"))
                    {
                        foreach (var hr in sexta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 5,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (!string.IsNullOrEmpty(sabado[0]) || diadaSemana.Contains("box-sabado"))
                    {
                        foreach (var hr in sabado)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 6,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (!string.IsNullOrEmpty(domingo[0]) || diadaSemana.Contains("box-domingo"))
                    {
                        foreach (var hr in domingo)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 0,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }

                    db.SaveChanges();
                }


                #endregion



                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Insercao, programacao.id);
                return RedirectToAction("Index");
            }

            ViewBag.ApresentadoresId = new MultiSelectList(db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == false), "id", "nome");
            ViewBag.ParticipantesId = new MultiSelectList(db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == true), "id", "nome");

            ViewBag.diaSemana = new SelectList(dayList, "ID", "Name");
            ViewBag.EditoriaIdProg = db.Editoriais.Where(ed => ed.chave == "programacao").Select(ed => ed.id).FirstOrDefault();
             
            //var editoriais = db.Editoriais.Where(a => !a.excluido).ToList();
            //int firstId = editoriais.FirstOrDefault().id;
            //ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome", firstId);

            return View(programacao);
        }

        //
        // GET: /Programacao/Edit/5

        public ActionResult Edit(int id = 0)
        {
            
            Programacao programacao = db.Programacao.Include("Apresentadores").FirstOrDefault(a => a.id == id);

            //var editoriais = db.Editoriais.Where(a => !a.excluido).ToList();

            //ViewBag.EditoriaId = new SelectList(editoriais, "id", "nome", programacao.editoriaId);
            
            if (programacao == null)
            {
                return HttpNotFound();
            }

            var apresentadores = db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == false);

            List<int> allapresentadores = new List<int>();
            foreach (var apresentador in programacao.Apresentadores.Where(x=>x.participanteConvidado == false))
            {
                allapresentadores.Add(apresentador.id);
            }

            ViewBag.Apresent = new MultiSelectList(apresentadores, "id", "nome", allapresentadores);



            var participantes = db.Apresentadores.Where(a => !a.excluido && a.participanteConvidado == true);

            List<int> allparticipantes = new List<int>();
            foreach (var participante in programacao.Apresentadores.Where(x=>x.participanteConvidado == true))
            {
                allparticipantes.Add(participante.id);
            }

            ViewBag.ParticipantesId = new MultiSelectList(participantes, "id", "nome", allparticipantes);


            //dias da semana
            ViewBag.segunda = db.Horario_programacao.Where(x => x.diaSemana == 1 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.terca = db.Horario_programacao.Where(x => x.diaSemana == 2 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.quarta = db.Horario_programacao.Where(x => x.diaSemana == 3 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.quinta = db.Horario_programacao.Where(x => x.diaSemana == 4 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.sexta = db.Horario_programacao.Where(x => x.diaSemana == 5 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.sabado = db.Horario_programacao.Where(x => x.diaSemana == 6 && x.idPrograma == id).Select(x => x.horario).ToList();
            ViewBag.domingo = db.Horario_programacao.Where(x => x.diaSemana == 0 && x.idPrograma == id).Select(x => x.horario).ToList();

            ViewBag.EditoriaIdProg = db.Editoriais.Where(ed => ed.chave.Contains("programacao")).Select(ed => ed.id).FirstOrDefault();

            return View(programacao);
        }

        //
        // POST: /Programacao/Edit/5

        [HttpPost]
        public ActionResult Edit(Programacao programacao, HttpPostedFileBase imagemUpload, HttpPostedFileBase logoUpload, int[] Apresent, int[] Participantes, string horariosTotal, string[] diadaSemana)
        {
            var dayList = from DayOfWeek t in Enum.GetValues(typeof(DayOfWeek))
                          select new { ID = (int)t, Name = Utils.WeekDayName(t) };


            ViewBag.diaSemana = new SelectList(dayList, "ID", "Name", programacao.diaSemana);

            if (ModelState.IsValid)
            {
                db.Entry(programacao).State = EntityState.Modified;
                db.Entry(programacao).Collection("Apresentadores").Load();

                programacao.Apresentadores.Clear();

                if (Apresent != null)
                {
                    foreach (int id in Apresent)
                    {
                        var dbApresent = db.Apresentadores.Find(id);
                        programacao.Apresentadores.Add(dbApresent);
                    }
                }

                if (Participantes != null)
                {
                    foreach (int id in Participantes)
                    {
                        var dbApresent = db.Apresentadores.Find(id);
                        programacao.Apresentadores.Add(dbApresent);
                    }
                }

                int suffix = 0;

                do
                {
                    programacao.chave = programacao.nome.GenerateSlug() + (suffix > 0 ? (suffix++).ToString() : "");
                    suffix++;
                } while (db.Programacao.Where(o => o.chave == programacao.chave && o.id != programacao.id).Count() > 0);


                db.Entry(programacao).Property("dataCadastro").IsModified = false;



                #region tratamento de horarios
                List<Horario_programacao> horasAntigas = db.Horario_programacao.Where(x => x.idPrograma == programacao.id).ToList();
                foreach (var horaAntiga in horasAntigas)
                {
                    db.Horario_programacao.Remove(horaAntiga);
                }

                if (programacao.periodo == 2) // horario e dia indeterminado
                {
                    Horario_programacao horario = new Horario_programacao
                    {
                        diaSemana = null,
                        horario = null,
                        idPrograma = programacao.id,
                        Programacao = programacao
                    };
                    db.Horario_programacao.Add(horario);
                    db.SaveChanges();
                }
                else
                {

                    string[] result;
                    string[] stringSeparators = new string[] { "/" };
                    string[] stringSeparators2 = new string[] { "," };

                    result = horariosTotal.Split(stringSeparators, StringSplitOptions.None);

                    //array de horarios pra cada dia
                    string[] segunda;
                    string[] terca;
                    string[] quarta;
                    string[] quinta;
                    string[] sexta;
                    string[] sabado;
                    string[] domingo;

                    //alimentos com os horarios
                    segunda = result[0].Split(stringSeparators2, StringSplitOptions.None);
                    terca = result[1].Split(stringSeparators2, StringSplitOptions.None);
                    quarta = result[2].Split(stringSeparators2, StringSplitOptions.None);
                    quinta = result[3].Split(stringSeparators2, StringSplitOptions.None);
                    sexta = result[4].Split(stringSeparators2, StringSplitOptions.None);
                    sabado = result[5].Split(stringSeparators2, StringSplitOptions.None);
                    domingo = result[6].Split(stringSeparators2, StringSplitOptions.None);


                    if (segunda[0] != "" || diadaSemana.Contains("box-segunda"))
                    {
                        foreach (var hr in segunda)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 1,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (terca[0] != "" || diadaSemana.Contains("box-terca"))
                    {
                        foreach (var hr in terca)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 2,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (quarta[0] != "" || diadaSemana.Contains("box-quarta"))
                    {
                        foreach (var hr in quarta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 3,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (quinta[0] != "" || diadaSemana.Contains("box-quinta"))
                    {
                        foreach (var hr in quinta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 4,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (sexta[0] != "" || diadaSemana.Contains("box-sexta"))
                    {
                        foreach (var hr in sexta)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 5,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (sabado[0] != "" || diadaSemana.Contains("box-sabado"))
                    {
                        foreach (var hr in sabado)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 6,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }
                    if (domingo[0] != "" || diadaSemana.Contains("box-domingo"))
                    {
                        foreach (var hr in domingo)
                        {
                            Horario_programacao horario = new Horario_programacao
                            {
                                diaSemana = 0,
                                horario = (string.IsNullOrEmpty(hr) ? null : hr),
                                idPrograma = programacao.id,
                                Programacao = programacao
                            };
                            db.Horario_programacao.Add(horario);
                        }
                    }

                    db.SaveChanges();
                }
                #endregion

                GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Edicao, programacao.id);
                return RedirectToAction("Index");
            }

            ViewBag.EditoriaIdProg = db.Editoriais.Where(ed => ed.chave == "programacao").Select(ed => ed.id).FirstOrDefault();
            return View(programacao);
        }

        //
        // GET: /Programacao/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Programacao programacao = db.Programacao.Find(id);

            if (programacao == null)
            {
                return HttpNotFound();
            }
            return View(programacao);
        }

        //
        // POST: /Programacao/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Programacao programacao = db.Programacao.Find(id);

            db.Entry(programacao).State = EntityState.Modified;

            db.Entry(programacao).Collection("Apresentadores").Load();
            db.Entry(programacao).Collection("Horario_programacao").Load();
            db.Entry(programacao).Collection("Materia").Load();

            List<Horario_programacao> horasAntigas = db.Horario_programacao.Where(x => x.idPrograma == programacao.id).ToList();
            foreach (var horaAntiga in horasAntigas)
            {

                db.Horario_programacao.Remove(horaAntiga);
            }

            List<Materia> MateriaAntigas = db.Materia.Where(x => x.idProgramacao == programacao.id).ToList();
            foreach (var materiaAntiga in MateriaAntigas)
            {
                materiaAntiga.excluido = true;
                db.Entry(materiaAntiga).State = EntityState.Modified;
                //db.Materia.Remove(materiaAntiga);
            }
            programacao.Apresentadores.Clear();
            //programacao.Horario_programacao.Clear();
            //programacao.Materia.Clear();

            programacao.excluido = true;

            db.Entry(programacao).State = EntityState.Modified;
            db.SaveChanges();
            GerenciaLogs.saveLog(ref db, WebSecurity.GetUserId(User.Identity.Name), areaADM, TipoAcesso.Exclusao, programacao.id);
            return RedirectToAction("Index");
        }
    }
}




