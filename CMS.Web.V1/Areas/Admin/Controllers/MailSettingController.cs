using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class MailSettingController : AdminBaseController
    {
        // GET: Admin/MailSetting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetMailSettting()
        {
            try
            {
                return JsonConvert.SerializeObject(MailSettingDAL.GetMailSetting());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetMailSettting", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveMailSetting(Mail mail)
        {
            bool result = false;
            try
            {
                result = MailSettingDAL.UpdateMailSetting(mail.Host, mail.Port, mail.SSL, mail.From, mail.Password, mail.To);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveMailSetting", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult MailTest(Mail mail)
        {
            bool result = false;
            try
            {
                mail.SubJect = "CMS - Test Maili.";
                mail.Body = "Bu eposta <b>mail server ayarlarını</b> test etmek amacı ile gönderilmiştir.";
                //mail.Attachments = new Attachment(@"C:\Users\he\Desktop\test.pdf"); ;
                result = mail.MailSender();
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "MailTest", ex, GetUserIpAddress());
                result = false;
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "Mail Testi Başarılı.") : new MessageBox(MessageBoxType.Error, "Mail Testi Başarısız!");
            return Json(message);
        }
    }
}