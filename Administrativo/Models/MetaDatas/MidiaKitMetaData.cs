using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(MidiaKitMetaData))]
    public partial class MidiaKit
    {

    }
    public partial class MidiaKitMetaData
    {
        
        public int Id { get; set; }
        
        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [Display(Name = "Arquivo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Arquivo { get; set; }
        
        [Display(Name = "Miniatura")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Miniatura { get; set; }

        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        public bool Excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }

    }
}