namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Respostas
    {
        public int id { get; set; }

        public int? idEnquete { get; set; }

        public string resposta { get; set; }

        public bool excluido { get; set; }

        public bool? certa { get; set; }

        public int votos { get; set; }

        public virtual Enquete Enquete { get; set; }
    }
}
