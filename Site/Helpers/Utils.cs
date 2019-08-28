using Site.Services;
using Site.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Site.Helpers
{
    public static class Utils
    {
        private static string primeKey = "utils:";
        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveAccent().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string StripTags(this string source)
        {
            return Regex.Replace(source, "<.*?>", string.Empty);
        }

        public static string RemoveAccent(this string s)
        {
            var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(s);
            return Encoding.ASCII.GetString(bytes);
        }

        public static Dictionary<string, string> clima = new Dictionary<string, string>()
        {
            {"ec", "Encoberto com Chuvas Isoladas"},  //Encoberto com Chuvas Isoladas
            {"ci", "Chuvas Isoladas"},//Chuvas Isoladas
            {"c", "Chuva"},//Chuva
            {"in", "Instável"},//Instável
            {"pp", "Possibilidade de Pancadas de Chuva"},//Poss. de Pancadas de Chuva
            {"cm", "Chuva pela Manhã"},//Chuva pela Manhã
            {"cn", "Chuva a Noite"},//Chuva a Noite
            {"pt", "Pancadas de Chuva a Tarde"},//Pancadas de Chuva a Tarde
            {"pm", "Pancadas de Chuva pela Manhã"},  //Pancadas de Chuva pela Manhã
            {"np", "Nublado e Pancadas de Chuva"},  //Nublado e Pancadas de Chuva
            {"pc", "Pancadas de Chuva"},  //Pancadas de Chuva
            {"pn", "Parcialmente Nublado"},  //Parcialmente Nublado
            {"cv", "Chuvisco"},  //Chuvisco
            {"ch", "Chuvoso"},  //Chuvoso
            {"t", "Tempestade"},   //Tempestade
            {"ps", "Predomínio de Sol"},  //Predomínio de Sol
            {"e", "Encoberto"},   //Encoberto
            {"n", "Nublado"},   //Nublado
            {"cl", "Céu Claro"},  //Céu Claro
            {"nv", "Nevoeiro"},  //Nevoeiro
            {"g", "Geada"},   //Geada
            {"ne", "Neve"},  //Neve
            {"nd", "Não Definido"},  //Não Definido
            {"pnt", "Pancadas de Chuva a Noite"}, //Pancadas de Chuva a Noite
            {"psc", "Possibilidade de Chuva"}, //Possibilidade de Chuva
            {"pcm", "Possibilidade de Chuva pela Manhã"}, //Possibilidade de Chuva pela Manhã
            {"pct", "Possibilidade de Chuva a Tarde"}, //Possibilidade de Chuva a Tarde
            {"pcn", "Possibilidade de Chuva a Noite"}, //Possibilidade de Chuva a Noite
            {"npt", "Nublado com Pancadas a Tarde"}, //Nublado com Pancadas a Tarde
            {"npn", "Nublado com Pancadas a Noite"}, //Nublado com Pancadas a Noite
            {"ncn", "Nublado com Possibilidade de Chuva a Noite"}, //Nublado com Poss. de Chuva a Noite
            {"nct", "Nublado com Possibilidade de Chuva a Tarde"}, //ublado com Poss. de Chuva a Tarde
            {"ncm", "Nublado com Possibilidade de Chuva pela Manhã"}, //Nubl. c/ Poss. de Chuva pela Manhã
            {"npm", "Nublado com Pancadas pela Manhã"}, //Nublado com Pancadas pela Manhã
            {"npp", "Nublado com Possibilidade de Chuva"}, //Nublado com Possibilidade de Chuva
            {"vn", "Variação de Nebulosidade"},  //Variação de Nebulosidade
            {"ct", "Chuva a Tarde"},  //Chuva a Tarde
            {"ppn", "Possibilidade de Pancadas de Chuva a Noite"}, //Poss. de Panc. de Chuva a Noite
            {"ppt", "Possibilidade de Pancadas de Chuva a Tarde"} //Poss. de Panc. de Chuva a Tarde
        };

        public static string RemoveDiacritics(this string input)
        {
            string stFormD = input.Normalize(NormalizationForm.FormD);
            int len = stFormD.Length;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < len; i++)
            {
                System.Globalization.UnicodeCategory uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(stFormD[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(stFormD[i]);
                }
            }
            return (sb.ToString().Normalize(NormalizationForm.FormC));
        }

        public static string TruncateText(string text, int length, bool symbol = false)
        {
            string result = text;

            if (text.Length <= length)
            {
                return result;
            }
            else
            {
                if (symbol)
                {
                    result = text.Substring(0, length - 3) + "...";
                }
                else
                {
                    result = text.Substring(0, length);
                }

                return result;
            }


        }

        public static int diaSemanaChave(string dia)
        {
            switch (dia)
            {
                case "domingo":
                    return 0;
                case "segunda":
                    return 1;
                case "terca":
                    return 2;
                case "quarta":
                    return 3;
                case "quinta":
                    return 4;
                case "sexta":
                    return 5;
                case "sabado":
                    return 6;
                default:
                    return 1;
            }

        }
        private static FotoViewModel getFotoDB(int idNoticia = 0)
        {
            if (idNoticia == 0) return null;
            using (Models.ModeloDados db = new Models.ModeloDados())
            {
                var foto = db.Noticias.Where(x => !x.excluido && x.liberado && 
                    x.dataAtualizacao < DateTime.Now && x.id == idNoticia)
                    .Select(n => new FotoViewModel {
                        fotoNome = n.foto,
                        IdNoticia = n.id
                    })
                    .FirstOrDefault();
                return foto;
            }
        }
        public static string fullUrlImage(string resolucao, int id = 0, int width = 0, int height = 0)
        {
            if (id > 0)
            {
                string key = primeKey + "fullUrlImage:" + id + ":TNoticias";

                Func<object, FotoViewModel> funcao = t => getFotoDB(id);
                var foto = RedisService.GetOrSetToRedis(key, funcao, 300);

                if (foto != null && foto.fotoNome != null)
                {
                    if (foto.fotoNome.ToLower().Contains("conteudo"))
                    {
                        string parametros = string.Empty;
                        string link = string.Empty;

                        if (width == 0 && height == 0)
                        {
                            link = "/admin/" + foto.fotoNome;
                            return link;
                        }

                        if (width > 0)
                        {
                            parametros = "width=" + width;
                            return "/admin/" + foto.fotoNome + "?width=" + width + "&height=" + height;
                        }

                        if (height > 0)
                        {
                            parametros = parametros + "&heigth=" + height;
                        }


                    }
                    else
                    {
                        string parametros = string.Empty;
                        string link = string.Empty;

                        if (width == 0 && height == 0)
                        {
                            link = "/admin/Conteudo/noticias/" + foto.IdNoticia + "/" + resolucao + "/" + foto.fotoNome;
                            return link;
                        }                           

                        if (width > 0)
                        {
                            parametros = "width=" + width;
                        }

                        if (height > 0)
                        {
                            parametros = parametros + "&heigth=" + height;
                        }

                        link = "/admin/Conteudo/noticias/" + foto.IdNoticia + "/" + resolucao + "/" + foto.fotoNome + "?" + parametros;

                        return link;

                    }

                }



            }

            return "";

        }



        public static string WeekDayName(this int weekDay)
        {
            var diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName((DayOfWeek)weekDay);
            return diaExtenso;
        }

        public static string WeekDayName(this DayOfWeek weekDay)
        {
            var diaExtenso = new CultureInfo("pt-BR").DateTimeFormat.GetDayName(weekDay);
            return diaExtenso;
        }



        public static string Friendly(this string s, int? length = null)
        {
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

    }
}