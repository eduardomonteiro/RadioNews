using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Administrativo.Enums
{
    public enum TipoDestaque
    {
        [Display(Name="Nenhum destaque")]
        NenhumDestaque = 0,
        [Display(Name = "Destaque Urgente 360x240")]
        Urgente360 = 1,
        [Display(Name = "Destaque Urgente 1130x240")]
        Urgente1130 = 2,
        [Display(Name = "1º Destaque na Home")]
        Normal1 = 3,
        [Display(Name = "2º Destaque na Home")]
        Normal2 = 4,
        [Display(Name = "3º Destaque na Home")]
        Normal3 = 5,
        [Display(Name = "1º Destaque na Interna")]
        Interna1 = 6,
        [Display(Name = "2º Destaque na Interna")]
        Interna2 = 7,
        [Display(Name = "3º Destaque na Interna")]
        Interna3 = 8,
        [Display(Name= "Destaque na Editoria")]
        Editoria = 9
    }
}