using CMS.Web.V1.Models.Entity;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class LanguageResourceDAL : DataAccess
    {
        public static List<LanguageResource> GetAllLanguageResource()
        {
            List<LanguageResource> resourceList = new List<LanguageResource>();

            DataTable dt = DAL.GetListLanguageResource();
            foreach (DataRow row in dt.Rows)
            {
                LanguageResource languageResource = new LanguageResource();
                {
                    languageResource.ResourceId = row.Field<int>("Id");
                    languageResource.ResourceKey = row.Field<string>("ResourceKey");
                    languageResource.ResourceValue = row.Field<string>("ResourceValue") != "" ? row.Field<string>("ResourceValue") : row.Field<string>("ResourceKey");
                    languageResource.LangCode = row.Field<string>("LangCode");
                    languageResource.LangName = row.Field<string>("LangName");
                    languageResource.LangId = row.Field<int>("LangId");

                }
                resourceList.Add(languageResource);
            }
            return resourceList;
        }
        public static List<LanguageResource> GetLanguageAllResourceByKey(string key)
        {
            List<LanguageResource> resourceList = new List<LanguageResource>();

            DataTable dt = DAL.GetLanguageResourceByKey(key);
            foreach (DataRow row in dt.Rows)
            {
                LanguageResource languageResource = new LanguageResource();
                {
                    languageResource.ResourceId = row.Field<int>("Id");
                    languageResource.ResourceKey = row.Field<string>("ResourceKey");
                    languageResource.ResourceValue = row.Field<string>("ResourceValue") != "" ? row.Field<string>("ResourceValue") : row.Field<string>("ResourceKey");
                    languageResource.LangCode = row.Field<string>("LangCode");
                    languageResource.LangName = row.Field<string>("LangName");
                    languageResource.LangId = row.Field<int>("LangId");

                }
                resourceList.Add(languageResource);
            }
            return resourceList;
        }
        public static bool AddLanguageResource(string key, string value, int langId)
        {
            return DAL.AddLanguageResource(key, value, langId);
        }
        public static bool UpdateLanguagResourcee(int id, string key, string value)
        {
            return DAL.EditLanguageResource(id, key, value);
        }

        #region system Resource
        public static Dictionary<string, string> GetResourceByLanguage(string code)
        {
            Dictionary<string, string> list = new Dictionary<string, string>();

            var resorcelist = GetAllLanguageResource().Where(w => w.LangCode == code).ToList();
            foreach (var item in resorcelist)
            {
                list.Add(item.ResourceKey, item.ResourceValue);
            }
            return list;
        }
        #endregion
    }
    public partial class DataAccessLayer
    {
        public DataTable GetListLanguageResource()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_LanguageResource_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable GetLanguageResourceByKey(string pResourceKey)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_LanguageResource_GetListByKey", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pResourceKey });
        }
        public bool AddLanguageResource(string pResourceKey, string pResourceValue, int pLangId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_LanguageResource_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pResourceKey, pResourceValue, pLangId });
        }
        public bool EditLanguageResource(int pId, string pResourceKey, string pResourceValue)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_LanguageResource_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pResourceKey, pResourceValue });
        }
    }
}

