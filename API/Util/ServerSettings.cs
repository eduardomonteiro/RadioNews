using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Util
{
    public static class ServerSettings
    {
        public static string Url
        {
            get
            {                
                var url = HttpContext.Current.Request.Url;
                string dominio = url.Scheme + "://" + url.Authority;

                return dominio;
            }
        }
    }
}