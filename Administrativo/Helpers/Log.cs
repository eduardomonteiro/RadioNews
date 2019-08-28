using Administrativo.Enums;
using Administrativo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Administrativo.Helpers
{
    public static class GerenciaLogs
    {
        public static void saveLog(ref RadioCompanyNameContext db, int userId, int areaId, TipoAcesso tipo, int? detalhes = null)
        {
            Logs objLog = new Logs();
            objLog.UserId = userId;
            objLog.AreaId = areaId;
            objLog.Acao = (int)tipo;
            objLog.Data = DateTime.Now;
            objLog.IP = HttpContext.Current.Request.UserHostAddress;

            if (detalhes != null)
            {
                objLog.Detalhes = Convert.ToInt32(detalhes);
            }

            db.Logs.Add(objLog);
            db.SaveChanges();

        }


        public static string GetDescriptionFromEnumValue(Enum value)
        {
            EnumMemberAttribute attribute = value.GetType()
                .GetField(value.ToString())
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .SingleOrDefault() as EnumMemberAttribute;
            return attribute == null ? value.ToString() : attribute.Value;
        }


    }
}

