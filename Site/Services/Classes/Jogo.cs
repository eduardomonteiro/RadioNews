using System;

namespace Site.Services.Classes
{
    public class Jogo
    {        
        public Clube Mandante { get; set; }
        public string PlacarMandante { get; set; }       
        public Clube Visitante { get; set; }
        public string PlacarVisitante { get; set; }        
        public DateTime Data { get; set; }
        public string Local { get; set; }
        public string Campeonato { get; set; }
        public string Grupo { get; set; }
    }
}