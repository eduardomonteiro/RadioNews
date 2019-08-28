using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(EquipesMetaData))]
    public partial class Equipe
    {

    }


    public class EquipesMetaData
    {
        public int id { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string nome { get; set; }

        [Display(Name = "Texto")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string texto { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Função")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string funcao { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage ="{0} não é um email válido")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string email { get; set; }
        
        [StringLength(50, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Telefone")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string telefone { get; set; }

        [Display(Name = "Imagem")]
        public string imagem { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Instagram")]
        
        public string instagram { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Facebook")]
        
        public string facebook { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Twitter")]
        
        public string twitter { get; set; }

        [Display(Name = "Tipo")]
        public short tipo { get; set; }

        [Display(Name = "Excluído")]
        public bool excluido { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }
    }
}