namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Equipe")]
    public partial class Equipe
    {
        public int id { get; set; }

        [Required]
        [StringLength(200)]
        public string nome { get; set; }

        [Column(TypeName = "text")]
        public string texto { get; set; }

        [StringLength(100)]
        public string funcao { get; set; }

        [StringLength(200)]
        public string imagem { get; set; }

        [StringLength(100)]
        public string instagram { get; set; }

        [StringLength(100)]
        public string facebook { get; set; }

        [StringLength(100)]
        public string email { get; set; }

        [StringLength(50)]
        public string telefone { get; set; }

        [StringLength(100)]
        public string twitter { get; set; }

        public short tipo { get; set; }

        public bool excluido { get; set; }

        [StringLength(260)]
        public string chave { get; set; }

        public int ordem { get; set; }

        public DateTime DataCadastro { get; set; }
    }
}
