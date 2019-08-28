using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;
using PagedList;
using Site.Enums;
using Site.Models;
using Site.Services.Classes;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class ClubesController : BaseController
    {
        public ClubesController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }


        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id;busca;editoria;dataInicio;dataFim;page;categoria")]
        public async Task<ActionResult> Index(string id, string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = id;
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;

            //ViewBag.categoria = new SelectList(db.Categorias.Where(x => !x.Excluido && x.Editoriais.chave == id), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            #endregion
            var hoje = DateTime.Now;
            var queryDestaques = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.idColunista == null && !x.plantao && x.Editoriais.Any(y => y.chave == id && y.ativo && !y.excluido)).OrderByDescending(p => p.dataAtualizacao)
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(ed => new EditorialNoticiaViewModel
                    {
                        Ativo = ed.ativo,
                        Chave = ed.chave,
                        Especial = ed.especial,
                        Esporte = ed.esportes,
                        Excluido = ed.excluido,
                        Nome = ed.nome

                    }),
                    Foto = noticia.foto,
                    FotoCredito = noticia.fotoCredito,
                    Id = noticia.id,
                    Subtitulo = noticia.subtitulo,
                    TipoDestaque = noticia.TipoDestaque,
                    Titulo = noticia.titulo,
                    TituloCapa = noticia.TituloCapa,
                    Url = noticia.url,
                    Audio = noticia.audio,
                    Liberado = noticia.liberado,
                    Excluido = noticia.excluido,
                    IdColunista = noticia.idColunista,
                    Plantao = noticia.plantao,
                    PorAutor = noticia.porAutor
                }).OrderByDescending(y => y.DataCadastro).Take(100);

            var destaques = await DestaquesEditoria(queryDestaques, id, new int[] { });

            var ids1 = new int[] { };
            if (destaques != null && destaques.Any())
            {
                ids1 = destaques.Select(x => x.Id).ToArray();
            }

            var modelo = new ClubesViewModel();


            switch (id)
            {
                case "america":
                    modelo.NomeClube = "América";
                    modelo.ClasseCSS = "green";
                    modelo.ProximoJogo = Site.Services.Cache.ProximoAmerica;
                    break;
                case "cruzeiro":
                    modelo.NomeClube = "Cruzeiro";
                    modelo.ClasseCSS = "blue";
                    modelo.ProximoJogo = Site.Services.Cache.ProximoCruzeiro;
                    break;
                case "brasil":
                    modelo.NomeClube = "Seleção Brasileira";
                    modelo.ClasseCSS = "yellow";
                    modelo.ProximoJogo = Site.Services.Cache.ProximoBrasil;
                    break;
                case "atletico":
                    modelo.NomeClube = "Atlético";
                    modelo.ClasseCSS = "dark";
                    modelo.ProximoJogo = Site.Services.Cache.ProximoAtletico;
                    break;
            }


            modelo.Destaques = destaques;
            var ids = ids1;

            modelo.Banner1 = db.Banners.Include(x => x.AreaBanner).Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-esportes" && w.Ativo.Value)).FirstOrDefault();
            modelo.Banner2 = db.Banners.Include(x => x.AreaBanner).Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-editoria-1" && w.Ativo.Value)).FirstOrDefault();
            modelo.Banner3 = db.Banners.Include(x => x.AreaBanner).Where(x => !x.Excluido && x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim && x.AreaBanner.Any(w => w.chave == "banner-editoria-2" && w.Ativo.Value)).FirstOrDefault();


            modelo.Elenco = db.Elencos.Where(e => e.Times.Chave.Equals(id) && !e.Excluido).ToList();
            modelo.Time = db.Times.Where(e => e.Chave.Equals(id)).FirstOrDefault();

            modelo.Noticias = ObterNoticias(busca, id, dataInicio, dataFim, page, categoria, ids, queryDestaques);

            ViewBag.ids = string.Join(",", ids);
            return View(modelo);
        }

        public Jogo ObterProximoJogo(string nome)
        {
            var jogos = new List<Jogo>();

            #region SerieA

            var seriea = (from fase in Site.Services.Cache.SerieA.Fases
                          from rodada in fase.Rodadas
                          from jogo in rodada.Jogos
                          where jogo.Data >= DateTime.Now && (jogo.Mandante.Nome.Equals(nome) || jogo.Visitante.Nome.Equals(nome))
                          orderby jogo.Data
                          select jogo)
                         .FirstOrDefault();

            if (seriea != null) jogos.Add(seriea);

            #endregion

            #region SerieB

            var serieb = (from fase in Site.Services.Cache.SerieB.Fases
                          from rodada in fase.Rodadas
                          from jogo in rodada.Jogos
                          where jogo.Data >= DateTime.Now && (jogo.Mandante.Nome.Equals(nome) || jogo.Visitante.Nome.Equals(nome))
                          orderby jogo.Data
                          select jogo)
                         .FirstOrDefault();

            if (serieb != null) jogos.Add(serieb);

            #endregion

            #region Mineiro

            var mineiro = (from fase in Site.Services.Cache.Mineiro.Fases
                           from rodada in fase.Rodadas
                           from jogo in rodada.Jogos
                           where jogo.Data >= DateTime.Now && (jogo.Mandante.Nome.Equals(nome) || jogo.Visitante.Nome.Equals(nome))
                           orderby jogo.Data
                           select jogo)
                         .FirstOrDefault();


            if (mineiro != null) jogos.Add(mineiro);

            #endregion

            #region CopadoBrasil

            var copabrasil = (from fase in Site.Services.Cache.CopaDoBrasil.Fases
                              from rodada in fase.Rodadas
                              from jogo in rodada.Jogos
                              where jogo.Data >= DateTime.Now && (jogo.Mandante.Nome.Equals(nome) || jogo.Visitante.Nome.Equals(nome))
                              orderby jogo.Data
                              select jogo)
                         .FirstOrDefault();


            if (copabrasil != null) jogos.Add(copabrasil);

            #endregion

            return jogos.OrderBy(j => j.Data).FirstOrDefault();
        }

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "busca;editoria;dataInicio;dataFim;page;categoria;ids")]
        public ActionResult MostrarMais(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0, string ids = null)
        {
            #region ViewBags

            ViewBag.Busca = busca;
            ViewBag.Editoria = editoria;
            ViewBag.DataInicio = dataInicio;
            ViewBag.DataFim = dataFim;
            ViewBag.Page = page ?? 1;

            ViewBag.categoria = new SelectList(db.Categorias.Include(x => x.Editoriais).Where(x => !x.Excluido && x.Editoriais.chave == editoria), "Id", "Titulo", (categoria > 0 ? categoria.ToString() : ""));

            #endregion

            if (!string.IsNullOrEmpty(ids))
            {
                var idIgnores = ids.Split(new char[] { ',' });
                var idIgnore = Array.ConvertAll(idIgnores, int.Parse);

                var modelo = ObterNoticias(busca, editoria, dataInicio, dataFim, page, categoria, idIgnore);
                return View(modelo);
            }
            else
            {
                var modelo = ObterNoticias(busca, editoria, dataInicio, dataFim, page, categoria);
                return View(modelo);
            }
        }

        public IPagedList<NoticiasViewModel> ObterNoticias(string busca, string editoria, string dataInicio, string dataFim, int? page, int categoria = 0, int[] ids = null, IQueryable<NoticiasViewModel> queryPrincipal = null)
        {
            IQueryable<NoticiasViewModel> dados;

            if (queryPrincipal != null)
            {
                dados = queryPrincipal.Where(n => n.Liberado && !n.Excluido && n.IdColunista == null);
            }
            else
            {
                dados = db.Noticias.Include(x => x.Editoriais).Where(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now && x.idColunista == null && !x.plantao && x.Editoriais.Any(y => y.chave == editoria && y.ativo)).OrderByDescending(p => p.dataAtualizacao)
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(ed => new EditorialNoticiaViewModel
                    {
                        Ativo = ed.ativo,
                        Chave = ed.chave,
                        Especial = ed.especial,
                        Esporte = ed.esportes,
                        Excluido = ed.excluido,
                        Nome = ed.nome

                    }),
                    PorAutor = noticia.porAutor,
                    VideoYoutube = noticia.videoYoutube,
                    Visualizacao = noticia.visualizacao,
                    Foto = noticia.foto,
                    FotoCredito = noticia.fotoCredito,
                    Id = noticia.id,
                    Subtitulo = noticia.subtitulo,
                    TipoDestaque = noticia.TipoDestaque,
                    Titulo = noticia.titulo,
                    TituloCapa = noticia.TituloCapa,
                    Url = noticia.url,
                    Audio = noticia.audio,
                    Liberado = noticia.liberado,
                    Excluido = noticia.excluido,
                    IdColunista = noticia.idColunista,
                    Plantao = noticia.plantao
                }).Take(100);
            }
            //var dados = db.Noticias.Where(n => n.liberado && !n.excluido && n.idColunista == null);}

            dados = dados.OrderByDescending(y => y.DataCadastro);

            #region Filtros
            if (!string.IsNullOrEmpty(busca))
            {
                dados = dados.Where(n => n.Chamada.Contains(busca));
            }

            if (ids != null && ids.Length > 0)
            {
                dados = dados.Where(n => !ids.Contains(n.Id));
            }

            if (!string.IsNullOrEmpty(editoria))
            {
                dados = dados.Where(n => n.Editoriais.Any(e => e.Ativo && e.Chave.Equals(editoria)));
            }
            if (!string.IsNullOrEmpty(dataInicio))
            {
                var data = DateTime.Parse(dataInicio);
                dados = dados.Where(n => n.DataCadastro >= data);
            }

            //if (categoria > 0)
            //{
            //    dados = dados.Where(n => n.categoria.Any(x => x.Id == categoria));
            //}

            if (!string.IsNullOrEmpty(dataFim))
            {
                var data = DateTime.Parse(dataFim);
                dados = dados.Where(n => n.DataCadastro <= data);
            }
            #endregion

            var lista = dados.OrderByDescending(n => n.DataCadastro).ToPagedList(page ?? 1, 2);
            return lista;
        }

        public async Task<List<NoticiasViewModel>> DestaquesEditoria(IQueryable<NoticiasViewModel> queryPrincipal, string chave, IEnumerable<int> idDestaques)
        {
            var destaques = new List<NoticiasViewModel>();

            //queryPrincipal = queryPrincipal.Where(noticia => !idDestaques.Contains(noticia.Id));


            queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Any(p => !p.Excluido && p.Ativo && p.Chave == chave));

            queryPrincipal = queryPrincipal.OrderByDescending(n => n.DataCadastro).Take(30);
            var skip = 0;

            var destaquesId = new int[] { };

            var destaque1 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna1);
            var destaque2 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna2);
            var destaque3 = await queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefaultAsync(noticia => noticia.TipoDestaque == TipoDestaque.Interna3);

            destaques.Add(destaque1);
            destaques.Add(destaque2);
            destaques.Add(destaque3);

            destaques.RemoveAll(destaque => destaque == null);

            destaquesId = destaques.Select(destaque => destaque.Id).ToArray();

            destaques.Clear();

            if (destaque1 == null)
            {
                destaque1 = await queryPrincipal.Where(x => x.Foto != null && !x.Excluido && x.Liberado && !destaquesId.Any(y => x.Id == y)).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            if (destaque2 == null)
            {
                destaque2 = await queryPrincipal.Where(x => x.Foto != null && !x.Excluido && x.Liberado && !destaquesId.Any(y => x.Id == y)).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            if (destaque3 == null)
            {
                destaque3 = await queryPrincipal.Where(x => x.Foto != null && !x.Excluido && x.Liberado && !destaquesId.Any(y => x.Id == y)).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefaultAsync();
                skip++;
            }

            destaques.Add(destaque1);
            destaques.Add(destaque2);
            destaques.Add(destaque3);
            destaques.RemoveAll(destaque => destaque == null);


            if (destaques == null && !destaques.Any())
            {
                destaquesId = destaques.Select(destaque => destaque.Id).ToArray();
                destaques.AddRange(await queryPrincipal.Where(destaque => (destaque.TipoDestaque == null || destaque.TipoDestaque == TipoDestaque.NenhumDestaque)
                    && !destaquesId.Contains(destaque.Id)).OrderByDescending(y => y.DataCadastro).Take(3).ToListAsync());
            }


            return destaques;

        }
    }
}