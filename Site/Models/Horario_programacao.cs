namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Horario_programacao
    {
        public int id { get; set; }

        public int? idPrograma { get; set; }

        public int? diaSemana { get; set; }

        [StringLength(10)]
        public string horario { get; set; }

        public virtual Programacao Programacao { get; set; }
    }
}
