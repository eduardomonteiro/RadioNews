using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class Mineiro : Campeonato
    {
        public override void Popular()
        {
            Titulo = "Campeonato Mineiro";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/campeonatomineiro/classificacao.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/campeonatomineiro/classificacao.xml"));
            PopularArtilharia(HttpContext.Current.Server.MapPath("~/tabelafacil/campeonatomineiro/artilharia.xml"));
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/campeonatomineiro/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/campeonatomineiro/jogos.xml"));
        }        
    }
}