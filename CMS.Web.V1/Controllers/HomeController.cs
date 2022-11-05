using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            ThemeSetting setting = ThemeSettingDAL.GetThemeSetting();
            if (setting.Maintenance)
            {
                return RedirectToAction("Index", "Maintenance");
            }
            return View();
        }
        public ActionResult ChangeLanguage(string id)
        {
            new LanguageDAL().SetLanguage(id, CurrentSetting.DefaultLanguage);
            return Redirect(Request.Headers["Referer"]);
        }

        [HttpPost]
        public string GetLanguageList()
        {
            return JsonConvert.SerializeObject(LanguageDAL.GetAllLanguage().Where(w => w.Status == true));
        }

        [HttpPost]
        public string GetSelectedLanguage()
        {
            return JsonConvert.SerializeObject(LanguageDAL.GetAllLanguage().FirstOrDefault(f => f.LangCode == CurrentLanguage.LangCode));
        }

        [HttpPost]
        public string GetSliderList()
        {
            List<Slider> sliderList = SliderDAL.GetAllSliderByLanguageId(CurrentLanguage.Id);
            return JsonConvert.SerializeObject(sliderList);
        }

        [HttpPost]
        public string GetTabList()
        {
            List<Content> tabList = ContentDAL.GetAllContent((int)ContentType.Tab).Where(w => w.Status == true).ToList();
            foreach (var tab in tabList)
            {
                tab.Detail = tab.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList();
            }
            return JsonConvert.SerializeObject(tabList.OrderBy(o => o.DisplayOrder));
        }

        [HttpPost]
        public string GetSocialMediaList()
        {
            List<SocialMedia> socialList = SocialMediaDAL.GetSocialMedia().ToList();
            return JsonConvert.SerializeObject(socialList.FirstOrDefault());
        }

        [HttpPost]
        public string GetCompanyInfo()
        {
            List<Company> company = CompanyDAL.GetListCompanyInformation();
            return JsonConvert.SerializeObject(company.FirstOrDefault());
        }

        [HttpPost]
        public JsonResult GetPopup()
        {
            bool popupStatus = false;
            try
            {
                Popup popup = PopupDAL.GetAllPopup().FirstOrDefault();
                popup.Detail = popup.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList();

                if (popup.StartDate < DateTime.Now && popup.EndDate > DateTime.Now && popup.Status)
                {
                    popupStatus = true;
                }
                else
                {
                    popupStatus = false;
                }
                return Json(
                   new
                   {
                       Popup = popup,
                       PopupStatus = popupStatus,
                       JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                       MaxJsonLength = Int32.MaxValue,
                       RecursionLimit = Int32.MaxValue
                   }
               );
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.User, "GetPopupList", ex, GetUserIpAddress());
                throw;
            }
        }

        public ActionResult Error()
        {
            Response.StatusCode = 404;
            Response.TrySkipIisCustomErrors = true;
            return View();
        }
    }
}