using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class EditoriasLayoutViewModel
    {
        public string chave { get; set; }
        public string nome { get; set; }
        public string Action { get; set; }
        public bool Canal { get; set; }
        public bool Especial { get; set; }
    }
}