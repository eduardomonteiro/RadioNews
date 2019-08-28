using System.Collections.Generic;
using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString BooleanFieldFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TProperty>> expression, object htmlAttributes)
        {
            return htmlHelper.BooleanFieldFor(expression, true, htmlAttributes);
        }

        public static MvcHtmlString BooleanFieldFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
       Expression<Func<TModel, TProperty>> expression, bool defaultValue, object htmlAttributes)
        {
            bool? value = htmlHelper.ViewData.Model == null ? (bool?)null : Convert.ToBoolean((expression.Compile()(htmlHelper.ViewData.Model)));

            var selectList = new List<SelectListItem> { new SelectListItem { Text = "Sim", Value = "true", Selected = (value != null ? value.Value : defaultValue) }, new SelectListItem { Text = "Não", Value = "false", Selected = (value != null ? !value.Value : !defaultValue) } };

            return htmlHelper.DropDownListFor(expression, selectList, htmlAttributes);
        }
    }
}
