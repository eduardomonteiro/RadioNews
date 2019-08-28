using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace System.Web.Mvc.Html
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum enumValue)
        {
            var value = enumValue.GetType().GetMember(enumValue.ToString())[0].GetCustomAttribute<DisplayAttribute>();

            return value != null ? value.Name : string.Empty;
        }

        #region EnumDropDownList

        public static MvcHtmlString EnumDropDownList(this HtmlHelper htmlHelper, string name, Type enumType,
             string selectedValue, string optionLabel, object htmlAttributtes)
        {
            var list = (from Enum item in Enum.GetValues(enumType)
                        select new SelectListItem
                        {
                            Value = ((int)(object)item).ToString(),
                            Text = item.GetDescription(),
                            Selected = selectedValue == item.ToString()
                        }).ToList();

            return htmlHelper.DropDownList(name, list, optionLabel, htmlAttributtes);
        }

        #endregion EnumDropDownList
    }
}