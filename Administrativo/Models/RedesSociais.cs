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
    
    public partial class RedesSociais
    {
        public int Id { get; set; }
        public int TipoRede { get; set; }
        public string Imagem { get; set; }
        public string Texto { get; set; }
        public string Link { get; set; }
        public Nullable<System.DateTime> Data { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public string Video { get; set; }
        public string PostId { get; set; }
    }
}
