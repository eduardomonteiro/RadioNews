using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(FaleConoscoMetaData))]
    public partial class FaleConosco
    {

    }


    public class FaleConoscoMetaData
    {

        [Display(Name = "Assunto")]
        public string assunto { get; set; }

        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Estado")]
        public string estado { get; set; }

        [Display(Name = "Cidade")]
        public string cidade { get; set; }

        [Display(Name = "Celular")]
        public string celular { get; set; }

        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        [Display(Name = "Mensagem")]
        public string mensagem { get; set; }

        [Display(Name = "Resposta")]
        public string resposta { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime dataCadastro { get; set; }

        [Display(Name = "Data de Resposta")]
        public System.DateTime dataResposta { get; set; }

        [Display(Name = "Excluído")]
        public bool excluido { get; set; }
    }
}