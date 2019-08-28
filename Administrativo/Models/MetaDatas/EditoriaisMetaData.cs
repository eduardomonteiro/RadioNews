using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(EditoriaisMetaData))]
    public partial class Editoriais
    {

    }

    public class EditoriaisMetaData
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string nome { get; set; }

        
        [Display(Name = "Especial")]
        public bool especial { get; set; }

        [Display(Name = "Ativo")]
        public bool ativo { get; set; }
      

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }
    }
}