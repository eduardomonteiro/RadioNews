using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.ViewModel
{
    public class EditoriasViewModel
    {
        public List<EditoriaisViewModel> esportes { get; set; }
        public List<EditoriaisViewModel> editoriais { get; set; }
        public List<TimesViewModel> times { get; set; }
    }
}