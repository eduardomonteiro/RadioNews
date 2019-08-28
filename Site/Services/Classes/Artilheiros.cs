using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Services.Classes
{
    public class Artilheiros
    {       
        public string Nome { get; set; }        
        public Clube Clube { get; set; }
        public int Gols { get; set; }
    }
}