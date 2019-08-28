namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Banners
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Banners()
        {
            BannersVisualizacoesCliques = new HashSet<BannersVisualizacoesCliques>();
            AreaBanner = new HashSet<AreaBanner>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [StringLength(200)]
        public string Arquivo { get; set; }

        [StringLength(200)]
        public string Arquivo2 { get; set; }

        [StringLength(200)]
        public string Anunciante { get; set; }

        [StringLength(300)]
        public string Link { get; set; }

        public bool TipoArquivo { get; set; }

        public int? Exibicoes { get; set; }

        public int? Cliques { get; set; }

        public bool Excluido { get; set; }

        public bool Liberado { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataCadastro { get; set; }

        public int? Local { get; set; }

        [StringLength(500)]
        public string Html { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BannersVisualizacoesCliques> BannersVisualizacoesCliques { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AreaBanner> AreaBanner { get; set; }
    }
}
