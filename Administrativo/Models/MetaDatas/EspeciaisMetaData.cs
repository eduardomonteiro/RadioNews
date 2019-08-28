using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(EspeciaisMetaData))]
    public partial class Especiais
    {

    }

    public class EspeciaisMetaData
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Título")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [ScaffoldInList(false)]
        public bool Ativo { get; set; }

        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [ScaffoldInList(false)]
        public string Chave { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }
    }
}