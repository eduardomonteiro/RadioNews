using Site.Enums;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebAPI.Controllers
{
    [RoutePrefix("api/radios")]
    public class RadiosController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult ObterRadios()
        {
            using (var db = new ModeloDados())
            {
                var radios = db.Radios.Select(radio => 
                    new
                    {
                        radio.Id,
                        streamingMP3 = (radio.TipoStream1 == TipoStream.MP3 ? radio.Stream1 : radio.TipoStream2 == TipoStream.MP3 ? radio.Stream2 : string.Empty),
                        streamingAAC = (radio.TipoStream1 == TipoStream.AAC ? radio.Stream1 : radio.TipoStream2 == TipoStream.AAC ? radio.Stream2 : string.Empty),
                        streamingM3U3 = (radio.TipoStream1 == TipoStream.M3U3 ? radio.Stream1 : radio.TipoStream2 == TipoStream.M3U3 ? radio.Stream2 : string.Empty),
                        streamingRTSP = (radio.TipoStream1 == TipoStream.RTSP ? radio.Stream1 : radio.TipoStream2 == TipoStream.RTSP ? radio.Stream2 : string.Empty), 
                        radio.Titulo,
                        radio.Imagem
                    }).ToList();

                return Json(radios);
            }
        }
    }
}
