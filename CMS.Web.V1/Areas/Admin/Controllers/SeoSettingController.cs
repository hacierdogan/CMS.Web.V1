using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class SeoSettingController : AdminBaseController
    {
        // GET: Admin/SeoSetting
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetSeoSettting()
        {
            try
            {
                return JsonConvert.SerializeObject(SeoSettingDAL.GetSeoSetting());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetSeoSettting", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveSeoSetting(Seo seo)
        {
            bool result = false;
            try
            {
                result = SeoSettingDAL.UpdateSeoSetting(seo.Title,seo.Description,seo.Keywords,seo.Owner,seo.Author,seo.Year,seo.GoogleAnalytic);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveSeoSetting", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}