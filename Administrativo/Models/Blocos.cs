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
    
    public partial class Blocos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Chave { get; set; }
        public string Imagem { get; set; }
        public string Local { get; set; }
        public System.DateTime Data { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Excluido { get; set; }
        public System.DateTime DataCadastro { get; set; }
    }
}
