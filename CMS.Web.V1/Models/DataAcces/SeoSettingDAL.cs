using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class SeoSettingDAL : DataAccess
    {
        public static Seo GetSeoSetting()
        {
            Seo seo = new Seo();
            DataTable dt = DAL.GetSeoSetting();
            foreach (DataRow row in dt.Rows)
            {
               seo.Title = row.Field<string>("Title");
               seo.Description = row.Field<string>("Description");
               seo.Keywords = row.Field<string>("Keywords");
               seo.Owner = row.Field<string>("Owner");
               seo.Author = row.Field<string>("Author");
               seo.Year = row.Field<string>("Year");
               seo.GoogleAnalytic = row.Field<string>("GoogleAnalytic");
            }
            return seo;
        }


        public static bool UpdateSeoSetting(string title, string description, string keywords, string owner, string author, string year, string googleAnalytic)
        {
            return DAL.UpdateSeoSetting(title, description, keywords, owner, author, year, googleAnalytic);
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetSeoSetting()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_SeoSetting_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdateSeoSetting(string pTitle, string pDescription, string pKeywords, string pOwner, string pAuthor, string pYear, string pGoogleAnalytic)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_SeoSetting_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pTitle, pDescription, pKeywords, pOwner, pAuthor, pYear, pGoogleAnalytic });
        }
    }
}