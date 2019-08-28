namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Midias
    {
        public int id { get; set; }

        public int? idGaleria { get; set; }

        [StringLength(500)]
        public string midia { get; set; }

        public string legenda { get; set; }

        public bool video { get; set; }

        public bool excluido { get; set; }

        public virtual Galeria Galeria { get; set; }
    }
}
