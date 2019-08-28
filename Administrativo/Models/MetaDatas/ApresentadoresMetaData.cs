using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{

    [MetadataType(typeof(ApresentadoresMetaData))]
    public partial class Apresentadores
    {

    }

    public class ApresentadoresMetaData
    {
        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string nome { get; set; }


        [Display(Name = "Foto (Programação - Detalhes) 140x140")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string fotoExterna { get; set; }

        [Display(Name = "Função")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [AllowHtml]
        public string chamada { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [AllowHtml]
        public string texto { get; set; }

        [Display(Name = "Data de Cadastro")]
        public string DataCadastro { get; set; }

        [Display(Name = "Twitter")]
        public string twitter { get; set; }

        [Display(Name = "Facebook")]
        public string facebook { get; set; }

    }
}



