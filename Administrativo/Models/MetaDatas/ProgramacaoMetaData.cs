using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.Models
{
    [MetadataType(typeof(ProgramacaoMetaData))]
    public partial class Programacao
    {

    }

    public class ProgramacaoMetaData
    {
        [Display(Name = "Id")]
        public int id { get; set; }

        [Display(Name = "Editoria")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public int editoriaId { get; set; }

        [StringLength(100, ErrorMessage = "{0} não pode conter mais do que {1} caracteres.")]
        [Display(Name = "Nome do Programa")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        public string nome { get; set; }

        [Display(Name = "Logo")]
        public string logo { get; set; }

        [Display(Name = "Periodicidade")]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [Range(0,3,ErrorMessage ="Escolha uma periodiocidade")]
        public int periodo { get; set; }

        [Display(Name = "Capa")]
        public string imagem { get; set; }

        [AllowHtml]
        [Required(ErrorMessage = "{0} é de preenchimento obrigatório.")]
        [Display(Name = "Descrição")]
        public string texto { get; set; }

        [Display(Name = "Chamada")]
        public string chamada { get; set; }

        [Display(Name = "Dia da Semana")]
        public int diaSemana { get; set; }

        [Display(Name = "Horário")]
        public System.TimeSpan horario { get; set; }

        [Display(Name = "Data de Cadastro")]
        public Nullable<System.DateTime> dataCadastro { get; set; }

        public bool excluido { get; set; }
    }
}