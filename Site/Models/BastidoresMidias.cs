namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BastidoresMidias
    {
        public int id { get; set; }

        public int? idGaleria { get; set; }

        [StringLength(500)]
        public string midia { get; set; }

        public string legenda { get; set; }

        public bool video { get; set; }

        public bool ativo { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        public virtual Bastidores Bastidores { get; set; }
    }
}
