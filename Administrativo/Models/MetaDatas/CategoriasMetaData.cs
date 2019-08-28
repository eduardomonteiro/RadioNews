using CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Models
{
    [MetadataType(typeof(CategoriasMetaData))]
    public partial class Categorias
    {

    }

    public class CategoriasMetaData
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Editorial")]
        public Nullable<int> EditoriaId { get; set; }

        [ScaffoldInList(false)]
        [Display(Name="Destaque")]
        public bool destaque { get; set; }

        [Required]
        public string Titulo { get; set; }

        [ScaffoldInList(false)]
        public bool Excluido { get; set; }

        [ScaffoldInList(false)]
        [Required]
        public int Ordem { get; set; }


        public System.DateTime DataCadastro { get; set; }
    }
}