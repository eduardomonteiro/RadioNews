using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Site.ViewModels;

namespace Site.Controllers
{
    public class AudioController : BaseController
    {
        // GET: Audio
        public ActionResult Index(string tipo, int id)
        {
            var url = "http://www.CompanyName.com.br";
            if (string.IsNullOrEmpty(tipo) || id == 0 || (tipo != "audio" && tipo != "noticia"))
                return RedirectToAction("Index", "Home");
            AudioPageViewModel audio = new AudioPageViewModel {
                AppId = "1478878518854722",
                Imagem = url + "/images/logo-branca.png",
                NomeSite = "Rádio CompanyName",
                Tipo = "music.song"
            };
            if (tipo == "audio")
            {
                audio = db.Audios.Where(a => a.Id == id && !a.Excluido && a.Liberado)
                    .Select( a => new AudioPageViewModel {
                        AppId = audio.AppId,
                        Imagem = audio.Imagem,
                        NomeSite = audio.NomeSite,
                        Tipo = audio.Tipo,
                        Titulo = a.Colecao.Titulo,
                        Url = url + "/Admin/conteudo/audios/" + a.Id + "/" + a.Url
                    }).FirstOrDefault();
            } else if (tipo == "noticia")
            {
                audio = db.Noticias.Where(n => n.id == id && !n.excluido && n.liberado)
                    .Select( n => new AudioPageViewModel {
                        AppId = audio.AppId,
                        Imagem = audio.Imagem,
                        NomeSite = audio.NomeSite,
                        Tipo = audio.Tipo,
                        Titulo = n.titulo,
                        Url = url + "/Admin/conteudo/noticias/" + n.id + "/audio/" + n.audio
                    }).FirstOrDefault();
            }
            return View(audio);
        }
    }
}