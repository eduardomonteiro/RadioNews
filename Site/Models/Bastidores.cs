namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bastidores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bastidores()
        {
            BastidoresMidias = new HashSet<BastidoresMidias>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string titulo { get; set; }

        public string chamada { get; set; }

        public string texto { get; set; }

        [Required]
        [StringLength(370)]
        public string chave { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        public bool ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BastidoresMidias> BastidoresMidias { get; set; }
    }
}
