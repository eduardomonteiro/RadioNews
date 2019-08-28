using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace Site.Models
{
    public class IndicadorFinanceiro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public bool Liberado { get; set; }
        public DateTime Atualizado { get; set; }
    }
}