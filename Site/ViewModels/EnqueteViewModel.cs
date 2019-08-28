using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class EnqueteViewModel
    {
        public string pergunta { get; set; }
        public List<RespostaEnqueteViewModel> Respostas { get; set; }
    }
    public class RespostaEnqueteViewModel
    {
        public int id { get; set; }
        public string resposta { get; set; }
        public int votos { get; set; }
    }
}