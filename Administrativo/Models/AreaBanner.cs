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
    
    public partial class AreaBanner
    {
        public AreaBanner()
        {
            this.Banners = new HashSet<Banners>();
        }
    
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string chave { get; set; }
        public string Tamanho { get; set; }
        public System.DateTime DataCadastro { get; set; }
        public Nullable<bool> Ativo { get; set; }
        public string Tamanho2 { get; set; }
    
        public virtual ICollection<Banners> Banners { get; set; }
    }
}
