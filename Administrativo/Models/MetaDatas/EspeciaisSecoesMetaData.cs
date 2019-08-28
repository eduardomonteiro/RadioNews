using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{

    [MetadataType(typeof(EspeciaisSecoesMetaData))]
    public partial class Especiais_Secoes
    {

    }

    public class EspeciaisSecoesMetaData
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Ativo")]
        public bool Ativo { get; set; }

        [Required]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataCadastro { get; set; }

        [ScaffoldColumn(false)]
        [ScaffoldInList(false)]
        public bool Excluido { get; set; }
    }
}