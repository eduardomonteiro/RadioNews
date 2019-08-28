namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Editoriais_Equipe
    {
        public int id { get; set; }

        public int? EditoriaisId { get; set; }

        [Required]
        [StringLength(200)]
        public string nome { get; set; }

        public string texto { get; set; }

        [StringLength(100)]
        public string funcao { get; set; }

        [StringLength(200)]
        public string imagem { get; set; }

        [StringLength(200)]
        public string imagemVertical { get; set; }

        [StringLength(100)]
        public string instagram { get; set; }

        [StringLength(100)]
        public string facebook { get; set; }

        [StringLength(100)]
        public string twitter { get; set; }

        public bool excluido { get; set; }

        [Required]
        [StringLength(260)]
        public string chave { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Editoriais Editoriais { get; set; }
    }
}
