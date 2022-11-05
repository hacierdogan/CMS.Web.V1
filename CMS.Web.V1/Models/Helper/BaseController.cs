using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Models.Helper
{
    public class BaseController : Controller
    {
        // GET: Base
        protected User AuthorityUser
        {
            get { return (User)Session["AuthorityUser"]; }
            set { Session["AuthorityUser"] = value; }
        }
        protected ThemeSetting CurrentSetting
        {
            get { return (ThemeSetting)Session["CurrentSetting"]; }
            set { Session["CurrentSetting"] = value; }
        }
        protected Language CurrentLanguage
        {
            get { return (Language)Session["CurrentLanguage"]; }
            set { Session["CurrentLanguage"] = value; }
        }
        protected Dictionary<string, string> LocaleStringResource
        {
            get { return (Dictionary<string, string>)Session["LocaleStringResource"]; }
            set { Session["LocaleStringResource"] = value; }
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (CurrentSetting == null)
                CurrentSetting = ThemeSettingDAL.GetThemeSetting();

            if (CurrentLanguage == null)
                CurrentLanguage = LanguageDAL.GetAllLanguage().FirstOrDefault(w=>w.LangCode==CurrentSetting.DefaultLanguage);

            if (LocaleStringResource == null)
                LanguageResourceDAL.GetResourceByLanguage(CurrentLanguage.LangCode);

            new LanguageDAL().SetLanguage(CurrentLanguage.LangCode,CurrentSetting.DefaultLanguage);
        }

        public string GetUserIpAddress()
        {
            string ip = HttpContext.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Request.ServerVariables["REMOTE_ADDR"];
            return ip;
        }
        public string GetControllerName()
        {
            return this.ControllerContext.RouteData.Values["controller"] + "/" + this.ControllerContext.RouteData.Values["action"];
        }

        public string GetTime(DateTime date)
        {
            string remainingTime = "";
            if (date > DateTime.Now)
            {
                TimeSpan difference = date - DateTime.Now;
                int year = difference.Days / 365;
                int day = difference.Days;
                int hour = difference.Hours;
                int min = difference.Minutes;
                int sec = difference.Seconds;
                int week = Convert.ToInt32(day / 7);
                int mount = Convert.ToInt32(day / 30);

                if (year >= 1) { remainingTime = year + " yıl sonra"; }
                else if (mount <= 12 && mount > 0) { remainingTime = mount + " Ay sonra"; }
                else if (week < 4 && week >= 1) { remainingTime = week + " hafta sonra"; }
                else if (day <= 6 && day > 0) { remainingTime = day + " gün sonra"; }
                else if (hour > 0) { remainingTime = hour + "saat sonra"; }
                else if (min > 0) { remainingTime = min + "dk sonra"; }
                else if (sec < 60 && sec >= 0) { remainingTime = "Şimdi"; }
            }
            else
            {
                TimeSpan difference = DateTime.Now - date;
                int day = difference.Days;
                int hour = difference.Hours;
                int min = difference.Minutes;
                int sec = difference.Seconds;
                int week = Convert.ToInt32(day / 7);
                int mount = Convert.ToInt32(day / 30);


                if (mount >= 3) { remainingTime = date.ToString(" dd MMMM yyyy  HH:MM"); }
                else if (mount <= 2 && mount > 0) { remainingTime = mount + " Ay önce"; }
                else if (week < 4 && week >= 1) { remainingTime = week + " hafta önce"; }
                else if (day <= 6 && day > 0) { remainingTime = day + " gün önce"; }
                else if (hour > 0) { remainingTime = hour + "saat önce"; }
                else if (min > 0) { remainingTime = min + "dk önce"; }
                else if (sec < 60 && sec > 30) { remainingTime = "Azönce "; }
                else if (sec < 30 && sec >= 0) { remainingTime = "Şimdi"; }
            }

            return remainingTime;
        }
    }
    public static class MyExtensions
    {
        public static string toLanguage(this string val)
        {
            string str = val;
            try
            {
                repeat:
                Dictionary<string, string> LanguageFile = HttpContext.Current.Session["LocaleStringResource"] != null ? (Dictionary<string, string>)HttpContext.Current.Session["LocaleStringResource"] : null;

                if (LanguageFile != null) str = LanguageFile[val];
                else
                {
                    Language lang = HttpContext.Current.Session["CurrentLanguage"] as Language;
                    HttpContext.Current.Session["LocaleStringResource"] = LanguageResourceDAL.GetResourceByLanguage(lang.LangCode);
                    goto repeat;
                }
            }
            catch (Exception)
            {
                str = val;
            }
            return str;
        }
    }

   
}