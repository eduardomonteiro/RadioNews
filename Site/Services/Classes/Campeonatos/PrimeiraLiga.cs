using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class PrimeiraLiga : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Primeira Liga";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/primeiraliga/class.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/primeiraliga/class.xml"));            
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/primeiraliga/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/primeiraliga/jogos.xml"));
        }        
    }
}