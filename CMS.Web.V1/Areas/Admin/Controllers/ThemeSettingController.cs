using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class ThemeSettingController : AdminBaseController
    {
        // GET: Admin/ThemeSetting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetThemeSetting()
        {
            try
            {
                return JsonConvert.SerializeObject(ThemeSettingDAL.GetThemeSetting());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetThemeSetting", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveThemeSetting(ThemeSetting setting)
        {
            bool result = false;
            try
            {
                result = ThemeSettingDAL.UpdateThemeSetting(setting.Maintenance, setting.DefaultLanguage, setting.LanguageType, setting.FontCDN, setting.FontFamily, setting.Color1, setting.Color2, setting.Color3, setting.Color4);
                if (result)
                {
                    new LanguageDAL().SetLanguage(setting.DefaultLanguage, CurrentSetting.DefaultLanguage);
                }
                string fileName = "~/Files/color.less";

                if (System.IO.File.Exists(Server.MapPath(fileName)))
                {
                    System.IO.File.Delete(Server.MapPath(fileName));
                }
                using (FileStream fs = System.IO.File.Create(Server.MapPath(fileName)))
                {
                    // Add some text to file
                    Byte[] title = new UTF8Encoding(true).GetBytes("@primary-color:" + setting.Color1 + ";");
                    fs.Write(title, 0, title.Length);
                    byte[] author = new UTF8Encoding(true).GetBytes("@primarytwo-color:" + setting.Color2 + "; @primarythree-color:" + setting.Color3 + ";@primaryfour-color:" + setting.Color4 + ";");
                    fs.Write(author, 0, author.Length);
                }

            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveThemeSetting", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}