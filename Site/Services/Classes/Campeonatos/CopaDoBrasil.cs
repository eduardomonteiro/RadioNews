using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace Site.Services.Classes.Campeonatos
{
    public class CopaDoBrasil : Campeonato
    {
        public override void Popular()
        {
            Titulo = "Copa do Brasil";
            PopularClubesCopaDoBrasil(HttpContext.Current.Server.MapPath("~/tabelafacil/copadobrasil/jogos.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/copadobrasil/fases.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/copadobrasil/artilharia.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/copadobrasil/jogos.xml"));
        }

        public void PopularClubesCopaDoBrasil(string caminho)
        {
            XDocument doc = XDocument.Load(caminho);
            //Id = (from id in doc.Descendants("standings") select (int)id.Attribute("id")).First();
            Clubes = new List<Clube>();
            Clubes = (from linha
                      in doc.Descendants("team")
                      select new Clube
                      {
                          UID = (string)linha.Attribute("uid"),
                          Id = (string)linha.Attribute("code"),
                          Logo = string.IsNullOrEmpty(linha.Attribute("uid").ToString()) ? "escudo.gif" : (string)linha.Attribute("uid") + ".gif",
                          Nome = (string)linha.Attribute("name")
                      }).ToList();

            Clubes = Clubes.Distinct().ToList();

        }

    }
}