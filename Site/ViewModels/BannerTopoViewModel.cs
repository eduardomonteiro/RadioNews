using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class BannerTopoViewModel
    {
        public int Id { get; set; }
        public string Anunciante { get; set; }
        public string Titulo { get; set; }
        public string Arquivo { get; set; }
        public string Arquivo2 { get; set; }
        public string Link { get; set; }
        public string Chave { get; set; }
        public string Tamanho { get; set; }
        public string Tamanho2 { get; set; }
        public string AreaDescricao { get; set; }
        public string Html { get; set; }
    }
}