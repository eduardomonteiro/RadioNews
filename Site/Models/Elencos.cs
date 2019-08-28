namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;


    [Table("Elencos")]
    public partial class Elencos
    {
        public int Id { get; set; }

        public int TimeId { get; set; }

        [StringLength(300)]
        public string Foto { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Posicao { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Times Times { get; set; }
    }
}
