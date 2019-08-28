using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Site.Helpers
{
    public static class ChavesBannerSeuBolso
    {
        public const string CHAVE_BANNER_1 = "seu-bolso-1";
        public const string CHAVE_BANNER_2 = "seu-bolso-2";
        public const string CHAVE_BANNER_3 = "seu-bolso-3";
        public const string CHAVE_BANNER_4 = "seu-bolso-4";
        public const string CHAVE_BANNER_5 = "seu-bolso-5";

        public static List<string> RetonaListaChaves()
        {
            return new List<string>()
            {
                CHAVE_BANNER_1, CHAVE_BANNER_2, CHAVE_BANNER_3, CHAVE_BANNER_4, CHAVE_BANNER_5
            };
        }
    }
}