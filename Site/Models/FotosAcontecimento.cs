
namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class FotosAcontecimento
    {
        [Key]
        public int Id { get; set; }

        public DateTime DataCadastro { get; set; }
        
        public string Legenda { get; set; }
        
        [Required]
        public string Imagem { get; set; }
        
        [Required]
        public int AcontecimentoId { get; set; }
        
        [Required]
        public virtual Acontecimentos Acontecimento { get; set; }
        
        public FotosAcontecimento()
        {
            DataCadastro = DateTime.Now;
        }
    }
}