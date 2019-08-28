using System.Web;

namespace Site.Services.Classes.Campeonatos
{
    public class Eliminatorias : Campeonato
    {
        public override void Popular()
        {            
            Titulo = "Eliminatórias";
            PopularClubes(HttpContext.Current.Server.MapPath("~/tabelafacil/conmebol/class.xml"));
            PopularClassificacao(HttpContext.Current.Server.MapPath("~/tabelafacil/conmebol/class.xml"));            
            PopularFaseseRodadas(HttpContext.Current.Server.MapPath("~/tabelafacil/conmebol/fases.xml"));
            PopularJogos(HttpContext.Current.Server.MapPath("~/tabelafacil/conmebol/jogos.xml"));
        }        
    }
}