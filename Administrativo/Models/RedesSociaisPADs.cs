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
    
    public partial class RedesSociaisPADs
    {
        public int Id { get; set; }
        public string Chamada { get; set; }
        public string Hashtag { get; set; }
        public int TipoRede { get; set; }
        public string Foto { get; set; }
        public bool Excluido { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<int> ApoioId { get; set; }
    
        public virtual ApoioPADs ApoioPADs { get; set; }
    }
}
