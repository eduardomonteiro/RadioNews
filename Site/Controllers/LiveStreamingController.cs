using PagedList;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Site.Models;
using Site.Services;

namespace Site.Controllers
{
    public class LiveStreamingController : BaseController
    {
        string primeKey = "livestreaming:";

        public LiveStreamingController()
        {
            db.Configuration.LazyLoadingEnabled = false;
        }
        // GET: LiveStreaming
        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult Index()
        {
            string key = primeKey + "Index:TLiveStreaming";

            LiveStreamPageViewModel retorno = null;

            Func<object, LiveStreamPageViewModel> funcao = t => IndexDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 60);

            return View(retorno);
        }

        private LiveStreamPageViewModel IndexDB()
        {
            var bannerTopo = BannerChave("banner-live-topo");
            var bannerBaixo = BannerChave("banner-live-baixo");

            var liveStreamingViewModel = new LiveStreamPageViewModel
            {
                BannerTopo = bannerTopo == null ? null : new BannerViewModel { Anunciante = bannerTopo.Anunciante, Arquivos = bannerTopo.Arquivo, Chave = bannerTopo.AreaBanner.First().chave, Link = bannerTopo.Link, Titulo = bannerTopo.Titulo, AreaBanner = bannerTopo.AreaBanner },
                BannerBaixo = bannerBaixo == null ? null : new BannerViewModel { Anunciante = bannerBaixo.Anunciante, Arquivos = bannerBaixo.Arquivo, Chave = bannerBaixo.AreaBanner.First().chave, Link = bannerBaixo.Link, Titulo = bannerBaixo.Titulo, AreaBanner = bannerBaixo.AreaBanner }
            };

            return liveStreamingViewModel;
        }

        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult NoticiasPassadas(int page)
        {
            string key = primeKey + "Index:NoticiasPassadas:TLiveStreaming:" + page;

            object retorno = null;

            Func<object, object> funcao = t => NoticiasPassadasDB(page);
            retorno = RedisService.GetOrSetToRedis(key, funcao, 300, page);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private object NoticiasPassadasDB(int page)
        {
            var queryableLivePassadas = db.LiveStreamings.Where(live => !live.Ativo && live.DataFinalizacao != null && !live.Excluido);
            var livesPassadas = queryableLivePassadas.OrderByDescending(live => live.DataFinalizacao).ToPagedList(page, 3);
            var list = livesPassadas.Select(live => new LiveStreamViewModel
            {
                Legenda = live.Legenda,
                CodigoTransmissao = live.CodigoTransmissao,
                DataCadastro = live.DataCadastro.ToString("dd/MM/yyyy")
            }).ToList();
            return new { Lives = list, ExibeCarregarMais = livesPassadas.HasNextPage };
        }

        [OutputCache(Duration = 60, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public ActionResult LiveAtual()
        {
            string key = primeKey + "Index:LiveAtual:TLiveStreaming";

            LiveStreamViewModel retorno = null;

            Func<object, LiveStreamViewModel> funcao = t => LiveAtualDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 60);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private LiveStreamViewModel LiveAtualDB()
        {
            var liveAoVivo = db.LiveStreamings.SingleOrDefault(live => live.Ativo && live.DataFinalizacao == null && !live.Excluido);
            return new LiveStreamViewModel { CodigoTransmissao = liveAoVivo?.CodigoTransmissao, Legenda = liveAoVivo?.Legenda };
        }

        public Banners BannerChave(string chave)
        {
            var hoje = DateTime.Now;

            return db.Banners.Include(banner => banner.AreaBanner).OrderBy(banner => Guid.NewGuid()).FirstOrDefault(banner => banner.AreaBanner.Any(area => area.Ativo.Value && area.chave == chave) && !banner.Excluido && banner.Liberado && hoje >= banner.DataInicio && hoje <= banner.DataFim);
        }
    }
}