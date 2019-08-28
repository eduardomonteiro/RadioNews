using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Services.Classes
{
    public class Fase
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public List<Rodada> Rodadas { get; set; }
        public string Tipo { get; set; }
    }
    
}