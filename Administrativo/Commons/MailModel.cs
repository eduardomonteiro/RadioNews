using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Administrativo.Commons
{
    public class MailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        private SmtpClient Smtp { get; set; }

        public MailModel()
        {
            Smtp = new SmtpClient
            {
                Host = "webmail.CompanyName.com.br",
                Port = 587,
                Credentials = new NetworkCredential("noreply@CompanyName.com.br", "sIkp=KU4-nV8")
            };
        }

        public bool SendEmail()
        {
            try
            {
                var mail = new MailMessage();
                mail.To.Add(this.To);
                //CompanyName@CompanyName.com.br
                mail.From = new MailAddress("noreply@CompanyName.com.br", !string.IsNullOrEmpty(this.From) ? this.From : "Rádio CompanyName - A Rádio de Minas");
                mail.Subject = this.Subject;
                var body = this.Body;
                mail.Body = body;
                mail.IsBodyHtml = true;
                Smtp.Send(mail);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}