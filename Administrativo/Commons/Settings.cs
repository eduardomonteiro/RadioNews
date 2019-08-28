using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Administrativo.Commons
{
    public static class Settings
    {
        public static string UrlSite = ConfigurationManager.AppSettings["UrlSite"].ToString();
    }
}