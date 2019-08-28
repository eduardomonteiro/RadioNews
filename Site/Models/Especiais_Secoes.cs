namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Especiais_Secoes
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Especiais_Secoes()
        {
            Secoes_Locais = new HashSet<Secoes_Locais>();
        }

        public int Id { get; set; }

        public int EditoriaId { get; set; }

        [Required]
        [StringLength(100)]
        public string Titulo { get; set; }

        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Editoriais Editoriais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Secoes_Locais> Secoes_Locais { get; set; }
    }
}
