using Site.Models;
using Site.Services;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Site.Helpers
{
    public class BannerTopo
    {
        private static string primeKey = "bannerTopo:";
        private static ModeloDados db = new ModeloDados();

        [OutputCache(Duration = 600, Location = System.Web.UI.OutputCacheLocation.ServerAndClient)]
        public static List<BannerTopoViewModel> getBanners()
        {

            string key = primeKey + "getBanners:TBanners";

            List<BannerTopoViewModel> retorno = null;

            Func<object, List<BannerTopoViewModel>> funcao = t => getBannersDB();
            retorno = RedisService.GetOrSetToRedis(key, funcao, 600);

            return retorno
                    .OrderBy(x => Guid.NewGuid())
                    .Take(5).ToList();
        }
        private static List<BannerTopoViewModel> getBannersDB()
        {
            var lista = db.Banners.Where(
                    x => !x.Excluido &&
                    x.Liberado &&
                    x.DataInicio <= DateTime.Now &&
                    x.DataFim >= DateTime.Now &&
                    (x.AreaBanner.FirstOrDefault().chave == "banner-topo" ||
                     x.AreaBanner.FirstOrDefault().chave== "banner-topo-2") &&
                    x.AreaBanner.FirstOrDefault().Ativo.Value
                ).Select(banner => new BannerTopoViewModel
                {
                    Arquivo = banner.Arquivo,
                    Arquivo2 = banner.Arquivo2,
                    Id = banner.Id,
                    Link = banner.Link,
                    Titulo = banner.Titulo,
                    Chave = banner.AreaBanner.FirstOrDefault().chave,
                    Anunciante = banner.Anunciante,
                    AreaDescricao = banner.AreaBanner.FirstOrDefault().Descricao,
                    Tamanho = banner.AreaBanner.FirstOrDefault().Tamanho,
                    Tamanho2 = banner.AreaBanner.FirstOrDefault().Tamanho2
                    //Html = banner.Html
                })
                .ToList();
            return lista;
        }
    }
}