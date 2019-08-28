namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class RedesSociais
    {
        public int Id { get; set; }

        public int TipoRede { get; set; }

        [StringLength(400)]
        public string Imagem { get; set; }

        public string Texto { get; set; }

        [StringLength(800)]
        public string Link { get; set; }

        public DateTime? Data { get; set; }

        public DateTime DataCadastro { get; set; }

        public string Video { get; set; }

        public string PostId { get; set; }
    }
}
