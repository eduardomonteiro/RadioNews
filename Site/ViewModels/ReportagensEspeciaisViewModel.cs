using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class ReportagensEspeciaisViewModel
    {
        public List<ReportagemEspecialViewModel> Reportagens { get; set; }
    }
    public class ReportagemEspecialViewModel
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string nome { get; set; }
        public string FotoCapa { get; set; }
        public string Descricao { get; set; }
        public string Chave { get; set; }
    }
}