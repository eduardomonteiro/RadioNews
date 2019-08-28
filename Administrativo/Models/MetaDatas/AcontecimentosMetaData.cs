using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(AcontecimentosMetaData))]
    public partial class Acontecimentos
    {
    }

    public class AcontecimentosMetaData
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Local { get; set; }

        [Required]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }
        
        [Display(Name = "Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Data { get; set; }

        public bool Excluido { get; set; }
        
        [Required]
        [StringLength(5)]
        [Display(Name = "Hora de Início")]
        public string HoraInicio { get; set; }

        [Required]
        [StringLength(5)]
        [Display(Name = "Hora Final")]
        public string HoraFim { get; set; }

        [Display(Name = "Album do Flickr")]
        public string FlickrId { get; set; }

        [Required]
        public int EventoId { get; set; }

        [Display(Name = "Ativo")]
        public bool Liberado { get; set; }

        public virtual Eventos Eventos { get; set; }

    }
}