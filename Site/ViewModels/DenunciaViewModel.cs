using PagedList;
using Site.Enums;
using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class DenunciaViewModel
    {
      
        public IPagedList<Ouvintes> paginacao { get; set; }

    }
    /*
    public class DenunciaModel
    {
        public int id { get; set; }
        public string nome { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public int regiaoId { get; set; }
        public string bairro { get; set; }
        public string endereco { get; set; }
        public string assunto { get; set; }
        public string horario { get; set; }
        public DateTime data { get; set; }
        public string comentario { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool liberado { get; set; }
        public bool excluido { get; set; }
        public virtual Models.Regioes Regioes { get; set; }
        public virtual ICollection<OuvintesArquivos> OuvintesArquivos { get; set; }
    }
    */
}