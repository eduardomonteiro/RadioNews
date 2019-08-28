using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Services.Classes
{
    public class Rodada
    {
        public int Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public bool Atual { get; set; }
        public bool Proxima { get; set; }
        public string Abreviado { get; set; }
        public string Titulo { get; set; }
        public List<Jogo> Jogos { get; set; }
    }
}