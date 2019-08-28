using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace API.Util
{
    public class StringUtils
    {
        public static string Strip(string text)
        {
            if (string.IsNullOrEmpty(text)) return text;
            string s = Regex.Replace(text, @"<[^>]*>", "");
            s = s.Replace("&nbsp;", " ");
            return s;
        }
    }
}