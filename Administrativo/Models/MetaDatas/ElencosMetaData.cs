using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(ElencosMetaData))]
    public partial class Elencos
    {

    }
    public partial class ElencosMetaData
    {
       
        public int Id { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public int TimeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]      
        [MaxLength(150,ErrorMessage ="{0} permite no máximo {1} caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Foto")]
        public string Foto { get; set; }

        [Display(Name = "Posição")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Posicao { get; set; }

        public bool Ativo { get; set; }

        [ScaffoldColumn(false)]
        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }


    }
}