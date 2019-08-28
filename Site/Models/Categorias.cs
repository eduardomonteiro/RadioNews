namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Categorias
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Categorias()
        {
            Noticias = new HashSet<Noticias>();
        }

        public int Id { get; set; }

        public int? EditoriaId { get; set; }

        public bool destaque { get; set; }

        [Required]
        [StringLength(50)]
        public string Titulo { get; set; }

        public bool Excluido { get; set; }

        public int Ordem { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Editoriais Editoriais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Noticias> Noticias { get; set; }
    }
}
