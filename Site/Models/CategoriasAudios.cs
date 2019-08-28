using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("CategoriasAudios")]
    public class CategoriaAudios
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual IEnumerable<ColecaoAudios> Colecoes { get; set; }
    }
}