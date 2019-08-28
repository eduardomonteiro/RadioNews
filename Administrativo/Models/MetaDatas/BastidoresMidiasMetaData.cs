using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(BastidoresMidiasMetaData))]
    public partial class BastidoresMidias
    {

    }

    public class BastidoresMidiasMetaData
    {
        public int id { get; set; }

        [StringLength(400, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Este campo é de preenchimento obrigatório.")]
        public string legenda { get; set; }

        [StringLength(500, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Video")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string midia { get; set; }

        public bool video { get; set; }
        public bool ativo { get; set; }
        public bool excluido { get; set; }
        public System.DateTime dataCadastro { get; set; }

        public virtual Bastidores Bastidores { get; set; }

    }
}