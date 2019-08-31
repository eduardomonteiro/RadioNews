using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Site.Services.Classes.Campeonatos
{
    public class SulAmericana : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Copa Sul-Americana";
            PopularClubesSulAmericana(HttpContext.Current.Server.MapPath("~/tabelafacil/sulamericana/jogos.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/sulamericana/jogos.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/sulamericana/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/sulamericana/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/sulamericana/jogos.xml"));

        }
        public void PopularClubesSulAmericana(string caminho)
        {
            //XDocument doc = XDocument.Load(caminho);
            ////Id = (from id in doc.Descendants("standings") select (int)id.Attribute("id")).First();
            //Clubes = new List<Clube>();
            //Clubes = (from linha
            //          in doc.Descendants("match").Descendants("team")
            //          select new Clube
            //          {
            //              UID = (string)linha.Attribute("uid"),
            //              Id = (string)linha.Attribute("code"),
            //              Logo = (string)linha.Attribute("uid") + ".gif",
            //              Nome = (string)linha.Attribute("name")
            //          }).ToList();


        }
    }
}