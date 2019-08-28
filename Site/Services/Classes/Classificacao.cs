using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Services.Classes
{
    public class Classificacao
    {
        public int Id { get; set; }
        public int Posicao { get; set; }        
        public Clube Clube { get; set; }
        public int Pontos { get; set; }
        public int Jogos { get; set; }
        public int Vitorias { get; set; }
        public int Empates { get; set; }
        public int Derrotas { get; set; }
        public int GolsPro { get; set; }
        public int GolsContra { get; set; }
        public int SaldoGols { get; set; }
        public double Aproveitamento { get; set; }
    }
}