using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Administrativo.Models;

namespace Administrativo.Controllers.PAD
{
    public class BannersPADController : Controller
    {
        protected RadioCompanyNameContext db = new RadioCompanyNameContext();
        public ActionResult Visualizar()
        {
            return View();
        }

        public ActionResult VisualizarNoticia(int id = 0)
        {
            if (id == 0) return RedirectToAction("Index", "NoticiasPaineis");
            var body = SalvaBannerNoticia(id, false).Data.ToString();
            body = ConvertToView(body);
            return Content(body);
        }

        public ActionResult VisualizarEGol(int id = 0)
        {
            if (id == 0) return RedirectToAction("Index", "EGolPaineis");
            var body = SalvaBannerEGol(id, false).Data.ToString();
            body = ConvertToView(body);
            return Content(body);
        }

        public ActionResult VisualizarRedesSociais(int id = 0)
        {
            if (id == 0) return RedirectToAction("Index", "RedesSociaisPaineis");
            var body = SalvaBannerRedesSociais(id, false).Data.ToString();
            body = ConvertToView(body);
            return Content(body);
        }

        public JsonResult SalvaBannerNoticia(int id = 0, bool salvar = true)
        {
            NoticiasPADs noticia = db.NoticiasPADs.Where(n => n.Id == id).FirstOrDefault();
            if (noticia == null) return Json("erro", JsonRequestBehavior.AllowGet);
            string body = System.IO.File.ReadAllText(LayoutsPath + "noticias.html");
            if (!string.IsNullOrEmpty(noticia.Foto))
            {
                body = body.Replace("##noticiaFoto##", URL + "Admin/Conteudo/NoticiasPAD/" + noticia.Id + "/" + noticia.Foto);
            }
            else
            {
                body = body.Replace("##noticiaFoto##", "Content/images/bg-default.jpg");
            }
            body = body.Replace("##noticiaId##", noticia.Id.ToString());
            body = body.Replace("##categoria##", noticia.Categoria);
            body = body.Replace("##titulo##", noticia.Titulo);
            if (noticia.ApoioId != null)
            {
                body = body.Replace("##apoioLogo##", URL + "Admin/Conteudo/ApoioPAD/" + noticia.ApoioId + "/" + noticia.ApoioPADs.Logo);
                body = body.Replace("##apoioNome##", noticia.ApoioPADs.Nome);
            } else
            {
                body = body.Replace("<div class='brand-apoio'>", "<div class='brand-apoio' style='display:none;'>");
            }

            if (!salvar)
            {
                body = body.Replace("<link rel='stylesheet' href='Content/styles.css' />", "<link rel='stylesheet' href='" + URL + "Admin/BannersPad/conteudo/Content/styles.css' />");
                body = body.Replace("Content/images/", URL + "Admin/BannersPad/conteudo/Content/images/");
                return Json(body, JsonRequestBehavior.AllowGet);
            }
            if (SalvaHTML(body)) return Json("ok", JsonRequestBehavior.AllowGet);
            else return Json("erro", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalvaBannerEGol(int id = 0, bool salvar = true)
        {
            EGolPADs gol = db.EGolPADs.Where(n => n.Id == id).FirstOrDefault();
            if (gol == null) return Json("erro", JsonRequestBehavior.AllowGet);
            string body;

            body = System.IO.File.ReadAllText(LayoutsPath + "gol.html");
            body = body.Replace("##bgcolor##", gol.BgColor);
            body = body.Replace("##mandanteLogo##", URL + "Admin/Conteudo/timesPAD/" + gol.MandanteId + "/" + gol.TimesPADs.Logo);
            body = body.Replace("##mandanteNome##", gol.TimesPADs.Nome);
            body = body.Replace("##visitanteLogo##", URL + "Admin/Conteudo/timesPAD/" + gol.VisitanteId + "/" + gol.TimesPADs1.Logo);
            body = body.Replace("##visitanteNome##", gol.TimesPADs1.Nome);
            body = body.Replace("##placar##", gol.Placar);
            body = body.Replace("##jogador##", gol.Jogador);
            body = body.Replace("##horaGol##", gol.HoraDoGol);
            body = body.Replace("##hashtag##", gol.HashTag);
            if (gol.ApoioId != null)
            {
                body = body.Replace("##apoioLogo##", URL + "Admin/Conteudo/ApoioPAD/" + gol.ApoioId + "/" + gol.ApoioPADs.Logo);
                body = body.Replace("##apoioNome##", gol.ApoioPADs.Nome);
            }
            else
            {
                body = body.Replace("<div class='brand-apoio'>", "<div class='brand-apoio' style='display:none;'>");
            }

            body = body.Replace("##goleadorLogo##", URL + "Admin/Conteudo/timesPAD/" + (gol.GolMandante ? (gol.MandanteId + "/" + gol.TimesPADs.Logo) : (gol.VisitanteId + "/" + gol.TimesPADs1.Logo)));
            body = body.Replace("##goleadorNome##", gol.GolMandante ? gol.TimesPADs.Nome : gol.TimesPADs1.Nome);

            if (!salvar)
            {
                body = body.Replace("<link rel='stylesheet' href='Content/styles.css' />", "<link rel='stylesheet' href='" + URL + "Admin/BannersPad/conteudo/Content/styles.css' />");
                body = body.Replace("Content/images/", URL + "Admin/BannersPad/conteudo/Content/images/");
                return Json(body, JsonRequestBehavior.AllowGet);
            }

            if (SalvaHTML(body)) return Json("ok", JsonRequestBehavior.AllowGet);
            else return Json("erro", JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalvaBannerRedesSociais(int id = 0, bool salvar = true)
        {
            RedesSociaisPADs post = db.RedesSociaisPADs.Where(n => n.Id == id).FirstOrDefault();
            if (post == null) return Json("erro", JsonRequestBehavior.AllowGet);
            string body = System.IO.File.ReadAllText(LayoutsPath + "rede-social.html");

            if (!string.IsNullOrEmpty(post.Foto))
            {
                body = body.Replace("##foto##", post.Foto);
            }
            else
            {
                body = body.Replace("##foto##", "Content/images/bg-default.jpg");
            }
            body = body.Replace("##chamada##", post.Chamada);
            body = body.Replace("##tipoRede##", post.TipoRede == 1 ? "facebook" : "twitter");
            body = body.Replace("##nomeRede##", post.TipoRede == 1 ? "Facebook" : "Twitter");
            body = body.Replace("##hashtag##", post.Hashtag);
            if (post.ApoioId != null)
            {
                body = body.Replace("##apoioLogo##", URL + "Admin/Conteudo/ApoioPAD/" + post.ApoioId + "/" + post.ApoioPADs.Logo);
                body = body.Replace("##apoioNome##", post.ApoioPADs.Nome);
            }
            else
            {
                body = body.Replace("<div class='brand-apoio'>", "<div class='brand-apoio' style='display:none;'>");
            }

            if (!salvar)
            {
                body = body.Replace("<link rel='stylesheet' href='Content/styles.css' />", "<link rel='stylesheet' href='" + URL + "Admin/BannersPad/conteudo/Content/styles.css' />");
                body = body.Replace("Content/images/", URL + "Admin/BannersPad/conteudo/Content/images/");
                return Json(body, JsonRequestBehavior.AllowGet);
            }

            if (SalvaHTML(body)) return Json("ok", JsonRequestBehavior.AllowGet);
            else return Json("erro", JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public ActionResult Update672x280()
        {
            string body = System.IO.File.ReadAllText(BannerContent);
            HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            return Content(body);
        }
        [AllowAnonymous]
        public ActionResult Update896x288()
        {
            string body = System.IO.File.ReadAllText(EmpenaContent);
            HttpContext.Response.AddHeader("Access-Control-Allow-Origin", "*");
            return Content(body);
        }

        private static string Header(string body)
        {
            return "<!doctype html><html xml:lang='pt-br' lang='pt-br'><head><meta charset='utf-8'><meta http-equiv='X-UA-Compatible' content='IE=edge'><title>Rádio CompanyName</title><link rel='stylesheet' href='Content/styles.css'/></head><body><div id='conteudo'> " + body;
        }
        private static string FooterEmpena(string body)
        {
            return body + "</div><script src='Content/jquery-1.8.2.min.js'></script><script src='Content/scripts-empena.js'></script></body></html>";
        }
        private static string FooterBanner(string body)
        {
            return body + "</div><script src='Content/jquery-1.8.2.min.js'></script><script src='Content/scripts-banner.js'></script></body></html>";
        }
        private static bool SalvaHTML(string body)
        {
            try
            {
                using (StreamWriter _testData = new StreamWriter(BannerContent, false))
                {
                    _testData.Write(body);
                }
                using (StreamWriter _testData = new StreamWriter(EmpenaContent, false))
                {
                    _testData.Write(body);
                }
                body = Header(body);
                using (StreamWriter _testData = new StreamWriter(BannerHtml, false))
                {
                    var bodyBanner = FooterBanner(body);
                    _testData.Write(bodyBanner);
                }
                using (StreamWriter _testData = new StreamWriter(EmpenaHtml, false))
                {
                    var bodyEmpena = FooterEmpena(body);
                    _testData.Write(bodyEmpena);
                }
                return true;
            } catch
            {
                return false;
            }
        }
        private static string ConvertToView(string body)
        {
            body = Header(body);
            body = body.Replace("src='images/", "src='/Admin/BannersPAD/conteudo/images/");
            body = body.Replace("href='Content/styles.css'", "href='/Admin/BannersPAD/conteudo/Content/styles.css'");
            body = body + "<div><br><p style='color:red;font-weight:bold;'>A visualização do Banner fica melhor se você colocar a resolução 448x144, pois assim você terá uma melhor percepção de como ficará no Painel Real.</p></div></div></body></html>";
            return body;
        }
        private static string URL
        {
            get
            {
                return string.Format("http://localhost/");
            }
        }
        private static string BannerHtml
        {
            get
            {
                return string.Format("{0}/BannersPAD/conteudo/index.html", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string BannerPath
        {
            get
            {
                return string.Format("{0}/BannersPAD/conteudo", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string EmpenaHtml
        {
            get
            {
                return string.Format("{0}/BannersPAD/empena/index.html", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string EmpenaPath
        {
            get
            {
                return string.Format("{0}/BannersPAD/empena", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string LayoutsPath
        {
            get
            {
                return string.Format("{0}/Views/LayoutsPAD/", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string EmpenaContent
        {
            get
            {
                return string.Format("{0}/BannersPAD/empena/content.html", HttpRuntime.AppDomainAppPath);
            }
        }
        private static string BannerContent
        {
            get
            {
                return string.Format("{0}/BannersPAD/conteudo/content.html", HttpRuntime.AppDomainAppPath);
            }
        }
    }
}
