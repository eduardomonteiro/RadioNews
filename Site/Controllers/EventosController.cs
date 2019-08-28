using FlickrNet;
using FlickrNet.Exceptions;
using Site.Helpers;
using Site.Models;
using Site.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Site.Controllers
{
    public class EventosController : BaseController
    {
        private Banners _banner;

        public EventosController()
        {
            _banner = db.Banners.Where(x => !x.Excluido && x.Liberado && x.AreaBanner.Any(w => w.Tamanho == "257x214") && x.AreaBanner.Any(w => w.Ativo.Value)).OrderBy(x => Guid.NewGuid()).FirstOrDefault();
        }


        // GET: CompanyNameNoPonto
        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "evento")]
        public ActionResult Index(string evento = null)
        {
            var agora = DateTime.Now;

            ViewBag.Eventos = db.Eventos.Where(evt => !evt.Excluido && evt.Liberado).Select(evt => new { evt.Titulo }).Distinct().OrderBy(evt => evt.Titulo).Select(evt => new SelectListItem { Value = evt.Titulo, Text = evt.Titulo, Selected = evt.Titulo == evento }).ToList();

            ViewBag.Banner = _banner;
            
            return View();

        }

        [OutputCache(Duration = 300, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id;local;dataInicio;dataFinal;pagina")]
        public ActionResult Detalhes(int id, string local = null, DateTime? dataInicio = null, DateTime? dataFinal = null, int? pagina = null)
        {
            var flickr = new CompanyNameFlickr();

            var eventoModel = db.Eventos.FirstOrDefault(evento => evento.Id == id && !evento.Excluido && evento.Liberado);

            if (eventoModel == null)
            {
                return HttpNotFound();
            }

            ViewBag.Banner = _banner;
            ViewBag.Locais = eventoModel.Acontecimentos.AsParallel()
                .Where(acontecimento => !acontecimento.Evento.Excluido && acontecimento.Evento.Liberado && !acontecimento.Excluido)
                .Select(acontecimento => new { acontecimento.Local }).Distinct().OrderBy(acontecimento => acontecimento.Local)
                .Select(acontecimento => 
                    new SelectListItem 
                    {
                        Value = acontecimento.Local,
                        Text = acontecimento.Local, 
                        Selected = local != null && acontecimento.Local == local 
                    }).ToList();

            var detalhesViewModel = new DetalhesEventoViewModel
            {
                Id = eventoModel.Id,
                Titulo = eventoModel.Titulo,
                Texto = eventoModel.Texto,
                Imagem = eventoModel.Imagem,
                Encerrados = eventoModel.Acontecimentos.AsParallel()
                .Where(acontecimento =>

                !acontecimento.Evento.Excluido && acontecimento.Evento.Liberado && acontecimento.Realizado && acontecimento.FlickrId != null && acontecimento.FlickrId != "" && !acontecimento.Excluido
                && (local == null || local == acontecimento.Local)
                && (dataInicio == null || dataInicio <= acontecimento.Data)
                && (dataFinal == null || dataFinal.Value.AddHours(23).AddMinutes(59).AddSeconds(59) >= acontecimento.Data)

                ).Select(acontecimento => new AcontecimentoViewModel
                {
                    Data = acontecimento.Data,
                    HoraFim = acontecimento.HoraFim,
                    HoraInicio = acontecimento.HoraInicio,
                    Id = acontecimento.Id,
                    Local = acontecimento.Local,
                    Url = acontecimento.Url,
                    Imagens = flickr.PhotosetsGetPhotos(acontecimento.FlickrId, 1, 1)

                }).OrderByDescending(acontecimento => acontecimento.Data).ToPagedList(pagina ?? 1, 6)
            };

            return View(detalhesViewModel);
        }

        [HttpPost]
        public ActionResult ObterEventosData(DateTime data, string evento = null)
        {
            var eventoString = Uri.UnescapeDataString(evento).Replace("+", " ");
            
            var noticias = db.Eventos.Where(evt =>
                (eventoString == string.Empty || evt.Titulo == eventoString)
                && evt.Liberado
                && !evt.Excluido
                && evt.Acontecimentos.Any(acontecimento => !acontecimento.Excluido && acontecimento.Data.Month == data.Month && acontecimento.Data.Year == data.Year && acontecimento.Data.Day == data.Day))
                .Select(evt => new EventoViewModel { Id = evt.Id, Imagem = evt.Imagem, Titulo = evt.Titulo }).ToList();
            
            return Json(noticias);
        }

        [HttpPost]
        public ActionResult ObterEventosMes(DateTime data, string evento = null)
        {
            var eventoString = Uri.UnescapeDataString(evento).Replace("+", " ");
            
            var queryNoticia = db.Eventos.Where(evt =>
                (eventoString == string.Empty || evt.Titulo == eventoString) 
                && evt.Liberado
                && !evt.Excluido
                && evt.Acontecimentos.Any(acontecimento => !acontecimento.Excluido && acontecimento.Data.Month == data.Month && acontecimento.Data.Year == data.Year));

            var noticias = queryNoticia.Select(evt => new EventoViewModel { Id = evt.Id, Imagem = evt.Imagem, Titulo = evt.Titulo }).ToList();

            var dias = queryNoticia.SelectMany(noticia => noticia.Acontecimentos.Where(acontecimento => !acontecimento.Excluido && acontecimento.Data.Month == data.Month && acontecimento.Data.Year == data.Year).Select(acontecimento => acontecimento.Data.Day)).Distinct().ToList();

            return Json(new { dias, noticias });
        }


        [OutputCache(Duration = 1200, Location = System.Web.UI.OutputCacheLocation.ServerAndClient, VaryByParam = "id")]
        public ActionResult Acontecimento(int id)
        {
            var flickr = new CompanyNameFlickr();

            var acontecimentoModel = db.Acontecimentos.FirstOrDefault(acontecimento => acontecimento.Id == id && !acontecimento.Excluido);

            var acontecimentoViewModel = new AcontecimentoViewModel
            {
                Id = acontecimentoModel.Id,
                EventoId = acontecimentoModel.EventoId,
                Data = acontecimentoModel.Data,
                HoraFim = acontecimentoModel.HoraFim,
                HoraInicio = acontecimentoModel.HoraInicio,
                Local = acontecimentoModel.Local,
                EventoUrl = acontecimentoModel.Evento.Url,
                EventoTitulo = acontecimentoModel.Evento.Titulo,
                Imagens = flickr.PhotosetsGetPhotos(acontecimentoModel.FlickrId)
            };

            ViewBag.Banner = _banner;

            //acontecimentoViewModel.Imagens.First().

            return View(acontecimentoViewModel);

        }
        
        public PartialViewResult MenuProximos(int id)
        {
            var agora = DateTime.Now;

            var proximos = db.Acontecimentos.Where(acontecimento => acontecimento.EventoId == id && !acontecimento.Evento.Excluido && acontecimento.Evento.Liberado && !acontecimento.Excluido && agora < acontecimento.Data && acontecimento.FlickrId != null && acontecimento.FlickrId != "")
                .Select(acontecimento => new AcontecimentoViewModel
                {
                    Data = acontecimento.Data,
                    HoraFim = acontecimento.HoraFim,
                    HoraInicio = acontecimento.HoraInicio,
                    Id = acontecimento.Id,
                    Local = acontecimento.Local
                }).ToList();

            return PartialView("_Proximos", proximos);
        }

    }
}