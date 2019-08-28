
namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Acontecimentos
    {
        [Key]
        public int Id { get; set; }
       
        [Required]
        [StringLength(500)]
        public string Local { get; set; }

        [Required]
        public DateTime Data { get; set; }

        public bool Liberado { get; set; }

        public DateTime DataCadastro { get; set; }

        [Required]
        [StringLength(5)]
        public string HoraInicio { get; set; }

        [Required]
        [StringLength(5)]
        public string HoraFim { get; set; }

        public virtual bool Realizado
        {
            get
            {

                return Vencido && FlickrId != null;

            }
        }
        public virtual bool Vencido
        {
            get
            {

                return DateTime.Now.AddHours(23).AddMinutes(59).AddSeconds(59) > Data;

            }
        }


        public virtual string Url
        {
            get
            {
                var url = "/CompanyName-no-ponto/acontecimento/" + Id + "/" + Evento.Slug;

                return url;
            }
        }
        
        [Required]
        public int EventoId { get; set; }

        [Required]
        public virtual Eventos Evento { get; set; }

        public Acontecimentos()
        {
            DataCadastro = DateTime.Now;
        }

        public bool Excluido { get; set; }

        public string FlickrId { get; set; }
    }
}