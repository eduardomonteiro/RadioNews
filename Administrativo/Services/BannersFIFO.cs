using Administrativo.Models;
using Site.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Administrativo.Services
{
    public class BannersFIFO
    {
        private static string primeKey = ConfigurationManager.AppSettings["BannersFIFOKey"];
        public static void BannerToFIFO(List<BannersVisualizacoesCliques> banners)
        {
            foreach (var banner in banners)
            {
                if (banner != null)
                    RedisService.LeftPush(banner, primeKey);
            }
        }
        public static void BannerFromFIFOToDB()
        {
            RadioItatiaiaContext db = new RadioItatiaiaContext();
            var obj = (BannersVisualizacoesCliques)RedisService.RightPop<BannersVisualizacoesCliques>(primeKey);
            while (obj != null) 
            {
                var publicidade = db.Banners.Where(b => b.Id == obj.CodigoBanner).FirstOrDefault();
                var banner = new BannersVisualizacoesCliques()
                {
                    Clique = obj.Clique,
                    Visualizacao = obj.Visualizacao,
                    CodigoBanner = obj.CodigoBanner,
                    DataCadastro = obj.DataCadastro
                };
                publicidade.BannersVisualizacoesCliques.Add(banner);
                db.Entry(publicidade).State = EntityState.Modified;
                obj = (BannersVisualizacoesCliques)RedisService.RightPop<BannersVisualizacoesCliques>(primeKey);
            }
            db.SaveChanges();
        }
    }
}