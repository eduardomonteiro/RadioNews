namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Secoes_Locais
    {
        public int Id { get; set; }

        public int SecaoId { get; set; }

        [StringLength(100)]
        public string Tipo { get; set; }

        [Required]
        [StringLength(150)]
        public string Titulo { get; set; }

        [StringLength(255)]
        public string Chave { get; set; }

        [StringLength(250)]
        public string Imagem { get; set; }

        [Column(TypeName = "text")]
        public string Descricao { get; set; }

        [StringLength(350)]
        public string Endereco { get; set; }

        [StringLength(50)]
        public string Latitude { get; set; }

        [StringLength(50)]
        public string Longitude { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Especiais_Secoes Especiais_Secoes { get; set; }
    }
}
