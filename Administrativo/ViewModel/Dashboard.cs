using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Administrativo.ViewModel
{

    public class DashboardViewModel
    {
        public int qtdVisualizacoes { get; set; }
        public int qtdCliques { get; set; }
        public List<ItensGrafico> grafico1Visualizacoes { get; set; }
        public List<ItensGrafico> grafico1Cliques { get; set; }
        public List<ItensGrafico> grafico2 { get; set; }
    }

    public class ItensGrafico
    {
        public double valor { get; set; }
        public string texto { get; set; }

    }
}