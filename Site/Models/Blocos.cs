namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Blocos
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [StringLength(200)]
        public string Chave { get; set; }

        [StringLength(250)]
        public string Imagem { get; set; }

        [StringLength(50)]
        public string Local { get; set; }

        public DateTime Data { get; set; }

        [Column(TypeName = "text")]
        [Required]
        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
