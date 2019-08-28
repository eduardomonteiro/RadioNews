namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TrabalheConosco")]
    public partial class TrabalheConosco
    {
        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string nome { get; set; }

        [Required]
        [StringLength(50)]
        public string celular { get; set; }

        [Required]
        [StringLength(50)]
        public string telefone { get; set; }

        public int sexo { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        [Required]
        [StringLength(200)]
        public string cidade { get; set; }

        [Required]
        [StringLength(300)]
        public string areaPretencao { get; set; }

        public DateTime dataNascimento { get; set; }

        [Required]
        [StringLength(200)]
        public string curriculo { get; set; }

        [Required]
        [StringLength(200)]
        public string email { get; set; }

        public DateTime dataCadastro { get; set; }
    }
}
