using CMS.Web.V1.Models.Entity;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class LanguageDAL : DataAccess
    {

        public static List<Language> GetAllLanguage()
        {
            List<Language> languageList = new List<Language>();
            DataTable dt = DAL.GetListLanguage();
            foreach (DataRow row in dt.Rows)
            {
                Language language = new Language();
                {
                    language.Id = row.Field<int>("Id");
                    language.LangName = row.Field<string>("Name");
                    language.LangCode = row.Field<string>("LangCode");
                    language.FlagCode = row.Field<string>("FlagCode");
                    language.Status = row.Field<bool>("Status");
                    language.Deleted = row.Field<bool>("Deleted");
                }
                languageList.Add(language);
            }
            return languageList;
        }
        public static bool AddLanguage(string name, string code, string flag, bool status)
        {
            return DAL.AddLanguage(name, code, flag, status);
        }
        public static bool UpdateLanguage(int id, string name, string code, string flag, bool status)
        {
            return DAL.EditLanguage(id, name, code, flag, status);
        }
        public static bool DeleteLanguage(int id)
        {
            return DAL.DeleteLanguage(id);
        }

        #region system Language Change
       
        public void SetLanguage(string langCode,string defaultLanguageCode)
        {
            List<Language> languageList = GetAllLanguage();
            if (languageList.FirstOrDefault(w => w.LangCode.ToLower().Equals(langCode.ToLower())) == null)
            {
                langCode = languageList.FirstOrDefault(w => w.LangCode == defaultLanguageCode).LangCode;
            }
            else
            {
                var cultureInfo = new CultureInfo(langCode);
                Thread.CurrentThread.CurrentUICulture = cultureInfo;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(cultureInfo.Name);

                //HttpCookie langCookie = new HttpCookie("culture", lang);
                //langCookie.Expires = DateTime.Now.AddYears(1);
                //HttpContext.Current.Response.Cookies.Add(langCookie);
                ///HttpCookie langCookie = Request.Cookies["culture"];

                HttpContext.Current.Session["CurrentLanguage"] = LanguageDAL.GetAllLanguage().FirstOrDefault(f => f.LangCode == langCode);
                HttpContext.Current.Session["CurrentLanguageCode"] = LanguageDAL.GetAllLanguage().FirstOrDefault(f => f.LangCode == langCode).LangCode;
                HttpContext.Current.Session["LocaleStringResource"] = null;
            }

        }
        #endregion
    }
    public partial class DataAccessLayer
    {
        public DataTable GetListLanguage()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Language_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool AddLanguage(string pName, string pCode, string pFlagCode, bool pStatus)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Language_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pName, pCode, pFlagCode, pStatus });
        }
        public bool EditLanguage(int pId, string pName, string pCode, string pFlagCode, bool pStatus)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Language_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pName, pCode, pFlagCode, pStatus });
        }
        public bool DeleteLanguage(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Language_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }
    }
}

