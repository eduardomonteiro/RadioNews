using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class NavigationBarViewModel
    {
        public List<EditoriasLayoutViewModel> especiais { get; set; }
        public List<EditoriasLayoutViewModel> canais { get; set; }
    }
}