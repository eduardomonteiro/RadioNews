using PagedList;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class CategoriaAudiosViewModel
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual IPagedList<ColecaoAudios> Colecoes { get; set; }
    }
}