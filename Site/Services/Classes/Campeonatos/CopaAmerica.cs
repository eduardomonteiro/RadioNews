using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class CopaAmerica : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Copa América";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/copaamerica/class.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/copaamerica/class.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/copaamerica/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/copaamerica/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/copaamerica/jogos.xml"));
        }        
    }
}