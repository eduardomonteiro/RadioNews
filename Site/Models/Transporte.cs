namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transporte")]
    public partial class Transporte
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(200)]
        public string CssClass { get; set; }

        [Required]
        [StringLength(10)]
        public string Status { get; set; }

        [StringLength(500)]
        public string Texto { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? Atualizacao { get; set; }
    }
}
