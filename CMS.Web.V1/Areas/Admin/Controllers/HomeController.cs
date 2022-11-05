using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        // GET: Admin/Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetDashboard()
        {
            try
            {
                Dashboard dashboard = DashboardDAL.GetDashboardInformation();
                List<Mission> missionList = MissionDAL.GetMissionList();
                foreach (var item in missionList)
                {
                    item.TimeStr = GetTime(item.Date);
                }

                List<WebInfo> webInfoList = new List<WebInfo>() {
                    new WebInfo {
                        Title="Hosting",
                        DateStr=GlobalSettings.HostingDate.ToString(),
                        DayStr=GetTime(DateTime.ParseExact(GlobalSettings.HostingDate, "dd.MM.yyyy", new CultureInfo("tr-TR"), DateTimeStyles.None))+" sona erecek"
                    },
                    new WebInfo {
                        Title="Domain",
                        DateStr=GlobalSettings.DomainDate.ToString(),
                        DayStr=GetTime(DateTime.ParseExact(GlobalSettings.HostingDate, "dd.MM.yyyy", new CultureInfo("tr-TR"), DateTimeStyles.None))+" sona erecek"
                    }
                };
                return Json(
                    new
                    {
                        dashboard = dashboard,
                        webInfoList = webInfoList,
                        missionList = missionList,
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = Int32.MaxValue,
                        RecursionLimit = Int32.MaxValue
                    }
                );
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetDashboard", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateMission(int id, bool status, bool delete)
        {
            try
            {
                bool result = false;
                if (delete)
                {
                    result = MissionDAL.UpdateMission(id, !status, true);
                }
                else
                {
                    result = MissionDAL.UpdateMission(id, !status, false);
                }
                var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UpdateMission", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string GetAuthorityUser()
        {
            try
            {
                return JsonConvert.SerializeObject(AuthorityUser);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetAuthorityUser", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveMission(string description)
        {
            try
            {
                bool result = false;
                result = MissionDAL.AddMission(description);
                var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveMission", ex, GetUserIpAddress());
                throw;
            }
        }
    }

    internal class WebInfo
    {
        public string Title { get; set; }
        public string DateStr { get; set; }
        public string DayStr { get; set; }
    }
}