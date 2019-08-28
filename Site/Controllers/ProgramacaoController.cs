using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Site.Enums;
using Site.Helpers;
using System.Data.Entity;

namespace Site.Controllers
{
    public class ProgramacaoController : BaseController
    {
        // GET: Programacao
        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "dia")]
        public async Task<ActionResult> Index(string dia = null)
        {
            
            if (!string.IsNullOrEmpty(dia))
            {
                int diaCode = Utils.diaSemanaChave(dia);
                if (dia == "diferenciada")
                {
                    var programacaoDia = await db.Horario_programacao.Where(x => x.Programacao.periodo == 2 && x.Programacao.excluido  == false).ToListAsync();

                    return View(programacaoDia);
                }
                else
                {
                    var programacaoDia = await db.Horario_programacao.Where(x=>x.diaSemana == diaCode && 
                    x.Programacao.excluido == false).OrderBy(t => t.horario).ToListAsync();

                    return View(programacaoDia);
                }
                

            }
            else 
            {
                var programacaoDia = await db.Horario_programacao.Where(p=>p.diaSemana == 1 && p.Programacao.excluido == false ).OrderBy(t => t.horario).ToListAsync();

                return View(programacaoDia);
            }


        }




    }
}