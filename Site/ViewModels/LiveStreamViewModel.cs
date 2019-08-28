using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class LiveStreamPageViewModel
    {
        public BannerViewModel BannerTopo { get; set; }
        public BannerViewModel BannerBaixo { get; set; }
    }

    public class LiveStreamViewModel
    {
        public string Legenda { get; set; }
        public string CodigoTransmissao { get; set; }
        public string DataCadastro { get; set; }
    }
    



}