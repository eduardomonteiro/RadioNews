using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(EventosMetaData))]
    public partial class Eventos
    {
    }

    public class EventosMetaData
    {
        [Key]
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Required]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Ativo")]
        public bool Liberado { get; set; }

        public bool Excluido { get; set; }

        [Required]
        [AllowHtml]
        public string Texto { get; set; }

        [StringLength(250)]
        public string Imagem { get; set; }

        public virtual List<Acontecimentos> Acontecimentos { get; set; }
        
    }
}