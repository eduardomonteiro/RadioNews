namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Ouvintes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Ouvintes()
        {
            OuvintesArquivos = new HashSet<OuvintesArquivos>();
        }

        public int id { get; set; }

        [Required]
        public string nome { get; set; }

        [Required]
        [StringLength(200)]
        public string email { get; set; }

        [Required]
        [StringLength(100)]
        public string telefone { get; set; }

        public int? regiaoId { get; set; }

        public int? cidade_id { get; set; }

        [StringLength(50)]
        public string bairro { get; set; }

        [Required]
        [StringLength(200)]
        public string endereco { get; set; }

        [Required]
        [StringLength(200)]
        public string assunto { get; set; }

        [Required]
        [StringLength(100)]
        public string horario { get; set; }

        [Column(TypeName = "date")]
        public DateTime data { get; set; }

        [Required]
        public string comentario { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool liberado { get; set; }

        public bool excluido { get; set; }
        
        public virtual Regioes Regioes { get; set; }

        public virtual cidade cidade { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OuvintesArquivos> OuvintesArquivos { get; set; }
    }
}
