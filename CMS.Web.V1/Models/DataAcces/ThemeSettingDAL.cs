using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class ThemeSettingDAL : DataAccess
    {
        public static ThemeSetting GetThemeSetting()
        {
            ThemeSetting themeSetting = new ThemeSetting();
            DataTable dt = DAL.GetThemeSetting();
            foreach (DataRow row in dt.Rows)
            {
                themeSetting.Maintenance = row.Field<bool>("Status");
                themeSetting.DefaultLanguage = row.Field<string>("DefaultLanguage");
                themeSetting.LanguageType = row.Field<string>("LanguageType");
                themeSetting.FontCDN = row.Field<string>("FontCDN");
                themeSetting.FontFamily = row.Field<string>("FontFamily");
                themeSetting.Color1 = row.Field<string>("Color1");
                themeSetting.Color2 = row.Field<string>("Color2");
                themeSetting.Color3 = row.Field<string>("Color3");
                themeSetting.Color4 = row.Field<string>("Color4");
            }
            return themeSetting;
        }
        public static bool UpdateThemeSetting(bool status,string defaultLanguage,string languageType, string fontCDN,string fontFamily,string color1,string color2,string color3,string color4)
        {
            return DAL.UpdateThemeSetting(status,defaultLanguage,languageType,fontCDN,fontFamily,color1,color2,color3,color4);
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetThemeSetting()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_ThemeSetting_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdateThemeSetting(bool pStatus,string pDefaultLanguage,string pLanguageType, string pFontCDN,string pFontFamily,string pColor1,string pColor2,string pColor3,string pColor4)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_ThemeSetting_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pStatus,pDefaultLanguage, pLanguageType, pFontCDN,pFontFamily,pColor1,pColor2,pColor3,pColor4});
        }
    }
}