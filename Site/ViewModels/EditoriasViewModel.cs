using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Site.Models;

namespace Site.ViewModels
{
    public class EditoriasViewModel
    {
        public EditorialViewModel Editoria { get; set; }
        public List<NoticiasViewModel> Destaques { get; set; }
        public Banners Banner { get; set; }
        public BannerViewModel BannerPrincipal { get; set; }
        public IPagedList<NoticiasViewModel> paginacao { get; set; }
    }

    public class EditorialViewModel
    {
        public int id { get; set; }

        public string nome { get; set; }

        public string Action { get; set; }

        public bool especial { get; set; }

        public int? modeloEspecial { get; set; }

        public bool ativo { get; set; }

        public bool esportes { get; set; }

        public bool excluido { get; set; }

        public string chave { get; set; }

        public DateTime DataCadastro { get; set; }

        public ICollection<Categorias> Categorias { get; set; }

        public ICollection<Editoriais_Equipe> Editoriais_Equipe { get; set; }

        public Especiais_Modelos Especiais_Modelos { get; set; }

        public ICollection<Especiais_Secoes> Especiais_Secoes { get; set; }
                
        public ICollection<Programacao> Programacao { get; set; }

        public ICollection<Galeria> Galeria { get; set; }

        public ICollection<NoticiasViewModel> Noticias { get; set; }

        public ICollection<Times> Times { get; set; }

    }

}