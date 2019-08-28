namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Especiais_Modelos
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Especiais_Modelos()
        {
            Editoriais = new HashSet<Editoriais>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Tipo { get; set; }

        [StringLength(150)]
        public string Action { get; set; }

        public bool TemSecao { get; set; }

        public DateTime DataCadastro { get; set; }

        public bool especial { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Editoriais> Editoriais { get; set; }
    }
}
