using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Site.Models
{
    [Table("Comentarios")]
    public class Comentario
    {
        [Key]
        public int Id { get; set; }
        public string Texto { get; set; }
        public string CPF { get; set; }
        public string Nome { get; set; }
        public string IPAcesso { get; set; }
        public string UrlFacebook { get; set; }

        public int Gostei { get; set; }
        public int NaoGostei { get; set; }

        public DateTime DataCadastro { get; set; }

        public int? IdComentarioRaiz { get; set; }
        public int IdNoticia { get; set; }

        public bool Bloqueado { get; set; }

        [ForeignKey(nameof(IdComentarioRaiz))]
        public virtual Comentario ComentarioRaiz { get; set; }

        [ForeignKey(nameof(IdNoticia))]
        public virtual Noticias Noticia { get; set; }

        public virtual ICollection<Comentario> Respostas { get; set; }

        public bool Resposta {
            get
            {
                return ComentarioRaiz != null;
            }
        }

        public void RemoverGostar()
        {
            if (Gostei > 0)
            {
                Gostei--;
            }
        }
        public void RemoverNaoGostar()
        {
            if (NaoGostei > 0)
            {
                NaoGostei--;
            }
        }

        public void Gostar()
        {
            Gostei++;
        }
        public void NaoGostar()
        {
            NaoGostei++;
        }

    }
}