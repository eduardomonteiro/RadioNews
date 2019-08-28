using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using Administrativo.Models;

namespace Administrativo.Services
{
    public class UpdateIbovespa
    {
        public static void Sync()
        {
            //Uri targetUri = new Uri("https://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20yahoo.finance.quotes%20where%20symbol%20in%20(%22^BVSP%22)&format=json&env=store%3A%2F%2Fdatatables.org%2Falltableswithkeys&callback=");
            //HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(targetUri);
            //var response = request.GetResponse() as HttpWebResponse;

            //var responseReader = new StreamReader(response.GetResponseStream());
            //var responseData = responseReader.ReadToEnd();

            //var ibovespa_new = Newtonsoft.Json.JsonConvert.DeserializeObject<IbovespaJson>(responseData);

            //RadioCompanyNameContext db2 = new RadioCompanyNameContext();
            //var ibovespa = db2.IndicadoresBovespas.Select(x => x).FirstOrDefault();

            //var percentualVariacao = float.Parse(ibovespa_new.query.results.quote.PercentChange.Replace(".",",").Replace("%", ""));
            //ibovespa.PercentualVariacao = percentualVariacao > 0 ? "+" : "";
            //ibovespa.PercentualVariacao += percentualVariacao.ToString("n2");

            //var pontos = int.Parse(ibovespa_new.query.results.quote.LastTradePriceOnly.Split('.')[0]);
            //ibovespa.Pontos = (pontos).ToString();

            //var variacao = float.Parse(ibovespa_new.query.results.quote.Change.Replace(".", ","));
            //ibovespa.ValorVariacao = variacao > 0 ? "+" : "-";
            //ibovespa.ValorVariacao += (variacao).ToString("n2");

            //ibovespa.Leitura = ibovespa_new.query.results.quote.LastTradeDate;

            //db2.Entry(ibovespa).State = EntityState.Modified;
            //db2.SaveChanges();
        }
    }
    public class IbovespaJson
    {
        public IbovespaQuery query { get; set; }
    }
    public class IbovespaQuery
    {
        public IbovespaResults results { get; set; }
    }
    public class IbovespaResults
    {
        public IbovespaQuote quote { get; set; }
    }
    public class IbovespaQuote
    {
        public string PercentChange { get; set; }
        public string LastTradePriceOnly { get; set; }
        public string Change { get; set; }
        public DateTime LastTradeDate { get; set; }
    }

}