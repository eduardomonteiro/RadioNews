using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;
using PagedList;

namespace Site.ViewModels
{
    public class GaleriasViewModel
    {

        public int id { get; set; }

        public string titulo { get; set; }

        public string chamada { get; set; }

        public string texto { get; set; }

        public bool Fixa { get; set; }

        public string chave { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        public bool ativo { get; set; }

        public virtual ICollection<Midias> Midias { get; set; }

        public virtual ICollection<Noticias> Noticias { get; set; }

        public virtual ICollection<Editoriais> Editoriais { get; set; }
    }
}