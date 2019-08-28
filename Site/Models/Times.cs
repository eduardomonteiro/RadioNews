namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Times")]
    public partial class Times
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Times()
        {
            Elencos = new HashSet<Elencos>();
        }

        public int Id { get; set; }

        public int EditoriaId { get; set; }

        [Required]
        [StringLength(150)]
        public string Nome { get; set; }

        [StringLength(150)]
        public string Escudo { get; set; }

        [Required]
        [StringLength(300)]
        public string Chave { get; set; }

        [StringLength(10)]
        public string Cor { get; set; }

        [Column(TypeName = "text")]
        public string Titulos { get; set; }

        [Column(TypeName = "text")]
        public string Informacoes { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Editoriais Editoriais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Elencos> Elencos { get; set; }
    }
}
