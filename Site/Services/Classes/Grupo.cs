using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Services.Classes
{
    public class Grupo
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Abreviacao { get; set; }
        public List<Classificacao> Classificacao { get; set; }
    }
}