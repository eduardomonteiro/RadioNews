namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public class Horoscopo
    {
        public Horoscopo()
        {
            DataAtualizacao = DateTime.Now;
        }

        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Signo { get; set; }

        [Required]
        public string Audio { get; set; }

        public string Descricao { get; set; }

        [Required]
        public DateTime DataAtualizacao { get; set; }
    }
}