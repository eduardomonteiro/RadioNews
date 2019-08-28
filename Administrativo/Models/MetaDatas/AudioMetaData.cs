using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(AudiosMetaData))]
    public partial class Audios
    {

    }
    public class AudiosMetaData
    {
        public int Id { get; set; }
        public string Legenda { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }

        public string Url { get; set; }

        public int? ColecaoId { get; set; }
        
        public virtual ColecoesAudios ColecoesAudios { get; set; }
    }


}