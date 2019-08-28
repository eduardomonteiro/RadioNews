using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{

    [MetadataType(typeof(ColunistaMetaData))]
    public partial class Colunista
    {

    }

    public class ColunistaMetaData
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        public int Ordem { get; set; }

        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string nome { get; set; }

        [ScaffoldInList(false)]
        [StringLength(150, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="Preencha um email válido!")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string email { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [AllowHtml]
        public string descricao { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Foto")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string foto { get; set; }

        //[ScaffoldInList(false)]
        //[Display(Name = "Miniatura")]
        //[Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        //public string fotoMini { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Ativo")]
        public bool liberado { get; set; }

        [Required]
        [Display(Name = "Data de Cadastro")]
        public Nullable<System.DateTime> dataCadastro { get; set; }

        [ScaffoldColumn(false)]
        [ScaffoldInList(false)]
        public bool excluido { get; set; }
    }
}