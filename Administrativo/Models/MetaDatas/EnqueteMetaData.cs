using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{

    [MetadataType(typeof(EnqueteMetaData))]
    public partial class Enquete
    {

    }

    public class EnqueteMetaData
    {

        [StringLength(300, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Pergunta")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string pergunta { get; set; }

        [Display(Name = "Ativo")]
        public bool ativa { get; set; }
        

        [Display(Name = "Data de Início da Votação")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public bool dataInicioVotacao { get; set; }

        [Display(Name = "Data de Fim da Votação (encerra as votações no site)")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public bool dataFimVotacao { get; set; }


        [Display(Name = "Data de Fim dos Resultados (até que data será exibido no site os resultados)")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public bool dataFimResultado { get; set; }

        [Display(Name = "Excluido")]
        public bool excluido { get; set; }

        [Display(Name = "Destaque")]
        public bool destaque { get; set; }

        [Display(Name = "DataCadastro")]
        public System.DateTime dataCadastro { get; set; }

        [Display(Name = "Respostas")]
        public virtual ICollection<Respostas> Respostas { get; set; }
    }
}