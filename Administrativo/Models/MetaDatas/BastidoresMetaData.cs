using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(BastidoresMetaData))]
    public partial class Bastidores
    {

    }

    public class BastidoresMetaData
    {
        public int id { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string titulo { get; set; }

        [AllowHtml]
        [StringLength(400, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Chamada")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string chamada { get; set; }

        [AllowHtml]
        [StringLength(400, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Texto")]
        public string texto { get; set; }

        [Display(Name = "Ativa no site?")]
        public string ativo { get; set; }

        [Display(Name = "Excluído")]
        public bool excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public Nullable<System.DateTime> dataCadastro { get; set; }
        public virtual ICollection<BastidoresMidias> BastidoresMidias { get; set; }
        
    }
}