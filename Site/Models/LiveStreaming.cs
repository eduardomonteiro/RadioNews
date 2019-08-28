using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("LiveStreamings")]
    public class LiveStreaming
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Legenda { get; set; }

        [Required]
        public string CodigoTransmissao { get; set; }

        public DateTime DataCadastro { get; set; }
        public DateTime? DataFinalizacao { get; set; }

        public bool Excluido { get; set; }
        public bool Ativo { get; set; }
                        
        public virtual Noticias Noticia { get; set; }
    }
}