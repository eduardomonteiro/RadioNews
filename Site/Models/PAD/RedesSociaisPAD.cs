using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class RedesSociaisPAD
    {
        public int Id { get; set; }

        [Required]
        [StringLength(70)]
        public string Chamada { get; set; }

        [Required]
        [StringLength(40)]
        public string Hashtag { get; set; }

        public int TipoRede { get; set; }

        public string Foto { get; set; }

        public bool Excluido { get; set; }
        public DateTime DataCadastro { get; set; }

        public int? ApoioId { get; set; }
        [ForeignKey(nameof(ApoioId))]
        public ApoioPAD Apoio { get; set; }
    }
}