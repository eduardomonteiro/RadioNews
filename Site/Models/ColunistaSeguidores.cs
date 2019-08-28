namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class ColunistaSeguidores
    {
        public int Id { get; set; }

        public int ColunistaId { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public DateTime DataCadastro { get; set; }

        public virtual Colunista Colunista { get; set; }
    }
}
