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
    
    public partial class NoticiasRegioes
    {
        public NoticiasRegioes()
        {
            this.Noticias = new HashSet<Noticias>();
        }
    
        public int Id { get; set; }
        public string Titulo { get; set; }
        public bool Excluido { get; set; }
        public int Ordem { get; set; }
        public System.DateTime DataCadastro { get; set; }
    
        public virtual ICollection<Noticias> Noticias { get; set; }
    }
}
