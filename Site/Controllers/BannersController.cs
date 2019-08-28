using Site.Enums;
using Site.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class BannersController : Controller
    {
        [HttpGet, ActionName("BannerClick")]
        //[OutputCache(Duration = 2880, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "bannerId")]
        public ActionResult BannerClick(int bannerId)
        {
            //ModeloDados db = new ModeloDados();

            //db.Configuration.LazyLoadingEnabled = false;

            //var banner = db.Banners.FirstOrDefault(x => x.Id == bannerId);

            ////var url = BannerCount(banner, BannerOperation.Clique, ref db);

            //if (!string.IsNullOrEmpty(url))
            //{
            //    return Redirect(url);
            //}
            return null;

        }
        

        [HttpPost, ActionName("VisualizacoesCount")]
        public JsonResult VisualizacoesCount(string[] ids)
        {
            //if (ids.Length > 0)
            //{
            //    Task<string> str = Task.Run(async () =>
            //    {
            //        string msg = await BannerViews(ids);
            //        return msg;
            //    });

            //    return Json(true, JsonRequestBehavior.AllowGet);
            //}

            return Json(true);
        }

        #region async
        private Task<string> BannerViews(string[] ids)
        {
            return Task.Run<string>(() => Visualizacao(ids));
        }
        #endregion

        #region visualicao banner
        public string Visualizacao(string[] ids)
        {
            //ids = ids.Distinct().ToArray();

            //ModeloDados db = new ModeloDados();

            //db.Configuration.LazyLoadingEnabled = false;

            //if (ids.Length > 0)
            //{
            //    foreach (var item in ids)
            //    {
            //        int idInt;
            //        int.TryParse(item, out idInt);

            //        var banner = db.Banners.FirstOrDefault(x => x.Id == idInt);

            //        var url = BannerCount(banner, BannerOperation.Visualizacao, ref db);

            //    }
            //}

            return "";
        }
        #endregion

        /// <summary>
        /// metodo que conta os cliques... ou add visualizacao
        /// </summary>
        /// <param name="banners"></param>
        /// <param name="acao"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public string BannerCount(Banners banners, BannerOperation acao, ref ModeloDados db)
        {
            string url = string.Empty;

            //if (banners != null)
            //{
            //    var registro = new BannersVisualizacoesCliques();


            //    if (acao == BannerOperation.Visualizacao)
            //    {
            //        registro = new BannersVisualizacoesCliques
            //        {
            //            CodigoBanner = banners.Id,
            //            Visualizacao = true,
            //            Clique = false,
            //            DataCadastro = DateTime.Now
            //        };
            //    }
            //    else
            //    {
            //        registro = new BannersVisualizacoesCliques
            //        {
            //            CodigoBanner = banners.Id,
            //            Visualizacao = false,
            //            Clique = true,
            //            DataCadastro = DateTime.Now
            //        };                    

            //url = banners.Link;
            //    }
            //    db.Entry(registro).State = EntityState.Added;
            //    db.SaveChanges();

            //}

            return url;


        }



    }
}