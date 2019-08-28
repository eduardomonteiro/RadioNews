using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class IndicadoresBovespa
    {
        public int Id { get; set; }
        public string PercentualVariacao { get; set; }
        public string Pontos { get; set; }
        public string ValorVariacao { get; set; }
        public DateTime Leitura { get; set; }
    }
}