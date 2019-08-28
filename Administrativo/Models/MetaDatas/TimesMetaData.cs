using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(TimesMetaData))]
    public partial class Times
    {

    }
    public partial class TimesMetaData
    {
       
        public int Id { get; set; }

        [Display(Name = "Noticia/Editoria")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public int EditoriaId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]      
        [MaxLength(150,ErrorMessage ="{0} permite no máximo {1} caracteres")]
        public string Nome { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Escudo do Time")]
        public string Escudo { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Cor")]
        public string Cor { get; set; }

        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Informações")]
        public string Informacoes { get; set; }

        [AllowHtml]
        [ScaffoldInList(false)]
        [Display(Name = "Títulos")]
        public string Titulos { get; set; }


        public bool Ativo { get; set; }

        [ScaffoldColumn(false)]
        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }


    }
}