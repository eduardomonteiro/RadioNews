using PagedList;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels.SeuBolso
{
    public class SeuBolsoViewModel
    {
        public List<BannerSeuBolsoViewModel> Banners { get; set; }
        public IPagedList<NoticiasViewModel> Noticias { get; set; }
        public IndicadoresViewModel Indicadores { get; set; }
        public List<MoedaValor> MoedasValor { get; set; }
    }
}