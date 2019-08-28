namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Galeria")]
    public partial class Galeria
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Galeria()
        {
            Midias = new HashSet<Midias>();
            Noticias = new HashSet<Noticias>();
            Editoriais = new HashSet<Editoriais>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(300)]
        public string titulo { get; set; }

        public string chamada { get; set; }

        public string texto { get; set; }

        public bool Fixa { get; set; }

        [Required]
        [StringLength(370)]
        public string chave { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        public bool ativo { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Midias> Midias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noticias> Noticias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Editoriais> Editoriais { get; set; }
    }
}
