using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(LocaisMetaData))]
    public partial class Secoes_Locais
    {

    }

    public class LocaisMetaData
    {
        public int Id { get; set; }
        
        [Display(Name = "Seção")]
        public int SecaoId { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Tipo { get; set; }

        [StringLength(150, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Descricao { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Endereco { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Latitude")]
        public string Latitude { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Longitude")]
        public string Longitude { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }

        [ScaffoldInList(false)]
        public virtual Especiais_Secoes Especiais_Secoes { get; set; }
    }
}