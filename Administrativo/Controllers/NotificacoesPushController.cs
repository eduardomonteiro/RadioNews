using Administrativo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace Administrativo.Controllers
{
    public class NotificacoesPushController : BaseController
    {
        //
        // GET: /NotificacoesPush/

        public void GerarNotificacoes(Noticias noticia, string message, string title)
        {
            List<GruposPush> grupos = new List<GruposPush>();
            foreach (var tag in noticia.Tags)
            {
                grupos.AddRange(db.GruposPush.Where(g => g.Tags.Contains(tag) && !grupos.Contains(g)).ToList());
            }
            if (grupos.Count > 0)
            {
                foreach (var grupo in grupos)
                {
                    var push = new NotificacoesPush()
                    {
                        DataCadastro = DateTime.Now,
                        Title = title,
                        Message = message,
                        Noticias = noticia,
                        GruposPush = grupo,
                        Enviado = false
                    };
                    db.NotificacoesPush.Add(push);
                }
                db.SaveChanges();
            }
        }
        private void EnviarNotificacao (NotificacoesPush push)
        {

            var webAddr = WebConfigurationManager.AppSettings["MobRadioAPIURL"].ToString(); 
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Method = "POST";
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                object objectData = new {
                    app_key = WebConfigurationManager.AppSettings["MobRadioAPIKey"].ToString(),
                    title = push.Title,
                    message = push.Message,
                    group_id = push.GruposPush.Id
                }; 
                var json = Json(objectData);
                streamWriter.Write(json);
                //streamWriter.Flush();
            }
            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                push.RetornoAPI = streamReader.ReadToEnd();
                push.Enviado = true;
                db.Entry(push).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void VerificaNotificacao()
        {
            var push = db.NotificacoesPush.Where(x => !x.Enviado).OrderByDescending(x => x.Id).FirstOrDefault();
            if (push != null)
            {
                EnviarNotificacao(push);
            }
        }

    }
}
