using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("Audios")]
    public class Audio
    {
        public int Id { get; set; }
        public string Legenda { get; set; }
        public string Url { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool Excluido { get; set; }
        public bool Liberado { get; set; }
        
        [ForeignKey("Colecao")]
        public int? ColecaoId { get; set; }

        public virtual ColecaoAudios Colecao { get; set; }
    }
}