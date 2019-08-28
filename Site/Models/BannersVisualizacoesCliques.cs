namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class BannersVisualizacoesCliques
    {
        public int Id { get; set; }

        public int CodigoBanner { get; set; }

        public bool Visualizacao { get; set; }

        public bool Clique { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Banners Banners { get; set; }
    }
}
