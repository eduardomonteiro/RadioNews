using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(BlocosMetaData))]
    public partial class Blocos
    {

    }
    public partial class BlocosMetaData
    {       
        public int Id { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]      
        [MaxLength(50,ErrorMessage ="{0} permite no máximo {1} caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Local")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [MaxLength(50, ErrorMessage = "{0} permite no máximo {1} caracteres")]
        public string Local { get; set; }

        [Display(Name = "Imagem")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Imagem { get; set; }

        [Display(Name = "Data")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [DataType(DataType.Date)]
        public System.DateTime Data { get; set; }

        [Display(Name = "Descrição")]
        [AllowHtml]
        public string Descricao { get; set; }

        public bool Ativo { get; set; }

        [ScaffoldColumn(false)]
        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }


    }
}