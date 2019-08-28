using PagedList;
using Site.Helpers;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;


namespace Site.Controllers
{
    public class BuscaController : BaseController
    {
        // GET: Busca 
        [OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "chave;page;palavraChave;dataInicio;dataFinal;editoria")]
        public async Task<ActionResult> Index(int? page, string busca, string dataInicio, string dataFinal, string editoria)
        {
            var viewModel = new BuscaViewModel {
                paginacao = new List<BuscaResultadosModel>().ToPagedList(page ?? 1, 15)
            };

            if (!string.IsNullOrEmpty(busca))
            {
                var hoje = DateTime.Now;

                var resultado = db.Noticias.Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now);

                var banner = await db.Banners.Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-editoria-interna" && w.Ativo.Value)).FirstOrDefaultAsync();

                int pageNumber = (page ?? 1);

                #region busca noticias e afins
                if (!string.IsNullOrEmpty(busca))
                {
                    busca = busca.ToLower();
                    resultado = resultado.Where(n => n.titulo != null && (n.titulo.Contains(busca) || n.texto.Contains(busca))
                    || (n.chamada != null && n.chamada.Contains(busca)));
                }

                if (!string.IsNullOrEmpty(editoria))
                {
                    resultado = resultado.Where(n => n.Editoriais.Any(x => x.chave == editoria));
                }


                if (!string.IsNullOrEmpty(dataInicio))
                {
                    DateTime DataInicio = DateTime.Parse(dataInicio).Date;
                    resultado = resultado.Where(d => d.dataCadastro >= DataInicio);

                }

                if (!string.IsNullOrEmpty(dataFinal))
                {
                    DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
                    resultado = resultado.Where(d => d.dataCadastro <= DataFim);
                }
                #endregion

                #region carnaval blocos
                var blocos = db.Blocos.Where(x => !x.Excluido && x.Ativo);

                if (!string.IsNullOrEmpty(busca))
                {
                    blocos = blocos.Where(n => n.Nome.Contains(busca)
                    || n.Chave != null && n.Chave.Contains(busca)
                    || n.Local != null && n.Local.Contains(busca));
                }

                #endregion

                #region programacao
                var programacao = db.Programacao.Where(x => !x.excluido);

                if (!string.IsNullOrEmpty(busca))
                {
                    programacao = programacao.Where(n => n.nome.Contains(busca));
                }

                var buscaGeral = (resultado.Select(x =>
                new BuscaResultadosModel
                {
                    Id = x.id,
                    Titulo = x.titulo,
                    Chamada = x.chamada,
                    Url = x.url,
                    Data = x.dataCadastro,
                    Control = "noticia",
                    Autor = x.porAutor,
                    Imagem = x.foto,
                    Subtitulo = x.subtitulo
                }).OrderByDescending(x => x.Data).Take(15).ToList()).Union(blocos.Select(x =>
                new BuscaResultadosModel
                {
                    Id = x.Id,
                    Titulo = x.Nome,
                    Chamada = null,
                    Url = "carnaval",
                    Data = x.DataCadastro,
                    Control = "especial",
                    Autor = null,
                    Imagem = x.Imagem,
                    Subtitulo = null
                }).OrderByDescending(x => x.Data).Take(15).ToList()).Union(programacao.Select(x =>
                new BuscaResultadosModel
                {
                    Id = x.id,
                    Titulo = x.nome,
                    Chamada = x.chamada,
                    // Url = (x.Horario_programacao.FirstOrDefault().diaSemana.HasValue ? Utils.WeekDayName((int)x.Horario_programacao.FirstOrDefault().diaSemana.Value) : string.Empty),
                    Url = string.Empty,
                    Data = x.dataCadastro,
                    Control = "programacao",
                    Autor = x.Apresentadores.FirstOrDefault().nome,
                    Subtitulo = null
                }).OrderByDescending(x => x.Data).Take(15).ToList()).OrderByDescending(x => x.Data);

                #endregion

                viewModel.Banner = banner;
                viewModel.paginacao = buscaGeral.ToPagedList(pageNumber, 15);
            }

            ViewBag.editoria = new SelectList(db.Editoriais.Select(x => new { x.chave, x.nome, x.ativo, x.excluido }).Where(x => x.ativo && !x.excluido), "chave", "nome", (!string.IsNullOrEmpty(editoria) ? editoria : null));

            return View(viewModel);

        }





    }
}