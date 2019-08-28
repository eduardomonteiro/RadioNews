using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(ColecoesAudiosMetaData))]
    public partial class ColecoesAudios
    {
    }
    public class ColecoesAudiosMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Título")]
        public string Titulo { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        public int? CategoriaId { get; set; }

        public virtual ICollection<Audios> Audios { get; set; }
        public virtual CategoriasAudios CategoriasAudios { get; set; }
    }


}