using PagedList;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class NoticiaViewModel
    {
        public List<Noticias> Destaques { get; set; }
        public List<Noticias> DestaquesBanners { get; set; }
        public Banners Banner { get; set; }
        public IPagedList<Noticias> paginacao { get; set; }
    }
}