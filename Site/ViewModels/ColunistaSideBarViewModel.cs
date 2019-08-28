using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;
using PagedList;

namespace Site.ViewModels
{
    public class ColunistaSideBarViewModel
    {
        public Colunista Colunista { get; set; }
        public List<ColunistaVM> ListaColunista { get; set; }
        //public List<Noticias> ListaUltimasMaterias { get; set; }
    }

    public class ColunistaMateriasFilterViewModel
    {
        public int id { get; set; }
        public string PalavraChave { get; set; }
        public string EditoriaChave { get; set; }
        public string DataInicio { get; set; }
        public string DataFim { get; set; }
    }

    public class ColunistaMateriasViewModel
    {
        public Colunista Colunista { get; set; }
        public IPagedList<Noticias> paginacao { get; set; }
    }

    public class ColunistaVM
    {        
        public string Chave { get; set; }
        public string Foto { get; set; }
        public string Nome { get; set; }
        public string Titulo { get; set; }
        public int Id { get; set; }
        public string Url { get; set; }

    }

}