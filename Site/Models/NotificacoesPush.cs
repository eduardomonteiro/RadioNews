using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("NotificacoesPush")]
    public class NotificacoesPush
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "O título não pode ter mais do que 30 caracteres.")]
        public string Title { get; set; }
        [Required]
        [MaxLength(250, ErrorMessage = "A mensagem não pode ter mais do que 250 caracteres.")]
        public string Message { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Enviado { get; set; }
        public string RetornoAPI { get; set; }

        public int? idGrupo { get; set; }
        public int? idNoticia { get; set; }

        public virtual GruposPush GrupoPush { get; set; }
        public virtual Noticias Noticia { get; set; }
    }
}