using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Administrativo.Services
{
    public class UpdateCache
    {
        public static void Sync() {
            Uri targetUri = new Uri("http://www.CompanyName.com.br/Cache/Sync");
            System.Net.HttpWebRequest request = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(targetUri);
            var response = request.GetResponse() as HttpWebResponse;
        }
    }
}