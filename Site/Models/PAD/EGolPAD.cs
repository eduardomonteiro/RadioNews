using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    public class EGolPAD
    {
        public int Id { get; set; }

        [Required]
        [StringLength(5)]
        public string Placar { get; set; }

        [Required]
        [StringLength(20)]
        public string Jogador { get; set; }

        [Required]
        [StringLength(15)]
        public string HashTag { get; set; }

        [Required]
        [StringLength(15)]
        public string HoraDoGol { get; set; }

        [Required]
        [StringLength(100)]
        public string BgColor { get; set; }

        public bool GolMandante { get; set; }

        public bool Excluido { get; set; }
        public DateTime DataCadastro { get; set; }

        public int? ApoioId { get; set; }
        [ForeignKey(nameof(ApoioId))]
        public ApoioPAD Apoio { get; set; }

        public int MandanteId { get; set; }
        [ForeignKey(nameof(MandanteId))]
        public TimesPAD Mandante { get; set; }

        public int VisitanteId { get; set; }
        [ForeignKey(nameof(VisitanteId))]
        public TimesPAD Visitante { get; set; }
    }
}