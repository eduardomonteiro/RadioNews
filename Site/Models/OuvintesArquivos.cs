namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OuvintesArquivos
    {
        public int id { get; set; }

        public int idOuvinteDenuncia { get; set; }

        [Required]
        [StringLength(150)]
        public string Arquivo { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Ouvintes Ouvintes { get; set; }
    }
}
