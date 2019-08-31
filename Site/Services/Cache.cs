using Site.Services.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Site.Services
{
    public static class Cache
    {
        public static Classes.Campeonatos.SerieA SerieA;
        public static Classes.Campeonatos.SerieB SerieB;
        public static Classes.Campeonatos.Mineiro Mineiro;
        public static Classes.Campeonatos.CopaDoBrasil CopaDoBrasil;
        public static Classes.Campeonatos.Libertadores Libertadores;
        public static Classes.Campeonatos.PrimeiraLiga PrimeiraLiga;
        public static Classes.Campeonatos.CopaAmerica CopaAmerica;
        public static Classes.Campeonatos.Eliminatorias Eliminatorias;
        public static Classes.Campeonatos.SulAmericana SulAmericana;
        public static Jogo ProximoBrasil;
        public static Jogo ProximoCruzeiro;
        public static Jogo ProximoAtletico;
        public static Jogo ProximoAmerica;

        public static void InicializaCache()
        {
            var c = new List<Campeonato>();

            SerieA = null;
            SerieA = new Classes.Campeonatos.SerieA();           
            c.Add(SerieA);

            SerieB = null;
            SerieB = new Classes.Campeonatos.SerieB();
            c.Add(SerieB);

            Mineiro = null;
            Mineiro = new Classes.Campeonatos.Mineiro();
            c.Add(Mineiro);

            CopaDoBrasil = null;
            CopaDoBrasil = new Classes.Campeonatos.CopaDoBrasil();
            c.Add(CopaDoBrasil);

            Libertadores = null;
            Libertadores = new Classes.Campeonatos.Libertadores();
            c.Add(Libertadores);

            PrimeiraLiga = null;
            PrimeiraLiga = new Classes.Campeonatos.PrimeiraLiga();
            c.Add(PrimeiraLiga);

            SulAmericana = null;
            SulAmericana = new Classes.Campeonatos.SulAmericana();
            c.Add(SulAmericana);

            CopaAmerica = null;
            CopaAmerica = new Classes.Campeonatos.CopaAmerica();
            c.Add(CopaAmerica);

            Eliminatorias = null;
            Eliminatorias = new Classes.Campeonatos.Eliminatorias();
            c.Add(Eliminatorias);

            CarregarCampeonatos(c);

            #region ProximosJogos
            ProximoAmerica = null;
            ProximoAmerica = ObterProximoJogo(c, "53");
            ProximoAtletico = null;
            ProximoAtletico = ObterProximoJogo(c, "33");
            ProximoBrasil = null;
            ProximoBrasil = ObterProximoJogo(c, "1");
            ProximoCruzeiro = null;
            ProximoCruzeiro = ObterProximoJogo(c, "40");
            #endregion
           
        }
        public static void CarregarCampeonatos(List<Campeonato> campeonatos){
            foreach (var item in campeonatos)
            {
                item.Popular();
            }
        }

        public static Jogo ObterProximoJogo(List<Campeonato> campeonatos, string UID)
        {
            var lista = new List<Jogo>();

            //foreach (var item in campeonatos)
            //{
            //    var ultimo = (from fase in item.Fases
            //                  from rodada in fase.Rodadas
            //                  from jogo in rodada.Jogos
            //                  where jogo.Data >= DateTime.Now && (jogo.Mandante != null ? (jogo.Mandante.UID.Equals(UID) || (jogo.Visitante != null && jogo.Visitante.UID.Equals(UID))) : false)
            //                  orderby jogo.Data
            //                  select jogo)
            //             .FirstOrDefault();

            //    if (ultimo != null) lista.Add(ultimo);
            //}
            return lista.OrderBy(j => j.Data).FirstOrDefault(l => l.Data >= DateTime.Now);

         
        }
        
    }
}