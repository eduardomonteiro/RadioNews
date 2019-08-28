using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;

namespace Site.ViewModels
{
    public class EsportesViewModel
    {
        public List<Noticias> NoticiasSlider { get; set; }
        public List<Noticias> NoticiasDestaque { get; set; }

        public List<Noticias> NoticiasAmerica { get; set; }
        public List<Noticias> NoticiasAtletico { get; set; }
        public List<Noticias> NoticiasCruzeiro { get; set; }
        public List<Noticias> NoticiasBrasil { get; set; }

        public List<Noticias> NoticiasEditoriaEsportes { get; set; }

        public Banners Banner { get; set; }
    }
}

