using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(PromocoesMetaData))]
    public partial class Promocoes
    {

    }
    public partial class PromocoesMetaData
    {
        public int Id { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Titulo { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Descricao { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Regulamento")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Regulamento { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Email dos Ganhadores")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string EmailTexto { get; set; }

        [Display(Name = "Data de Início")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public System.DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public System.DateTime DataFim { get; set; }

        [Display(Name = "Resultado Automático?(Gerado pelo sistema)")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public bool ResultadoAutomatico { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Prêmio")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string Premio { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Quantidade Total")]
        public int Quantidade { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Imagem")]
        public string Imagem { get; set; }
        
        [ScaffoldInList(false)]
        [Display(Name = "Tipo de Promoção")]
        public int TipoPromocao { get; set; }

        [Display(Name = "Promoção Ativa?")]
        public bool Ativo { get; set; }

        [ScaffoldInList(false)]
        [Display(Name = "Excluido")]
        public bool Excluido { get; set; }

        public Nullable<System.DateTime> DataCadastro { get; set; }

    }
}