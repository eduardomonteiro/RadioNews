using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class NoticiasPAD
    {
        public int Id { get; set; }

        [Required]
        [StringLength(85)]
        public string Titulo { get; set; }

        [StringLength(350)]
        public string Chamada { get; set; }

        [StringLength(15)]
        public string Categoria { get; set; }

        public string Foto { get; set; }

        public bool Excluido { get; set; }
        public DateTime DataCadastro { get; set; }

        public int? ApoioId { get; set; }
        [ForeignKey(nameof(ApoioId))]
        public ApoioPAD Apoio { get; set; }

    }
}