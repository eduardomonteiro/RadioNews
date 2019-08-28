using Site.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.ViewModels
{
    public class RedesSociaisViewModel
    {

    }

    public class RedesSociaisHomeViewModel
    {
        public List<RedesSociais> Twitter { get; set; }
        public List<RedesSociais> Facebook { get; set; }
        public RedesSociais FotoDestaque{ get; set; }
    }

}
