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
    public class OurServiceController : AdminBaseController
    {
        // GET: Admin/OurService
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public string GetOurServiceList()
        {
            try
            {
                return JsonConvert.SerializeObject(OurServiceDAL.GetAllOurService());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetOurServiceList", ex, GetUserIpAddress());
                throw;
            }
        }
        [HttpPost]

        public JsonResult SaveOurService(OurService ourService)
        {
            MessageBox message;
            try
            {
                if (ourService.Id > 0)
                {
                    OurServiceDAL.UpdateOurService(ourService.Id, ourService.Icon, ourService.Status,ourService.DisplayOrder);
                    foreach (var item in ourService.Detail)
                    {
                        item.Title = item.Title == null ? string.Empty : item.Title;
                        item.Description = item.Description == null ? string.Empty : item.Description;
                        OurServiceDAL.UpdateOurServiceDetail(ourService.Id, item.LanguageId, item.Title, item.Description);
                    }
                    message = new MessageBox(MessageBoxType.Success, "Hizmet güncellendi.");
                }
                else
                {
                    int createdOurServiceId = OurServiceDAL.AddOurService(ourService.Icon, ourService.Status,ourService.DisplayOrder);
                    foreach (var item in ourService.Detail)
                    {
                        item.Title = item.Title == null ? string.Empty : item.Title;
                        item.Description = item.Description == null ? string.Empty : item.Description;

                        OurServiceDAL.AddOurServiceDetail(createdOurServiceId, item.LanguageId, item.Title, item.Description);
                    }
                    message = new MessageBox(MessageBoxType.Success, "Hizmet eklendi.");
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveOurService", ex, GetUserIpAddress());
                message = new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            }
            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteOurService(OurService ourService)
        {
            MessageBox message;
            try
            {
                bool result = OurServiceDAL.DeleteOurService(ourService.Id);
                message = result ? new MessageBox(MessageBoxType.Success, "Hizmet silindi.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveOurService", ex, GetUserIpAddress());
                message = new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            }
            return Json(message);
        }
    }
}