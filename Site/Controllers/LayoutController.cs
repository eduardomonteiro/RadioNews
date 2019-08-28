using Site.Enums;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.Mvc;
using Site.Services;

namespace Site.Controllers
{
    public class LayoutController : BaseController
    {
        private string primeKey = "layout:";
        // GET: Layout
        public PartialViewResult Menu()
        {
            var retorno = MenuCached();

            return PartialView("_Menu", retorno);
        }
        public PartialViewResult Header()
        {
            var retorno = MenuCached();

            return PartialView("_Header", retorno);
        }
        private MenuViewModel MenuCached()
        {
            string key = primeKey + "Menu:TRadios:TEditoriais:TCategoriasAudios";

            Func<object, MenuViewModel> funcao = t => MenuDB();
            var retorno = RedisService.GetOrSetToRedis(key, funcao, 300);
            return retorno;
        }
        private MenuViewModel MenuDB()
        {
            var radios = db.Radios.Where(x => x.Ativo).OrderBy(x => x.Titulo).Select(r => new RadiosMenuViewModel { Chave = r.Chave, Titulo = r.Titulo, Stream1 = r.Stream1, Stream2 = r.Stream2 }).ToList();
            var editorias = db.Editoriais.Where(x => x.ativo && !x.especial & !x.excluido && !x.esportes && x.chave != "ouvintes-no-ar").OrderBy(x => x.nome).Select(e => new EditoriasLayoutViewModel { chave = e.chave, nome = e.nome }).ToList();
            var campeonatos = db.Editoriais.Where(x => x.ativo && !x.especial & !x.excluido && x.esportes && x.Action != null).OrderBy(x => x.nome).Select(e => new EditoriasLayoutViewModel { Action = e.Action, nome = e.nome }).ToList();
            var categorias = db.CategoriasAudios.Where(x => x.Liberado && !x.Excluido).OrderBy(x => x.Descricao).Select(a => new CategoriasAudiosMenuViewModel { Descricao = a.Descricao, Id = a.Id }).ToList();

            var diaS = (int)DateTime.Now.DayOfWeek;
            var horaAtual = string.Format("{0:HH:mm tt}", DateTime.Now);

            var programacoesDia = db.Horario_programacao.Where(x => x.diaSemana == diaS).Select(y => new ProgramacoesVM { programaId = y.idPrograma.ToString(), horario = y.horario, Data = new DateTime() }).ToList();
            foreach (var item in programacoesDia)
            {
                item.Data = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + item.horario);
            }

            var atual = programacoesDia.OrderBy(x => x.Data).LastOrDefault(x => x.Data <= DateTime.Now);
            var horarioFinal = programacoesDia.OrderBy(x => x.Data).Where(x => x.Data >= DateTime.Now).Take(2).FirstOrDefault();
            var convertId = Int32.Parse(atual.programaId);

            var hf = "";
            if (horarioFinal != null)
            {
                hf = horarioFinal.horario;
            }

            var programaAtual = db.Programacao.Where(x => x.id == convertId).Select(a => new ProgramacaoMenuViewModel { Id = a.id, Horario = atual.horario, FimHorario = hf, Nome = a.nome }).ToList();

            var viewModel = new MenuViewModel
            {
                Editorias = editorias,
                Radios = radios,
                Campeonatos = campeonatos,
                CategoriasAudios = categorias,
                Programacao = programaAtual
            };
            return viewModel;
        }

        public PartialViewResult MenuEspeciais()
        {
            var retorno = EditoriaisLayoutCached();

            return PartialView("_NavigationBar", retorno);
        }
        public PartialViewResult Footer()
        {
            var retorno = EditoriaisLayoutCached();

            return PartialView("_Footer", new FooterViewModel { Editoriais = retorno });
        }
        private List<EditoriasLayoutViewModel> EditoriaisLayoutCached()
        {
            string key = primeKey + "EditoriaisLayoutCached:TEditoriais";

            Func<object, List<EditoriasLayoutViewModel>> funcao = t => MenuEspeciaisDB();
            var retorno = RedisService.GetOrSetToRedis(key, funcao, 300);

            return retorno;
        }
        private List<EditoriasLayoutViewModel> MenuEspeciaisDB()
        {
            var editoriais = db.Editoriais.Where(x => !x.excluido && x.ativo && x.especial)
                .OrderByDescending(x => x.DataCadastro)
                .Select(e => new EditoriasLayoutViewModel {
                    Action = e.Especiais_Modelos != null ? e.Especiais_Modelos.Action : "",
                    chave = e.chave,
                    nome = e.nome,
                    Canal = e.canal,
                    Especial = e.especial
                }).ToList();

            return editoriais;
        }
        
        public PartialViewResult RedesSociais()
        {
            string key = primeKey + "RedesSociais:TRedesSociais";

            Func<object, RedesSociaisHomeViewModel> funcao = t => RedesSociaisDB();
            var retorno = RedisService.GetOrSetToRedis(key, funcao, 60);


            return PartialView("_RedesSociais", retorno);
        }
        private RedesSociaisHomeViewModel RedesSociaisDB()
        {
            var redesTwitter = db.RedesSociais.Where(x => (RedeSocialTipo)x.TipoRede == RedeSocialTipo.Twitter).Take(2).OrderByDescending(x => x.Data);
            var redesFace = db.RedesSociais.Where(x => (RedeSocialTipo)x.TipoRede == RedeSocialTipo.Facebook && x.Texto != null).Take(2).OrderByDescending(x => x.Data);
            var redesFoto = db.RedesSociais.Where(x => (RedeSocialTipo)x.TipoRede == RedeSocialTipo.Facebook && x.Texto == null).OrderByDescending(x => x.Data).FirstOrDefault();

            var viewModelRedes = new RedesSociaisHomeViewModel
            {
                Facebook = redesFace.ToList(),
                Twitter = redesTwitter.ToList(),
                FotoDestaque = redesFoto
            };

            return viewModelRedes;
        }

        private class ProgramacoesVM
        {
            public string programaId { get; set; }
            public string horario { get; set; }
            public DateTime? Data { get; set; }
        }

    }
}