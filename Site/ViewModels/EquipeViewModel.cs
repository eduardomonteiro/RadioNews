using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class EquipeViewModel
    {
        public List<Equipe> Equipes { get; set; }
        public Banners Banner { get; set;}
    }
}