namespace Site.Models
{
    using Site.Enums;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Radios
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        [StringLength(400)]
        public string Stream1 { get; set; }

        [StringLength(400)]
        public string Stream2 { get; set; }
        
        public TipoStream TipoStream1 { get; set; }

        public TipoStream TipoStream2 { get; set; }

        public string Imagem { get; set; }

        [StringLength(150)]
        public string Chave { get; set; }

        public bool Ativo { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual ICollection<cidade> cidades { get; set; }
    }
}
