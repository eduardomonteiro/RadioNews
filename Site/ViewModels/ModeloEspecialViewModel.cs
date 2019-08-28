using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;
using PagedList;

namespace Site.ViewModels
{
    public class ModeloEspecialViewModel
    {
        public Editoriais Editoria { get; set;}
        public IPagedList<Noticias> paginacao { get; set; }
    }

    public class ModeloEspecialSecoesViewModel
    {
        public Editoriais Editoria { get; set; }
        public Banners Banner { get; set; }
        public List<Noticias> Noticias { get; set; }
    }


    public class ModeloEspecialPampulhaViewModel
    {
        public Editoriais Editoria { get; set; }
        public Banners Banner { get; set; }
        public List<Noticias> NoticiasDestaques { get; set; }
        public List<Noticias> Noticias { get; set; }
    }

    public class ModeloEspecialCarnavalViewModel
    {
        public Banners Banner { get; set; }
        public List<Noticias> NoticiasDestaques { get; set; }
        public IPagedList<Noticias> Noticias { get; set; }
        public IPagedList<Blocos> paginacao { get; set; }
    }
}