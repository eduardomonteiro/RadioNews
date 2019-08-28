using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(TagsMetaData))]
    public partial class Tags
    {

    }

    public class TagsMetaData
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [Display(Name = "Data de cadastro")]
        public System.DateTime DataCadastro { get; set; }

    }
}