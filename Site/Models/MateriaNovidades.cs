namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class MateriaNovidades
    {
        public int id { get; set; }

        public int idMateria { get; set; }

        [Required]
        [StringLength(100)]
        public string status { get; set; }

        [Column(TypeName = "date")]
        public DateTime dataPrograma { get; set; }

        [Required]
        [StringLength(200)]
        public string novidades { get; set; }

        [StringLength(50)]
        public string audio { get; set; }

        public DateTime dataCadastro { get; set; }

        public bool excluido { get; set; }

        public virtual Materia Materia { get; set; }
    }
}
