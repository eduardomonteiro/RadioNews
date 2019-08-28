using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("ColecoesAudios")]
    public class ColecaoAudios
    {
        public int Id { get; set; }
        public string Titulo { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }
        
        [ForeignKey("Categoria")]
        public int? CategoriaId { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual IEnumerable<Audio> Audios { get; set; }
        public virtual CategoriaAudios Categoria { get; set; }
    }
}