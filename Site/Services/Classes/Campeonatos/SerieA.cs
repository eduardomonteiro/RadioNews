using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class SerieA : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Campeonato Brasileiro - Série A";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileiroa/class.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileiroa/class.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileiroa/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileiroa/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileiroa/jogos.xml"));
        }        
    }
}