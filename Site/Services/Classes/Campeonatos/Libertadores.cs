using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Site.Services.Classes.Campeonatos
{
    public class Libertadores : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Libertadores";
            PopularClubesLibertadores(HttpContext.Current.Server.MapPath("~/tabelafacil/libertadores/jogos.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/libertadores/class.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/libertadores/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/libertadores/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/libertadores/jogos.xml"));
        }
        
        public void PopularClubesLibertadores(string caminho)
        {
            //XDocument doc = XDocument.Load(caminho);
            ////Id = (from id in doc.Descendants("standings") select (int)id.Attribute("id")).First();
            //Clubes = new List<Clube>();
            //Clubes = (from linha
            //          in doc.Descendants("team")
            //          select new Clube
            //          {
            //              UID = (string)linha.Attribute("uid"),
            //              Id = (string)linha.Attribute("code"),
            //              Logo = string.IsNullOrEmpty(linha.Attribute("uid").ToString()) ? "escudo.gif" : (string)linha.Attribute("uid") + ".gif",
            //              Nome = (string)linha.Attribute("name")
            //          }).ToList();
        }        
    }
}