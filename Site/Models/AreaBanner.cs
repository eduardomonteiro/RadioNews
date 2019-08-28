namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AreaBanner")]
    public partial class AreaBanner
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AreaBanner()
        {
            Banners = new HashSet<Banners>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Titulo { get; set; }

        [Required]
        [StringLength(200)]
        public string Descricao { get; set; }

        [Required]
        [StringLength(50)]
        public string chave { get; set; }

        [StringLength(50)]
        public string Tamanho { get; set; }

        [StringLength(50)]
        public string Tamanho2 { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool? Ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Banners> Banners { get; set; }
    }
}
