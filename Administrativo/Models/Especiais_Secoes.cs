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
    
    public partial class Especiais_Secoes
    {
        public Especiais_Secoes()
        {
            this.Secoes_Locais = new HashSet<Secoes_Locais>();
        }
    
        public int Id { get; set; }
        public int EditoriaId { get; set; }
        public string Titulo { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public System.DateTime DataCadastro { get; set; }
    
        public virtual Editoriais Editoriais { get; set; }
        public virtual ICollection<Secoes_Locais> Secoes_Locais { get; set; }
    }
}
