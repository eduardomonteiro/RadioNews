//------------------------------------------------------------------------------
// <auto-generated>
//    O código foi gerado a partir de um modelo.
//
//    Alterações manuais neste arquivo podem provocar comportamento inesperado no aplicativo.
//    Alterações manuais neste arquivo serão substituídas se o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Administrativo.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Transporte
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string CssClass { get; set; }
        public string Status { get; set; }
        public string Texto { get; set; }
        public bool Excluido { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<System.DateTime> Atualizacao { get; set; }
    }
}
