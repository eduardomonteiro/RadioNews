namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Materia")]
    public partial class Materia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Materia()
        {
            MateriaNovidades = new HashSet<MateriaNovidades>();
        }

        public int id { get; set; }

        public int idProgramacao { get; set; }

        public int idAssunto { get; set; }

        [Required]
        [StringLength(200)]
        public string titulo { get; set; }

        [Required]
        [StringLength(255)]
        public string chamada { get; set; }

        [Required]
        public string texto { get; set; }

        [Required]
        [StringLength(200)]
        public string foto { get; set; }

        public int status { get; set; }

        [Required]
        [StringLength(250)]
        public string chave { get; set; }

        public bool excluido { get; set; }

        public DateTime dataCadastro { get; set; }

        public virtual AssuntoMateria AssuntoMateria { get; set; }

        public virtual Programacao Programacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MateriaNovidades> MateriaNovidades { get; set; }
    }
}
