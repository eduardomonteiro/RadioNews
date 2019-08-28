using PagedList;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class BuscaViewModel
    {
        public int Total { get; set; }
        public Banners Banner { get; set; }
        public IPagedList<BuscaResultadosModel> paginacao { get; set; }
    }


    public class BuscaResultadosModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Chamada { get; set; }
        public string Subtitulo { get; set; }
        public string Imagem { get; set; }
        public string Autor { get; set; }
        public string Url { get; set; }
        public string Control { get; set; }
        public DateTime? Data { get; set; }
    }
}