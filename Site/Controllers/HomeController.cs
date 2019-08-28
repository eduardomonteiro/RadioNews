using System.Web.Mvc;
using Site.Models;
using Site.ViewModels;
using System.Linq;
using Site.Enums;
using System.Threading.Tasks;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Site.Services;
using Site.Controllers;

namespace API.Controllers
{
    public class HomeController : BaseController
    {
        string primeKey = "home:";

        [OutputCache(Duration = 10, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            return View(CachedIndex());
        }
        public PartialViewResult PartialIndex()
        {
            HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            return PartialView(CachedIndex());
        }
        private HomeViewModel CachedIndex()
        {
            string key = primeKey + "Index:TNoticias:TEditoriais";

            HomeViewModel retorno = null;

            Func<object, HomeViewModel> funcao = t => IndexDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 10);

            var bannersGeneral = BannersCached();

            retorno.Banner1 = bannersGeneral.Where(x => (x.Chave == "banner-home-1" || x.Chave == "home-maxiboard-pos-2") && x.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.BannerMobile = bannersGeneral.Where(x => x.Chave == "banner-home-mobile" && x.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.Banner2 = bannersGeneral.Where(x => x.Chave == "banner-home-2" && x.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.BannerMobilePrincipal = bannersGeneral.Where(x => x.Chave == "banner-home-mobile-principal" && x.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.BannersPrincipal = bannersGeneral.Where(x => (x.Chave == "banner-home-leaderboard" || x.Chave == "banner-home-super") && x.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            return retorno;
        }

        private List<BannerHomeViewModel> BannersCached()
        {
            string keyBanner = primeKey + "Index:Banners:TBanners";

            Func<object, List<BannerHomeViewModel>> funcao = t => BannersHomeDB();
            return RedisService.GetOrSetToRedis(keyBanner, funcao, 600);
        }

        private List<BannerHomeViewModel> BannersHomeDB()
        {
            var hoje = DateTime.Now;
            var bannersGeneral = db.Banners.Where(x => x.Liberado && !x.Excluido && hoje >= x.DataInicio && hoje <= x.DataFim)
                .Select(banner => new BannerHomeViewModel
                {
                    Arquivos = banner.Arquivo,
                    Id = banner.Id,
                    Link = banner.Link,
                    Titulo = banner.Titulo,
                    Chave = banner.AreaBanner.FirstOrDefault().chave,
                    Anunciante = banner.Anunciante,
                    AreaBannerDescricao = banner.AreaBanner.FirstOrDefault().Descricao,
                    AreaBannerTamanho = banner.AreaBanner.FirstOrDefault().Tamanho,
                    AreaBannerAtivo = banner.AreaBanner.FirstOrDefault().Ativo.Value
                }).ToList();
            return bannersGeneral;
        }

        private HomeViewModel IndexDB()
        {
            var hoje = DateTime.Now;
            #region DEclaração de performance
            var ultimos7 = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0).AddDays(-7);
            var noticiasGeneral = db.Noticias.Where(x =>
            !x.excluido
            && x.Colunista == null
            && x.dataCadastro >= ultimos7
            && x.liberado
            && x.Editoriais.Any(e => e.ativo && !e.excluido))
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(editorial => new EditorialNoticiaViewModel
                    {
                        Ativo = editorial.ativo,
                        Chave = editorial.chave,
                        Especial = editorial.especial,
                        Esporte = editorial.esportes,
                        Excluido = editorial.excluido,
                        Nome = editorial.nome

                    }).Take(1),
                    Foto = noticia.foto,
                    Id = noticia.id,
                    TipoDestaque = noticia.TipoDestaque,
                    Titulo = noticia.titulo,
                    TituloCapa = noticia.TituloCapa,
                    Url = noticia.url,
                    Audio = noticia.audio,
                    Liberado = noticia.liberado,
                    Excluido = noticia.excluido,
                    IdColunista = noticia.idColunista,
                    Plantao = noticia.plantao
                }).OrderByDescending(y => y.DataCadastro).Take(200).AsNoTracking().ToList();

            #endregion

            #region Gerando Destaques
            var destaques = new List<NoticiasViewModel>();
            var destaqueQuery = noticiasGeneral.Where(noticia => noticia.Foto != null);
            var skip = 0;

            var destaque1 = destaqueQuery
                .FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Normal1);
            if (destaque1 == null)
            {
                destaque1 = destaqueQuery.Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).Skip(skip).FirstOrDefault();
                skip++;
            }

            var destaque2 = destaqueQuery.FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Normal2);
            if (destaque2 == null)
            {
                destaque2 = destaqueQuery.Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).Skip(skip)
                    .FirstOrDefault();
                skip++;
            }

            var destaque3 = destaqueQuery.FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Normal3);
            if (destaque3 == null)
            {
                destaque3 = destaqueQuery
                    .Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).Skip(skip).FirstOrDefault();
                skip++;
            }

            destaques.Add(destaque1);
            destaques.Add(destaque2);
            destaques.Add(destaque3);

            destaques.RemoveAll(destaque => destaque == null);

            var idDestaques = destaques.Select(destaque => destaque.Id).ToArray();

            #endregion

            var destaqueEspecial = noticiasGeneral.OrderByDescending(y => y.DataCadastro)
                .Where(x => x.Liberado &&
                !x.Excluido
                && x.DataCadastro == hoje
                && x.Foto != null
                && (x.TipoDestaque == TipoDestaque.Urgente360 || x.TipoDestaque == TipoDestaque.Urgente1130)
                && !x.Plantao && x.IdColunista == null).Skip(skip).FirstOrDefault();

            var podcast = noticiasGeneral.Where(x => x.Audio != null &&
           x.Editoriais.Any(p => !p.Excluido && p.Ativo)).OrderByDescending(y => y.DataCadastro).Take(9).ToList();

            var editoriaEsportes = DestaquesEditoria(noticiasGeneral, "esporte", idDestaques);

            var editoriaCidade = DestaquesEditoria(noticiasGeneral, "cidades", idDestaques);

            var editoriaEntretenimento = DestaquesEditoria(noticiasGeneral, "entretenimento", idDestaques);

            var editoriaPolitica = DestaquesEditoria(noticiasGeneral, "politica", idDestaques);

            var noticias = DestaquesEditoria(noticiasGeneral, "noticias", idDestaques);

            var homeViewModel = new HomeViewModel
            {
                Destaques = destaques,
                DestaqueEspecial = destaqueEspecial,
                EditoriaCidade = editoriaCidade,
                EditoriaEntretenimento = editoriaEntretenimento,
                EditoriaPolitica = editoriaPolitica,
                EditoriaEsportes = editoriaEsportes,
                Noticias = noticias,
                Podcast = podcast
            };

            return homeViewModel;
        }

        public List<NoticiasViewModel> DestaquesEditoria(List<NoticiasViewModel> queryPrincipal, string chave, IEnumerable<int> idDestaques)
        {
            var destaques = new List<NoticiasViewModel>();

            queryPrincipal = queryPrincipal.Where(noticia => !idDestaques.Contains(noticia.Id)).ToList();

            if (chave == "esporte")
            {
                queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Any(p => !p.Excluido && !p.Especial && p.Ativo && (p.Chave == chave || p.Esporte))).ToList();
            }
            //else if (chave == "noticia")
            //{
            //    queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Take(1)
            //        .Any(p => !p.Excluido
            //            && p.Ativo
            //            && p.Chave != "politica"
            //            && p.Chave != "entretenimento"
            //            && p.Chave != "cidade"
            //            && p.Chave != "esporte"
            //            && p.Chave != "parlamento-aberto"
            //            && !p.Esporte));
            //}
            else
            {
                queryPrincipal = queryPrincipal.Where(x => x.Editoriais.Any(p => !p.Excluido && p.Ativo && p.Chave == chave)).ToList();
            }

            queryPrincipal = queryPrincipal.Take(30).ToList();
            var skip = 0;

            var destaque1 = queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Interna1);
            if (destaque1 == null)
            {
                destaque1 = queryPrincipal.Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefault();
                skip++;
            }

            var destaque2 = queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Interna2);
            if (destaque2 == null)
            {
                destaque2 = queryPrincipal.Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefault();
                skip++;
            }

            var destaque3 = queryPrincipal.OrderByDescending(y => y.DataCadastro).FirstOrDefault(noticia => noticia.TipoDestaque == TipoDestaque.Interna3);
            if (destaque3 == null)
            {
                destaque3 = queryPrincipal.Where(noticia => noticia.TipoDestaque == null || noticia.TipoDestaque == TipoDestaque.NenhumDestaque).OrderByDescending(y => y.DataCadastro).Skip(skip).FirstOrDefault();
                skip++;
            }

            destaques.Add(destaque1);
            destaques.Add(destaque2);
            destaques.Add(destaque3);

            destaques.RemoveAll(destaque => destaque == null);

            var destaquesId = new int[] { };

            if (destaques != null && destaques.Any())
            {
                destaquesId = destaques.Select(destaque => destaque.Id).ToArray();
                destaques.AddRange(queryPrincipal.Where(destaque => (destaque.TipoDestaque == null || destaque.TipoDestaque == TipoDestaque.NenhumDestaque)
                   && !destaquesId.Contains(destaque.Id)).OrderByDescending(y => y.DataCadastro).Take(chave == "esporte" ? 6 : destaques.Count).ToList());
            }
            else
            {
                destaques = null;
            }


            return destaques;

        }

        public PartialViewResult SidebarHome()
        {
            string key = primeKey + "SidebarHome:TNoticias:TEditoriais:TColunista:TEnquete";

            SideBarHomeViewModel retorno = null;

            Func<object, SideBarHomeViewModel> funcao = t => SidebarHomeDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 300);

            var bannersGeneral = BannersCached();

            retorno.Banner = bannersGeneral
                .Where(x => x.Chave == "banner-editoria-1" && x.AreaBannerAtivo.Value)
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.BannerUltimoSidebar = bannersGeneral
                .Where(x => x.Chave == "banner-ultimo-sidebar" && x.AreaBannerAtivo.Value)
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            return PartialView("_SidebarHome", retorno);
        }

        private SideBarHomeViewModel SidebarHomeDB()
        {
            var maisLidas = new List<NoticiasViewModel>();

            var hoje = DateTime.Now;
            var ultimos7 = DateTime.Now.Date.AddDays(-7);

            #region DEclaração de performance
            var noticiasGeneral = db.Noticias.Where(x => !x.excluido && x.liberado && x.dataCadastro >= ultimos7 && x.Editoriais.Any(e => e.ativo && !e.excluido))
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(editorial => new EditorialNoticiaViewModel
                    {
                        Ativo = editorial.ativo,
                        Chave = editorial.chave,
                        Especial = editorial.especial,
                        Esporte = editorial.esportes,
                        Excluido = editorial.excluido,
                        Nome = editorial.nome

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
                    Visualizacao = noticia.visualizacao,
                    VideoYoutube = noticia.videoYoutube,
                    Galeria = noticia.Galeria
                }).OrderByDescending(y => y.DataCadastro).Take(40).ToList();

            #endregion

            var plantao = noticiasGeneral
                .Where(x => x.Plantao && x.DataCadastro >= hoje && x.Editoriais.Any(p => p.Especial == false))
                .OrderByDescending(x => x.DataCadastro)
                .Select(noticia => new PlantaoViewModel { Titulo = noticia.Titulo, Hora = noticia.DataCadastro })
                .ToList();

            //if (maisLidas.Count == 0)
            //{
            //    maisLidas = noticiasGeneral
            //        .Where(x => !x.Plantao && x.IdColunista == null && x.Editoriais.Any(p => p.Especial == false))
            //        .OrderByDescending(x => x.Visualizacao)
            //        .Take(5)
            //        .ToList();
            //}
            //var video =
            //    noticiasGeneral.Select(
            //        n =>
            //            n.Galeria == null ? null : n.Galeria.Midias.FirstOrDefault(
            //                m => !m.excluido
            //                && m.video
            //                && !m.Galeria.excluido
            //                && m.Galeria.ativo))
            //                .FirstOrDefault();

            var colteste = db.Colunista
                .Where(x => !x.excluido && x.liberado && !x.nome.Equals("Cadu Doné") && !x.nome.Equals("Emanuel Carneiro"))
                .Select(c => new ColunistaViewModel
                {
                    Chave = c.chave,
                    Foto = c.foto,
                    Nome = c.nome,
                    Titulo = c.Noticias
                        .OrderByDescending(noticia => noticia.dataAtualizacao)
                        .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).titulo,
                    Url = c.Noticias
                        .OrderByDescending(noticia => noticia.dataAtualizacao)
                        .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).url,
                    UltimaAtualizacao = c.Noticias.Count > 0 ?
                    c.Noticias.OrderByDescending(noticia => noticia.dataAtualizacao)
                    .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).dataAtualizacao
                    : DateTime.MinValue
                })
                .OrderByDescending(x => x.UltimaAtualizacao)
                .ToList();
            var colunista = new ObservableCollection<ColunistaViewModel>(colteste);
            if (colunista.Where(e => e.Nome.Equals("Cadu Doné")).FirstOrDefault() == null)
            {
                if (db.Colunista.Where(x => !x.excluido && x.liberado && x.nome.Equals("Cadu Doné")).FirstOrDefault() != null)
                {
                    colunista.Add(db.Colunista
                   .Where(x => !x.excluido && x.liberado && x.nome.Equals("Cadu Doné"))
                   .OrderBy(x => x.Ordem)
                   .Select(c => new ColunistaViewModel
                   {
                       Chave = c.chave,
                       Foto = c.foto,
                       Nome = c.nome,
                       Titulo = c.Noticias
                           .OrderByDescending(noticia => noticia.dataAtualizacao)
                           .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).titulo,
                       Url = c.Noticias
                           .OrderByDescending(noticia => noticia.dataAtualizacao)
                           .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).url,
                   }).FirstOrDefault());
                    colunista.Move(colunista.IndexOf(colunista.Where(e => e.Nome.Equals("Cadu Doné")).FirstOrDefault()), 0);
                }
            }
            if (colunista.Where(e => e.Nome.Equals("Emanuel Carneiro")).FirstOrDefault() == null)
            {
                if (db.Colunista.Where(x => !x.excluido && x.liberado && x.nome.Equals("Emanuel Carneiro")).FirstOrDefault() != null)
                {
                    colunista.Add(db.Colunista
                   .Where(x => !x.excluido && x.liberado && x.nome.Equals("Emanuel Carneiro"))
                   .OrderBy(x => x.Ordem)
                   .Select(c => new ColunistaViewModel
                   {
                       Chave = c.chave,
                       Foto = c.foto,
                       Nome = c.nome,
                       Titulo = c.Noticias
                           .OrderByDescending(noticia => noticia.dataAtualizacao)
                           .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).titulo,
                       Url = c.Noticias
                           .OrderByDescending(noticia => noticia.dataAtualizacao)
                           .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).url,
                   }).FirstOrDefault());
                    colunista.Move(colunista.IndexOf(colunista.Where(e => e.Nome.Equals("Emanuel Carneiro")).FirstOrDefault()), 0);
                }
            }

            var enquete = db.Enquete
                .Where(x => !x.excluido && x.ativa && hoje >= x.dataInicioVotacao && hoje <= x.dataFimVotacao)
                .Select(e => new EnqueteViewModel {
                    pergunta = e.pergunta,
                    Respostas = e.Respostas.Select(r => new RespostaEnqueteViewModel {
                        id = r.id,
                        resposta = r.resposta,
                        votos = r.votos
                    }).ToList()
                }).FirstOrDefault();

            var viewModel = new SideBarHomeViewModel
            {
                Colunistas = colunista.ToList(),
                Enquete = enquete,
                PlantaoBH = plantao
            };

            return viewModel;
        }

        public PartialViewResult Sidebar()
        {
            string key = primeKey + "Sidebar:TNoticias:TEditoriais:TColunista";

            SideBarHomeViewModel retorno = null;

            Func<object, SideBarHomeViewModel> funcao = t => SidebarDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 300);

            var bannersGeneral = BannersCached();

            retorno.Banner = bannersGeneral
                .Where(x => x.Chave == "banner-editoria-1" && x.AreaBannerAtivo.Value)
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault();
            retorno.Banner2 = bannersGeneral
                .Where(x => x.Chave == "banner-editoria-2" && x.AreaBannerAtivo.Value)
                .OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            return PartialView("_Sidebar", retorno);
        }
        private SideBarHomeViewModel SidebarDB()
        {
            db.Configuration.LazyLoadingEnabled = false;

            var maisLidas = new List<NoticiasViewModel>();

            var hoje = DateTime.Now;
            var ultimos7 = DateTime.Now.Date.AddDays(-7);

            var noticiasGeneral = db.Noticias.Include(x => x.Editoriais).Where(x =>
              !x.excluido
              && x.Colunista == null
              && x.dataCadastro >= ultimos7
              && x.liberado
              && x.Editoriais.Any(e => e.ativo && !e.excluido))
                .Select(noticia => new NoticiasViewModel
                {
                    Chamada = noticia.chamada,
                    DataCadastro = noticia.dataCadastro,
                    Editoriais = noticia.Editoriais.Select(editorial => new EditorialNoticiaViewModel
                    {
                        Ativo = editorial.ativo,
                        Chave = editorial.chave,
                        Especial = editorial.especial,
                        Esporte = editorial.esportes,
                        Excluido = editorial.excluido,
                        Nome = editorial.nome

                    }).Take(1),
                    Foto = noticia.foto,
                    Id = noticia.id,
                    TipoDestaque = noticia.TipoDestaque,
                    Titulo = noticia.titulo,
                    TituloCapa = noticia.TituloCapa,
                    Url = noticia.url,
                    Audio = noticia.audio,
                    Liberado = noticia.liberado,
                    Excluido = noticia.excluido,
                    IdColunista = noticia.idColunista,
                    Plantao = noticia.plantao
                }).OrderByDescending(y => y.DataCadastro).Take(100).AsNoTracking().ToList();

            //maisLidas = noticiasGeneral.Where(x => x.DataCadastro >= ultimos7).OrderByDescending(x => x.Visualizacao).Take(5).ToList();

            //if (maisLidas.Count == 0)
            //{
            //    maisLidas = noticiasGeneral.OrderByDescending(x => x.Visualizacao).Take(5).ToList();
            //}

            var colteste = db.Colunista
                .Where(x => !x.excluido && x.liberado && !x.nome.Equals("Cadu Doné") && !x.nome.Equals("Emanuel Carneiro"))
                .Select(c => new ColunistaViewModel
                {
                    Chave = c.chave,
                    Foto = c.foto,
                    Nome = c.nome,
                    Titulo = c.Noticias
                        .OrderByDescending(noticia => noticia.dataCadastro)
                        .FirstOrDefault(x => !x.excluido && x.liberado).titulo,
                    Url = c.Noticias
                        .OrderByDescending(noticia => noticia.dataCadastro)
                        .FirstOrDefault(x => !x.excluido && x.liberado).url,
                    UltimaAtualizacao = c.Noticias.Count > 0 ?
                    c.Noticias.OrderByDescending(noticia => noticia.dataAtualizacao)
                    .FirstOrDefault(x => !x.excluido && x.liberado && x.dataAtualizacao < DateTime.Now).dataAtualizacao
                    : DateTime.MinValue
                })
                .OrderByDescending(x => x.UltimaAtualizacao)
                .ToList();
            var colunista = new ObservableCollection<ColunistaViewModel>(colteste);
            if (colunista.Where(e => e.Nome.Equals("Cadu Doné")).FirstOrDefault() == null)
            {
                if (db.Colunista.Where(x => !x.excluido && x.liberado && x.nome.Equals("Cadu Doné")).FirstOrDefault() != null)
                {
                    colunista.Add(db.Colunista
                   .Where(x => !x.excluido && x.liberado && x.nome.Equals("Cadu Doné"))
                   .OrderBy(x => x.Ordem)
                   .Select(c => new ColunistaViewModel
                   {
                       Chave = c.chave,
                       Foto = c.foto,
                       Nome = c.nome,
                       Titulo = c.Noticias
                           .OrderByDescending(noticia => noticia.dataCadastro)
                           .FirstOrDefault(x => !x.excluido && x.liberado).titulo,
                       Url = c.Noticias
                           .OrderByDescending(noticia => noticia.dataCadastro)
                           .FirstOrDefault(x => !x.excluido && x.liberado).url,
                   }).FirstOrDefault());
                    colunista.Move(colunista.IndexOf(colunista.Where(e => e.Nome.Equals("Cadu Doné")).FirstOrDefault()), 0);
                }
            }
            if (colunista.Where(e => e.Nome.Equals("Emanuel Carneiro")).FirstOrDefault() == null)
            {
                if (db.Colunista.Where(x => !x.excluido && x.liberado && x.nome.Equals("Emanuel Carneiro")).FirstOrDefault() != null)
                {
                    colunista.Add(db.Colunista
                   .Where(x => !x.excluido && x.liberado && x.nome.Equals("Emanuel Carneiro"))
                   .OrderBy(x => x.Ordem)
                   .Select(c => new ColunistaViewModel
                   {
                       Chave = c.chave,
                       Foto = c.foto,
                       Nome = c.nome,
                       Titulo = c.Noticias
                           .OrderByDescending(noticia => noticia.dataCadastro)
                           .FirstOrDefault(x => !x.excluido && x.liberado).titulo,
                       Url = c.Noticias
                           .OrderByDescending(noticia => noticia.dataCadastro)
                           .FirstOrDefault(x => !x.excluido && x.liberado).url,
                   }).FirstOrDefault());
                    colunista.Move(colunista.IndexOf(colunista.Where(e => e.Nome.Equals("Emanuel Carneiro")).FirstOrDefault()), 0);
                }
            }

            var podcastViewModel = db.Noticias.Include(x => x.Editoriais)
                .Where(x => !x.excluido && x.liberado && !x.destaqueHome && x.audio != null &&
                x.Editoriais.Select(editorial => new {
                    excluido = editorial.excluido, ativo = editorial.ativo
                }).Any(p => !p.excluido && p.ativo))
                .Select(podcast => new PodcastViewModel {
                    Audio = podcast.audio,
                    DataCadastro = podcast.dataAtualizacao,
                    Id = podcast.id,
                    Nome = podcast.Editoriais.FirstOrDefault() != null ? podcast.Editoriais.FirstOrDefault().nome : string.Empty,
                    Titulo = podcast.titulo
                }).OrderByDescending(y => y.DataCadastro).Take(3).ToList();

            SideBarHomeViewModel viewModel = new SideBarHomeViewModel
            {
                Colunistas = colunista.ToList(),
                Podcast = podcastViewModel
            };

            return viewModel;
        }

        [HttpPost]
        public JsonResult Votar(string option)
        {
            int resp = Convert.ToInt32(option);

            var resposta = db.Respostas.Find(resp);
            resposta.votos += 1;

            try
            {
                db.Entry(resposta).State = EntityState.Modified;
                db.SaveChanges();
                return Json(new { data = true });
            }
            catch
            {
                return Json(new { data = false });
            }

        }

        [OutputCache(Duration = 600)]
        public ActionResult Contact()
        {
            return View();
        }

        [OutputCache(Duration = 60)]
        public ActionResult Player(string chave)
        {
            string key = primeKey + "Player:" + chave;

            RadiosViewModel retorno = null;


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

            ViewBag.programacao = db.Programacao.Where(x => x.id == convertId).Select(a => new ProgramacaoMenuViewModel { Id = a.id, Horario = atual.horario, FimHorario = hf, Nome = a.nome }).ToList();
            ViewBag.radios = db.Radios.Where(x => x.Ativo).OrderBy(x => x.Titulo).Select(r => new RadiosMenuViewModel { Chave = r.Chave, Titulo = r.Titulo, Stream1 = r.Stream1, Stream2 = r.Stream2 }).ToList();

            Func<object, RadiosViewModel> funcao = t => PlayerDB(chave);
            retorno = RedisService.GetOrSetToRedis(key, funcao, 60);


            return View(retorno);
        }

        [OutputCache(Duration = 600)]
        public async Task<JsonResult> RadioJsonAsync(string chave)
        {
            return Json(new { data = await db.Radios.FirstOrDefaultAsync(e => e.Chave == chave) }, JsonRequestBehavior.AllowGet);
        }

        private RadiosViewModel PlayerDB(string chave)
        {
            var retorno = db.Radios.Where(x => x.Chave == chave).Select(r => new RadiosViewModel {
                Titulo = r.Titulo,
                Stream1 = r.Stream1,
                Stream2 = r.Stream2,
                Chave = r.Chave
            }).FirstOrDefault();
            if (retorno == null)
            {
                retorno = db.Radios.Where(x => x.Chave == "belo-horizonte").Select(r => new RadiosViewModel
                {
                    Titulo = r.Titulo,
                    Stream1 = r.Stream1,
                    Stream2 = r.Stream2,
                    Chave = r.Chave
                }).FirstOrDefault();
            }
            return retorno;
        }
        public class ProgramacoesVM
        {
            public string programaId { get; set; }
            public string horario { get; set; }
            public DateTime? Data { get; set; }
        }


        public JsonResult ProgramacaoRadioMenu()
        {
            HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            string key = primeKey + "ProgramacaoRadioMenu:TProgramacao:THorario_programacao";

            List<ProgramacaoMenuViewModel> retorno = null;

            Func<object, List<ProgramacaoMenuViewModel>> funcao = t => ProgramacaoRadioMenuDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 10);

            return Json(
                new
                {
                    data = retorno
                }, JsonRequestBehavior.AllowGet);
        }

        public List<ProgramacaoMenuViewModel> ProgramacaoRadioMenuDB()
        {
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

            return programaAtual;
        }
    }
}