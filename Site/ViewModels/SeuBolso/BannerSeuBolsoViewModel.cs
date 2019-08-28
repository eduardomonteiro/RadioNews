using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels.SeuBolso
{
    public class BannerSeuBolsoViewModel
    {
        public int Id { get; set; }
        public string Anunciante { get; set; }
        public string Titulo { get; set; }
        public string Arquivos { get; set; }
        public string Link { get; set; }
        public string Chave { get; set; }
        public string AreaBannerTamanho { get; set; }
        public string AreaBannerDescricao { get; set; }
        public bool? AreaBannerAtivo { get; set; }
    }
}