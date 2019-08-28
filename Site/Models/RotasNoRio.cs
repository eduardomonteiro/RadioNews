namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RotasNoRio")]
    public partial class RotasNoRio
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; }

        [Column(TypeName = "text")]
        public string Texto { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime? Atualizacao { get; set; }
    }
}
