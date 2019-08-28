using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class EspeciaisViewModel
    {
        public EditoriaEspecialViewModel Editoria { get; set; }
        public IPagedList<NoticiaEspecialViewModel> noticias { get; set; }
        public List<NoticiaEspecialViewModel> destaques { get; set; }
    }

    public class NoticiaEspecialViewModel
    {
        public int Id { get; set; }
        public bool Destaque { get; set; }
        public string titulo { get; set; }
        public string tituloCapa { get; set; }
        public string subTitulo { get; set; }
        public string foto { get; set; }
        public string FotoCredito { get; set; }
        public string chamada { get; set; }
        public string videoYoutube { get; set; }
        public string texto { get; set; }
        public string Url { get; set; }
        public DateTime dataCadastro { get; set; }
        public DateTime dataAtualizacao { get; set; }
    }
    public class EditoriaEspecialViewModel
    {
        public int Id { get; set; }
        public string nome { get; set; }
        public string action { get; set; }
        public string Chave { get; set; }
    }
}