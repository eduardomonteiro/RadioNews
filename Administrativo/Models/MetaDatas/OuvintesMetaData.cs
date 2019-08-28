using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(OuvintesMetaData))]
    public partial class Ouvintes
    {

    }
    public class OuvintesMetaData
    {
        [Display(Name = "Nome")]
        public string nome { get; set; }

        [Display(Name = "Email")]
        public string email { get; set; }

        [Display(Name = "Endereço")]
        public string endereco { get; set; }

        [Display(Name = "Região")]
        public int regiaoId { get; set; }
        [Display(Name = "Cidade")]
        public int cidade_id { get; set; }

        [Display(Name = "Bairro")]
        public string bairro { get; set; }

        [Display(Name = "Assunto")]
        public string assunto { get; set; }

        [Display(Name = "Horário")]
        public string horario { get; set; }

        [Display(Name = "Data")]
        public System.DateTime data { get; set; }

        [Display(Name = "Telefone")]
        public string telefone { get; set; }

        [Display(Name = "Comentário")]
        public string comentario { get; set; }

        [Display(Name = "Data de Cadastro")]
        public System.DateTime DataCadastro { get; set; }

        [Display(Name = "Excluido")]
        public bool excluido { get; set; }
        
    }
}