namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Promocoes_Participantes
    {
        public int Id { get; set; }

        public int PromocaoCodigo { get; set; }

        public int ParticipanteCodigo { get; set; }

        public string CampoExtra { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Promocoes Promocoes { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}
