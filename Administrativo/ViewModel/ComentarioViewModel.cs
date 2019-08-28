using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Administrativo.ViewModel
{
    public class ComentarioViewModel
    {
        public SelectList Colunistas { get; set; }
        public SelectList Editorias { get; set; }
    }
}