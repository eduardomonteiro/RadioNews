using Site.Models;
using Site.Services.Classes;
using System.Collections.Generic;

namespace Site.ViewModels
{
    public class ClubesViewModel
    {
        public string NomeClube { get; set; }
        public string ClasseCSS { get; set; }
        public Banners Banner1 { get; set; }
        public Banners Banner2 { get; set; }
        public Banners Banner3 { get; set; }
        public IEnumerable<NoticiasViewModel> Destaques { get; set; }
        public IEnumerable<NoticiasViewModel> OutrasNoticias { get; set; }
        public IEnumerable<NoticiasViewModel> Noticias { get; set; }
        public Times Time { get; set; }
        public Jogo ProximoJogo { get; set; }
        public IEnumerable<Elencos> Elenco { get; set; }
    }
}