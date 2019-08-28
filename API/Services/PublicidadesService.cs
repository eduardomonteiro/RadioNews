using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using API.Models;
using API.Util;
using Site.Services;
using System.Configuration;

namespace API.Services
{
    public static class PublicidadesService
    {
        static string primeKey = "publicidades:";
        public static List<PublicidadeViewModel> ObterPublicidadesPrincipais()
        {
            string key = primeKey + "ObterPublicidadesPrincipais:TBanners";

            List<PublicidadeViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<PublicidadeViewModel>> funcao = t => ObterPublicidadesPrincipais(db);
                retorno = Site.Services.RedisService.GetOrSetToRedis(key, funcao, 60);
            }
            BannersToFIFO(retorno);

            return retorno;
        }
        public static List<PublicidadeViewModel> ObterPublicidadesPrincipais(ModeloDados db)
        {
            var url = ServerSettings.Url;

            var publicidades = db.Banners
                .Where(banner => !banner.Excluido && banner.Liberado &&
                    DateTime.Now >= banner.DataInicio &&
                    DateTime.Now <= banner.DataFim &&
                    banner.AreaBanner.Any(area => area.chave.Contains("app-principal"))
                ).ToList();

            //foreach (var publicidade in publicidades)
            //{
            //    publicidade.BannersVisualizacoesCliques.Add(new BannersVisualizacoesCliques
            //    {
            //        CodigoBanner = publicidade.Id,
            //        Visualizacao = true,
            //        Clique = false,
            //        DataCadastro = DateTime.Now
            //    });

            //    db.Entry(publicidade).State = EntityState.Modified;
            //}
            //db.SaveChanges();

            var publicidadesList = publicidades.Select(banner => new PublicidadeViewModel
            {
                Id = banner.Id,
                Url = string.IsNullOrEmpty(banner.Link) ? null : url + "/API/publicidade/clique?idBanner=" + banner.Id,
                Imagem = url + "/Admin/Conteudo/Banners/600x86/" + banner.Arquivo

            }).ToList();

            return publicidadesList;
        }
        public static List<PublicidadeViewModel> ObterPublicidadesInternas()
        {

            string key = primeKey + "ObterPublicidadesInternas:TBanners";

            List<PublicidadeViewModel> retorno = null;
            using (var db = new ModeloDados())
            {
                Func<object, List<PublicidadeViewModel>> funcao = t => ObterPublicidadesInternas(db);
                retorno = RedisService.GetOrSetToRedis(key, funcao, 60);
            }
            BannersToFIFO(retorno);

            return retorno;

        }
        public static List<PublicidadeViewModel> ObterPublicidadesInternas(ModeloDados db)
        {
            var url = ServerSettings.Url;

            var publicidades = db.Banners.Where(banner => !banner.Excluido && banner.Liberado && DateTime.Today >= banner.DataInicio && DateTime.Today <= banner.DataFim && banner.AreaBanner.Any(area => area.chave.Contains("app-interna")));

            //foreach (var publicidade in publicidades)
            //{
            //    publicidade.BannersVisualizacoesCliques.Add(new BannersVisualizacoesCliques
            //    {
            //        CodigoBanner = publicidade.Id,
            //        Visualizacao = true,
            //        Clique = false,
            //        DataCadastro = DateTime.Now
            //    });

            //    db.Entry(publicidade).State = EntityState.Modified;
            //}
            //db.SaveChanges();

            var publicidadesList = publicidades.Select(banner => new PublicidadeViewModel
            {
                Id = banner.Id,
                Url = string.IsNullOrEmpty(banner.Link) ? null : url + "/API/publicidade/clique?idBanner=" + banner.Id,
                Imagem = url + "/Admin/Conteudo/Banners/600x331/" + banner.Arquivo

            }).ToList();

            return publicidadesList;

        }

        private static void BannersToFIFO(List<PublicidadeViewModel> lista)
        {
            var banners = lista.Select(p => new BannersVisualizacoesCliques {
                CodigoBanner = p.Id,
                Visualizacao = true,
                Clique = false,
                DataCadastro = DateTime.Now
            }).ToList();
            foreach (var banner in banners)
            {
                RedisService.LeftPush(banner, ConfigurationManager.AppSettings["BannersFIFOKey"]);
            }
        }
    }
}