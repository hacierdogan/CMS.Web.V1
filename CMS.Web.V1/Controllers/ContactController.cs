using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Controllers
{
    public class ContactController : BaseController
    {
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendMessage(Message message)
        {
            MessageBox messageAlert;
            bool result = false;
            try
            {
                if (ModelState.IsValid)
                {
                    result = MessageDAL.AddMesage(message.Title, message.Mail, message.Subject, message.MessageStr);

                    if (result)
                    {
                        Mail mail = MailSettingDAL.GetMailSetting();
                        mail.SubJect = "CMS - İletişim Sayfasından Mesaj Var.";
                        mail.Body = "<b>İsim:</b> " + message.Title+ "<br> <b>Mail:</b> " + message.Mail+ " <br><b>Konu:</b> " + message.Subject+ "<br><b>Mesaj:</b> " + message.MessageStr;
                        //mail.Attachments = new Attachment(@"C:\Users\he\Desktop\test.pdf");
                        //mail.MailSender();
                    }
                    messageAlert = result ? new MessageBox(MessageBoxType.Success, ("Contact.Form.Success.Message").toLanguage()) : new MessageBox(MessageBoxType.Error, ("Contact.Form.Error.Message").toLanguage());
                }
                else
                {
                    messageAlert =  new MessageBox(MessageBoxType.Error, ("Contact.Form.Empty.Message").toLanguage());
                }
               
                return Json(messageAlert);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.User, "SendMessage", ex, GetUserIpAddress());
                throw;
            }
        }
    }
}