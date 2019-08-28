using System.Text;
using System.Text.RegularExpressions;

namespace System.Web.Mvc.Html
{
    public static class StringExtensions
    {
        public static string Friendly(this string s, int? length = null)
        {
            /*
            string str = s.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            if (length != null)
                str = str.Substring(0, str.Length <= (int)length ? str.Length : (int)length).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;

            */
            int suffix = 1;

            if (s == null) return "";

            const int maxlen = 100;
            var len = s.Length;
            var prevdash = false;
            var sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = s[i];
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                    prevdash = false;
                }
                else if (c >= 'A' && c <= 'Z')
                {
                    // tricky way to convert to lowercase
                    sb.Append((char)(c | 32));
                    prevdash = false;
                }
                else if (c == ' ' || c == ',' || c == '.' || c == '/' ||
                    c == '\\' || c == '-' || c == '_' || c == '=')
                {
                    if (!prevdash && sb.Length > 0)
                    {
                        sb.Append('-');
                        prevdash = true;
                    }
                }
                else if (c >= 128)
                {
                    int prevlen = sb.Length;
                    sb.Append(RemapInternationalCharToAscii(c));
                    if (prevlen != sb.Length) prevdash = false;
                }
                if (i == maxlen) break;
            }

            if (prevdash)
                return (suffix == 1 ? sb.ToString().Substring(0, sb.Length - 1) : string.Format("{0}-{1}", sb.ToString().Substring(0, sb.Length - 1), suffix));

            return (suffix == 1 ? sb.ToString() : string.Format("{0}-{1}", sb, suffix));

        }

        private static string RemapInternationalCharToAscii(char c)
        {
            var s = c.ToString().ToLowerInvariant();
            if ("àåáâäãåą".Contains(s))
            {
                return "a";
            }
            if ("èéêëę".Contains(s))
            {
                return "e";
            }
            if ("ìíîïı".Contains(s))
            {
                return "i";
            }
            if ("òóôõöøőð".Contains(s))
            {
                return "o";
            }
            if ("ùúûüŭů".Contains(s))
            {
                return "u";
            }
            if ("çćčĉ".Contains(s))
            {
                return "c";
            }
            if ("żźž".Contains(s))
            {
                return "z";
            }
            if ("śşšŝ".Contains(s))
            {
                return "s";
            }
            if ("ñń".Contains(s))
            {
                return "n";
            }
            if ("ýÿ".Contains(s))
            {
                return "y";
            }
            if ("ğĝ".Contains(s))
            {
                return "g";
            }
            if (c == 'ř')
            {
                return "r";
            }
            if (c == 'ł')
            {
                return "l";
            }
            if (c == 'đ')
            {
                return "d";
            }
            if (c == 'ß')
            {
                return "ss";
            }
            if (c == 'Þ')
            {
                return "th";
            }
            if (c == 'ĥ')
            {
                return "h";
            }
            return c == 'ĵ' ? "j" : "";
        }

        public static string RemoveAccent(this string s)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(s);
            return Encoding.ASCII.GetString(bytes);
        }

        public static string Truncate(this string s, int length, bool symbol = false)
        {
            if (s.Length <= length)
            {
                return s;
            }
            else
            {
                if (!symbol)
                {
                    return s.Substring(0, length);
                }
                else
                {
                    return s.Substring(0, length - 4) + " ...";
                }

            }
        }

        public static string EnsureUrlStartsWithHttp(this string url)
        {
            if (!string.IsNullOrEmpty(url))
            {
                return url.ToLower().StartsWith("http") ? url : string.Format("http://{0}", url);
            }
            return "";
        }
    }
}
