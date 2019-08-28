
namespace Site.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.RegularExpressions;
    using Site.Helpers;

    public class Eventos
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250)]
        public string Titulo { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }

        public bool Liberado { get; set; }

        [Required]
        public string Texto { get; set; }

        [Required]
        [StringLength(250)]
        public string Imagem { get; set; }

        public virtual List<Acontecimentos> Acontecimentos { get; set; }

        public bool Excluido { get; set; }

        public virtual string Slug
        {
            get
            {
                string slug = Titulo.RemoveDiacritics().ToLower();
                slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
                slug = Regex.Replace(slug, @"\s+", " ").Trim();
                slug = slug.Substring(0, slug.Length <= 45 ? slug.Length : 45).Trim();
                slug = Regex.Replace(slug, @"\s", "-");

                return slug;
            }
        }

        public virtual string Url
        {
            get
            {
                var url = "/CompanyName-no-ponto/evento/" + Id + "/" + Slug;

                return url;
            }
        }

        public Eventos()
        {
            DataCadastro = DateTime.Now;
            Liberado = false;
        }
    }
}