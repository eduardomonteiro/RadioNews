namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FaleConosco")]
    public partial class FaleConosco
    {
        public int id { get; set; }

        [StringLength(200)]
        public string assunto { get; set; }

        [Required]
        [StringLength(300)]
        public string nome { get; set; }

        [Required]
        [StringLength(300)]
        public string email { get; set; }

        [Required]
        [StringLength(100)]
        public string estado { get; set; }

        [Required]
        [StringLength(150)]
        public string cidade { get; set; }

        [Required]
        [StringLength(100)]
        public string celular { get; set; }

        [Required]
        [StringLength(100)]
        public string telefone { get; set; }

        [Required]
        public string mensagem { get; set; }

        public DateTime dataCadastro { get; set; }

        public string resposta { get; set; }

        public DateTime? dataResposta { get; set; }

        public bool excluido { get; set; }

        public byte lida { get; set; }
    }
}
