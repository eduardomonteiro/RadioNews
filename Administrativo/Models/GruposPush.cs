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
    
    public partial class GruposPush
    {
        public GruposPush()
        {
            this.NotificacoesPush = new HashSet<NotificacoesPush>();
            this.Tags = new HashSet<Tags>();
        }
    
        public int Id { get; set; }
        public bool Excluido { get; set; }
        public bool Liberado { get; set; }
        public string Title { get; set; }
        public System.DateTime DataCadastro { get; set; }
    
        public virtual ICollection<NotificacoesPush> NotificacoesPush { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
    }
}
