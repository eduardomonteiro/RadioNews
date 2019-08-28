using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelpers
    {
        public static MvcHtmlString DisplayWeekDayName(this HtmlHelper html, int weekDay)
        {
            var diaExtenso = Administrativo.Commons.Utils.WeekDayName(weekDay);
            return new MvcHtmlString(diaExtenso);
        }

        
    }
}