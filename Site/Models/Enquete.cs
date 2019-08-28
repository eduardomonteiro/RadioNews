namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Enquete")]
    public partial class Enquete
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Enquete()
        {
            Respostas = new HashSet<Respostas>();
        }

        public int id { get; set; }

        [StringLength(500)]
        public string pergunta { get; set; }

        public bool ativa { get; set; }

        public bool excluido { get; set; }

        public bool destaque { get; set; }

        [Required]
        [StringLength(100)]
        public string chave { get; set; }

        public DateTime dataCadastro { get; set; }

        public DateTime? dataInicioVotacao { get; set; }

        public DateTime? dataFimVotacao { get; set; }

        public DateTime? dataFimResultado { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Respostas> Respostas { get; set; }
    }
}
