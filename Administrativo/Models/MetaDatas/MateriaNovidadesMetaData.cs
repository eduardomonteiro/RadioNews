using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{


    [MetadataType(typeof(MateriaNovidadesMetaData))]
    public partial class MateriaNovidades
    {

    }

    public class MateriaNovidadesMetaData
    {
        public int id { get; set; }

        [Display(Name="Id")]
        public int idMateria { get; set; }

        [Display(Name = "Status")]
        public string status { get; set; }

        [Display(Name = "Data do Programa")]
        [Required(ErrorMessage = "Informe a data dessa novidade")]
        public System.DateTime dataPrograma { get; set; }

        [Display(Name = "Novidade")]
        public string novidades { get; set; }

        [Display(Name = "Áudio")]
        public string audio { get; set; }

        [Display(Name = "Data de Cadstro")]
        public System.DateTime dataCadastro { get; set; }
        public bool excluido { get; set; }
    }
}