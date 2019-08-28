using PagedList;
using Recaptcha.Web;
using Recaptcha.Web.Mvc;
using Site.Services;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class NoticiasController : BaseController
    {
        private string primeKey = "Noticias:";
        // GET: Noticias
        //[OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "page;editoria;palavraChave;dataInicio;dataFinal")]
        public async Task<ActionResult> Index(int? page, string editoria, string palavraChave, string dataInicio, string dataFinal)
        {

            return RedirectToAction("Index", "Editorias", new {chave = "noticias"});
            //#region Declaração de performance
            //var hoje = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
            //var noticiasGeneral = db.Noticias.Where(x => !x.excluido && x.liberado &&
            //x.Editoriais.Any(y => y.ativo && !y.excluido && !y.especial && !y.esportes) && !x.plantao && x.idColunista == null).OrderByDescending(p => p.dataAtualizacao).Take(5000);
            //var bannersGeneral = db.Banners.Where(x => x.Liberado && hoje >= x.DataInicio && hoje <= x.DataFim);
            //int pageNumber = (page ?? 1);
            //#endregion

            //var noticiasBanners = await noticiasGeneral.Where(x => !x.plantao && x.idColunista == null && x.foto != null && x.destaqueEditoria && x.Editoriais.Any(p => !p.excluido && p.ativo && !p.especial)).OrderByDescending(y => y.dataAtualizacao).ToListAsync();

            //var destaques = await noticiasGeneral.Where(x => !x.destaqueEditoria && x.foto != null)
            //.OrderByDescending(p => p.dataAtualizacao)
            //.Take(3)
            //.ToListAsync();
            //var ids = destaques.Select(x => x.id).ToArray();


            //var banner = await bannersGeneral.Where(x => x.AreaBanner.Any(w => w.chave == "banner-editoria-interna" && w.Ativo.Value)).FirstOrDefaultAsync();

            //var noticiasLista = noticiasGeneral.Where(x => !x.destaqueEditoria && !ids.Contains(x.id)).OrderByDescending(y => y.dataAtualizacao).AsQueryable();

            //if (!string.IsNullOrEmpty(palavraChave))
            //{
            //    noticiasLista = noticiasLista.Where(n => n.titulo.Contains(palavraChave) || n.chamada.Contains(palavraChave) || n.subtitulo.Contains(palavraChave));
            //}

            //if (!string.IsNullOrEmpty(editoria))
            //{
            //    noticiasLista = noticiasLista.Where(n => n.Editoriais.Any(p => p.chave == editoria));
            //}

            //if (noticiasLista == null)
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            //if (!string.IsNullOrEmpty(dataInicio))
            //{
            //    DateTime DataInicio = DateTime.Parse(dataInicio).AddHours(0).AddMinutes(0).AddSeconds(0);
            //    noticiasLista = noticiasLista.Where(d => d.dataCadastro >= DataInicio);
            //}

            //if (!string.IsNullOrEmpty(dataFinal))
            //{
            //    DateTime DataFim = DateTime.Parse(dataFinal).AddHours(23).AddMinutes(59).AddSeconds(59);
            //    noticiasLista = noticiasLista.Where(d => d.dataCadastro <= DataFim);                
            //}
            //var viewModel = new NoticiaViewModel
            //{
            //    DestaquesBanners = noticiasBanners,
            //    Destaques = destaques,
            //    Banner = banner,
            //    paginacao = noticiasLista.ToList().ToPagedList(pageNumber, 8)
            //};


            //ViewBag.editoria = new SelectList(db.Editoriais.Where(x => !x.excluido && x.ativo && !x.especial && !x.esportes), "chave", "nome", (!string.IsNullOrEmpty(editoria) ? editoria : ""));

            //return View(viewModel);

        }

        [OutputCache(Duration = 30, Location = System.Web.UI.OutputCacheLocation.ServerAndClient,VaryByParam ="chave")]
        public async Task<ActionResult> Detalhes(string chave)
        {
            string key = primeKey + "Detalhes:" + chave + ":TNoticias";

            NoticiasDetalhesViewModel noticia = null;

            Func<object, NoticiasDetalhesViewModel> funcao = t => DetalhesDB(chave);
            noticia = RedisService.GetOrSetToRedis(key, funcao, 30);

            if (noticia == null)
            {
                return RedirectToAction("Index", "Noticias");
            }
            else
            {
                if (noticia.CreditoFotoNoTexto)
                {
                    var textos = fixFotoCredito(noticia.Texto);

                    if (textos.Length > 0 && !string.IsNullOrEmpty(textos[0]))
                    {
                        noticia.Texto = textos[0];
                        noticia.FotoCredito = (!string.IsNullOrEmpty(textos[1]) ? textos[1] : null);
                    }

                }
            }

            key = primeKey + "Detalhes:Banners:TBanners";

            List<BannerHomeViewModel> banners = null;

            Func<object, List<BannerHomeViewModel>> funcao2 = t => BannerDB();
            banners = RedisService.GetOrSetToRedis(key, funcao2, 300);

            noticia.Banner1 = banners.Where(b => (b.Chave == "banner-noticia-super" || b.Chave == "banner-noticia-leaderboard" || b.Chave == "banner-noticia-maxiboard") && b.AreaBannerAtivo.Value).OrderBy(x => Guid.NewGuid()).FirstOrDefault();

            return View(noticia);
        }

        private List<BannerHomeViewModel> BannerDB()
        {
            var hoje = DateTime.Now;
            return db.Banners.Include(x => x.AreaBanner).Where(x => x.Liberado && !x.Excluido && hoje >= x.DataInicio && hoje <= x.DataFim)
            .Select(banner => new BannerHomeViewModel
            {
                Arquivos = banner.Arquivo,
                Id = banner.Id,
                Link = banner.Link,
                Titulo = banner.Titulo,
                Anunciante = banner.Anunciante,
                Chave = banner.AreaBanner.FirstOrDefault().chave,
                AreaBannerDescricao = banner.AreaBanner.FirstOrDefault().Descricao,
                AreaBannerTamanho = banner.AreaBanner.FirstOrDefault().Tamanho,
                AreaBannerAtivo = banner.AreaBanner.FirstOrDefault().Ativo.Value
            }).ToList();
        }

        private NoticiasDetalhesViewModel DetalhesDB(string chave)
        {
            return db.Noticias.Where(x => x.liberado && !x.excluido && x.dataAtualizacao < DateTime.Now && x.url == chave)
                .Select(n => new NoticiasDetalhesViewModel
                {
                    Id = n.id,
                    Titulo = n.titulo,
                    TituloCapa = n.TituloCapa,
                    Chamada = n.chamada,
                    Foto = n.foto,
                    Subtitulo = n.subtitulo,
                    PorAutor = n.porAutor,
                    DataCadastro = n.dataCadastro,
                    DataAtualizacao = n.dataAtualizacao,
                    Audio = n.audio,
                    FotoCredito = n.fotoCredito,
                    FotoDescricao = n.fotoDescricao,
                    CreditoFotoNoTexto = n.CreditoFotoNoTexto,
                    chaveGaleria = n.Galeria != null ? n.Galeria.chave : null,
                    exibirImagemInterna = n.ExibirImagemInterna,
                    galeriaExcluida = n.Galeria != null ? n.Galeria.excluido : false,
                    Texto = n.texto,
                    VideoYoutube = n.videoYoutube,
                    Tags = n.Tags.Select(t => t.Titulo).ToList()
                }).FirstOrDefault();
        }

        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Esportes()
        {
            return RedirectPermanent("/editoria/esporte");
        }

        private string[] fixFotoCredito(string textoEmbeded)
        {
            string[] textos = { null, null };

            if (!string.IsNullOrEmpty(textoEmbeded))
            {
                Regex regex = new Regex(@"(<em>(.*?)<\/em>)");
                Match match = regex.Match(textoEmbeded);
                if (match.Success)
                {
                    string textoSubs = match.Groups[1].Value;
                    textos[0] = textoEmbeded.Replace(textoSubs, " ");

                    textos[1] = match.Groups[2].Value;
                }

            }

            return textos;
        }


    }
}