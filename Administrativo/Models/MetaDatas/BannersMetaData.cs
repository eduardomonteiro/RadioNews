using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(BannersMetaData))]
    public partial class Banners
    {

    }
    public partial class BannersMetaData
    {
        
        [Display(Name = "Arquivo")]
        //[Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Arquivo { get; set; }

        [StringLength(200, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Anunciante")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Anunciante { get; set; }

        public string Link { get; set; }

        public bool TipoArquivo { get; set; }

        public bool Liberado { get; set; }
        public string Html { get; set; }

        /*
        [Display(Name = "Tipo de Banner")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public int? Local { get; set; }
 */

        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public System.DateTime DataInicio { get; set; }

        
        [Display(Name = "Data Fim")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [DataType(DataType.Date)]
        public System.DateTime DataFim { get; set; }

        

    }
}