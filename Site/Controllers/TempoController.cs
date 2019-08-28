using Site.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;

namespace Site.Controllers
{
    public class TempoController : Controller
    {
        // GET: Tempo
        [OutputCache(Duration = 28800, Location = System.Web.UI.OutputCacheLocation.Server)]
        public JsonResult Previsao()
        {
          #region getPrevisaoTempo
            try
            {
                var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://servicos.cptec.inpe.br/XML/cidade/222/previsao.xml");
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var ser = new XmlSerializer(typeof(cidade2));

                    var temperatura = (cidade2)ser.Deserialize(new StringReader(streamReader.ReadToEnd()));

                    /*
                    var dic = Utils.clima.Where(p => p.Key == temperatura.previsao[0].tempo)
                                .ToDictionary(p => p.Key, p => p.Value);
                                */
                    temperatura.altPrevisao = Utils.clima[temperatura.previsao[0].tempo];

                     
                    
                    return Json(temperatura, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {

            }

            #endregion

            return Json(true);
            
        }

        
    }
    #region auxiliares
    [XmlRoot("cidade")]
    public class cidade2
    {
        [XmlElement("nome")]
        public string nome { get; set; }
        [XmlElement("uf")]
        public string uf { get; set; }
        [XmlElement("atualizacao")]
        public string atualizacao { get; set; }
        [XmlElement("previsao")]
        public List<previsao> previsao { get; set; }
        public string altPrevisao { get; set; }


    }

    public class previsao
    {
        [XmlElement("dia")]
        public string dia { get; set; }
        [XmlElement("tempo")]
        public string tempo { get; set; }
        [XmlElement("maxima")]
        public string maxima { get; set; }
        [XmlElement("minima")]
        public string minima { get; set; }
        [XmlElement("iuv")]
        public string iuv { get; set; }
      
    }
    #endregion
}