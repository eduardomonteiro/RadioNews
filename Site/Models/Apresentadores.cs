namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Apresentadores
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Apresentadores()
        {
            Programacao = new HashSet<Programacao>();
        }

        public int id { get; set; }

        [StringLength(100)]
        public string nome { get; set; }

        [StringLength(100)]
        public string fotoInterna { get; set; }

        [StringLength(100)]
        public string fotoExterna { get; set; }

        public string chamada { get; set; }

        public string texto { get; set; }

        [Required]
        [StringLength(200)]
        public string chave { get; set; }

        public bool excluido { get; set; }

        [StringLength(100)]
        public string Instagram { get; set; }

        [StringLength(50)]
        public string twitter { get; set; }

        public bool? participanteConvidado { get; set; }

        [StringLength(50)]
        public string facebook { get; set; }

        public DateTime DataCadastro { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Programacao> Programacao { get; set; }
    }
}
