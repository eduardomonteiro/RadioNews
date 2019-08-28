using Site.ViewModels.SeuBolso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Site.Services;
using Site.Helpers;
using Site.ViewModels;
using Site.Models;
using System.Threading.Tasks;
using PagedList;

namespace Site.Controllers
{
    public class SeuBolsoController : BaseController
    {

        private const string CHAVE_EDITAL = "banco-inter";

        public SeuBolsoController()
        {
           // db.Configuration.LazyLoadingEnabled = false;
        }


        // GET: SeuBolso
        string primeKey = "seubolso:";
        public ActionResult Index(int page = 1, string palavraChave = null, string dataInicio = null, string dataFinal = null)
        {
            //Iniciando variáveis
            var retorno = new SeuBolsoViewModel();

            //Carregando o VM com os dados das rotinas que vão no banco e no Redis
            retorno.Banners = BannersCached();
            retorno.Noticias = RetornaNoticiasCached(palavraChave, page, dataInicio, dataFinal).ToPagedList(page,9);
            retorno.Indicadores = RetornaIndicadores();

            return View(retorno);
        }
        
        private IndicadoresViewModel RetornaIndicadores() {
            string keyBanner = primeKey + "Index:Indicadores:TIndicadoresBovespa:TIndicadorFinanceiro";

            Func<object, IndicadoresViewModel> funcao = t => RetornaIndicadoresCached();
            return RedisService.GetOrSetToRedis(keyBanner, funcao, 30);
        }
        private IndicadoresViewModel RetornaIndicadoresCached() {
            return new IndicadoresViewModel {
                Bovespa = db.IndicadoresBovespa.FirstOrDefaultAsync().Result,
                Financeiros = db.IndicadoresFinanceiros.Where(x => x.Liberado).ToListAsync().Result
            };
        }

        private List<NoticiasViewModel> RetornaNoticiasCached(string text,int page, string dataInicio = null, string dataFinal = null) 
        {
            string keyBanner = primeKey + "Index:Noticias:TNoticias:text=" + text + ":page=" + page + ":dtInicio=" + dataInicio + ":dtFim=" + dataFinal;

            Func<object, List<NoticiasViewModel>> funcao = t => RetornaNoticias(text, page, dataInicio, dataFinal);
            return RedisService.GetOrSetToRedis(keyBanner, funcao, 30);
        }
        private List<NoticiasViewModel> RetornaNoticias(string text,int page, string dataInicio = null, string dataFinal = null)
        {

            DateTime? dtInicio = null;
            DateTime? dtFim = null;

            if (!String.IsNullOrEmpty(dataInicio))
            {
                dtInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
            }
            if (!String.IsNullOrEmpty(dataFinal))
            {
                dtFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
            }

            var n = db.Noticias.Where(x => x.liberado &&
                                            !x.excluido &&
                                            x.dataAtualizacao < DateTime.Now &&
                                            x.Editoriais.Any(e => e.chave == CHAVE_EDITAL) &&
                                            x.idColunista == null &&
                                            (
                                                (text == null || (
                                                                    (x.titulo != null && x.titulo.Contains(text.ToLower())) ||
                                                                    (x.chamada != null && x.chamada.Contains(text)) ||
                                                                    (x.subtitulo != null && x.subtitulo.Contains(text))
                                                                )) &&
                                                (dtInicio == null || x.dataCadastro >= dtInicio) &&
                                                (dtFim == null || x.dataCadastro <= dtFim)
                                            )
                                        )
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
                    PorAutor = noticia.porAutor,
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
            
            return n;
        }

        private List<BannerSeuBolsoViewModel> BannersCached()
        {
            string keyBanner = primeKey + "Index:Banners:TBanners";

            Func<object, List<BannerSeuBolsoViewModel>> funcao = t => BannersSeuBolsoDB();
            return RedisService.GetOrSetToRedis(keyBanner, funcao, 600);
        }

        private List<BannerSeuBolsoViewModel> BannersSeuBolsoDB()
        {
            // esta pegando todos os banners como a home, fazer filtro futuramente
            // também fazer na home

            var hoje = DateTime.Now;
            var bannersGeneral = db.Banners.Where(x => x.Liberado && !x.Excluido && hoje >= x.DataInicio && hoje <= x.DataFim)
                .Select(banner => new BannerSeuBolsoViewModel
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
    }
}