using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Administrativo.Enums
{
    public enum TipoAcesso
    {
        [Display(Name ="Inserção")]
        Insercao = 1,
        [Display(Name = "Edição")]
        Edicao = 2,
        [Display(Name = "Exclusão")]
        Exclusao = 3,
        [Display(Name = "Acesso ao sistema")]
        Acesso = 4,
        [Display(Name = "Saída do sistema")]
        Saida = 5
    }

}