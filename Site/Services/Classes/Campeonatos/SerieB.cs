using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class SerieB : Campeonato
    {
        public override void Popular()
        {
            Titulo = "Campeonato Brasileiro - Série B";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileirob/class.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileirob/class.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileirob/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileirob/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/brasileirob/jogos.xml"));
        }        
    }
}