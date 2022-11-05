using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Mail
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool SSL { get; set; }
        public string From { get; set; }
        public string Password { get; set; }
        public string To { get; set; }
        public string SubJect { get; set; }
        public string Body { get; set; }
        public Attachment Attachments { get; set; }

        public bool MailSender()
        {
            bool result = false;
            using (var smtp = new SmtpClient
            {
                Host = Host,
                Port = Port,
                EnableSsl = SSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(From, Password),
                Timeout = int.MaxValue
            })
            {
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(From);
                    foreach (var item in To.Split(';'))
                    {
                        message.To.Add(item);
                    }
                    message.Subject = SubJect;
                    message.IsBodyHtml = true;
                    message.Body = Body;
                    if (Attachments != null)
                    {
                        message.Attachments.Add(Attachments);
                    }

                    smtp.Send(message);
                    result = true;
                }
            }
            return result;
        }
    }
}