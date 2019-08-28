namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Programacao")]
    public partial class Programacao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Programacao()
        {
            Horario_programacao = new HashSet<Horario_programacao>();
            Materia = new HashSet<Materia>();
            Apresentadores = new HashSet<Apresentadores>();
        }

        public int id { get; set; }

        public int editoriaId { get; set; }

        [Required]
        [StringLength(150)]
        public string nome { get; set; }

        [StringLength(100)]
        public string logo { get; set; }

        [StringLength(100)]
        public string imagem { get; set; }

        public int periodo { get; set; }

        public string texto { get; set; }

        public string chamada { get; set; }

        public int? diaSemana { get; set; }

        public TimeSpan? horario { get; set; }

        public DateTime? dataCadastro { get; set; }

        public bool excluido { get; set; }

        [Required]
        [StringLength(150)]
        public string chave { get; set; }

        public virtual Editoriais Editoriais { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Horario_programacao> Horario_programacao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Materia> Materia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Apresentadores> Apresentadores { get; set; }
    }
}
