using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Site.Enums
{
    public enum RedeSocialTipo
    {
        [Display(Name = "Facebook")]
        Facebook = 1,
        [Display(Name = "Twitter")]
        Twitter = 2,
        [Display(Name = "Instagram")]
        Instagram = 3,
        [Display(Name = "Youtube")]
        Youtube = 4
    }

    public enum DiaSemana
    {
        [Display(Name = "Domingo")]
        Domingo = 0,
        [Display(Name = "Segunda")]
        Segunda = 1,
        [Display(Name = "Terça")]
        Terca = 2,
        [Display(Name = "Quarta")]
        Quarta = 3,
        [Display(Name = "Quinta")]
        Quinta = 4,
        [Display(Name = "Sexta")]
        Sexta = 5,
        [Display(Name = "Sábado")]
        Sabado = 6,
        
    }


    public enum BannerOperation
    {
        [Display(Name = "Visualização")]
        Visualizacao = 0,
        [Display(Name = "Clique")]
        Clique = 1
    }

    public enum Regioes
    {
        [Display(Name = "Central")]
        Central = 0,
        [Display(Name = "Leste")]
        Leste = 1,
        [Display(Name = "Oeste")]
        Oeste = 2,
        [Display(Name = "Norte")]
        Norte = 3,
        [Display(Name = "Sul")]
        Sul = 4
    }

    public enum TipoStream
    {
        [Display(Name = "MP3")]
        MP3 = 0,
        [Display(Name = "AAC")]
        AAC = 1,
        [Display(Name = "RTSP")]
        RTSP = 2,
        [Display(Name = "M3U3")]
        M3U3 = 3
    }

}