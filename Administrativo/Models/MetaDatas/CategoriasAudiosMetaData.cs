using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(CategoriaAudiosMetaData))]
    public partial class CategoriasAudios
    {

    }
    public class CategoriaAudiosMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        public virtual IEnumerator<ColecoesAudios> ColecoesAudios { get; set; }
    }
}