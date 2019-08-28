using Administrativo.Models.CustomValidation;
using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(MateriaMetaData))]
    public partial class Materia
    {

    }

    public class MateriaMetaData
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        [Display(Name = "Programa")]
        public int idProgramacao { get; set; }

        [Display(Name = "Assunto")]
        public int idAssunto { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string titulo { get; set; }

        [Display(Name = "Chamada")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [AllowHtml]
        public string chamada { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [AllowHtml]
        public string texto { get; set; }

        [Display(Name = "Foto (140x140px)")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string foto { get; set; }

        [Display(Name = "Status")]
        public int status { get; set; }

        [Display(Name = "Data de cadastro")]
        public System.DateTime dataCadastro { get; set; }

        public bool excluido { get; set; }

        public string chave { get; set; }

    }
}