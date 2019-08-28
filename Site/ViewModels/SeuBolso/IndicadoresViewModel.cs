using Site.Models;
using System.Collections.Generic;

namespace Site.ViewModels.SeuBolso {
    public class IndicadoresViewModel {
        public IndicadoresBovespa Bovespa { get; set; }
        public List<IndicadorFinanceiro> Financeiros { get; set; }
    }
}