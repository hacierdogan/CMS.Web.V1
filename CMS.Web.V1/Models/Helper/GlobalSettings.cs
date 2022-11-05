using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Helper
{
    public class GlobalSettings
    {
        public static string Ip => ConfigurationManager.AppSettings["ip"];
        public static uint Port => Convert.ToUInt32(ConfigurationManager.AppSettings["port"]);
        public static string DbServer => ConfigurationManager.AppSettings["dbServer"];
        public static string Database => ConfigurationManager.AppSettings["database"];
        public static string UserName => ConfigurationManager.AppSettings["userName"];
        public static string Password => ConfigurationManager.AppSettings["password"];
        public static string DomainAddress => ConfigurationManager.AppSettings["domainAddress"];
        public static string WebAddress=> ConfigurationManager.AppSettings["webAddress"];
        public static string WebAddressLocal => ConfigurationManager.AppSettings["webAddressLocal"];
        public static string Developer => ConfigurationManager.AppSettings["developer"];
        public static string SiteStartDate => ConfigurationManager.AppSettings["siteStartDate"];
        public static string HostingDate => ConfigurationManager.AppSettings["hostingDate"];
        public static string HostingCompany => ConfigurationManager.AppSettings["hostingCompany"];
        public static string DomainDate => ConfigurationManager.AppSettings["domainDate"];
        public static string DomainCompany => ConfigurationManager.AppSettings["domainCompany"];

        public static string ConnectionString{
            get
            {
                string cs = string.Empty;
                SqlConnectionStringBuilder csBuilder = new SqlConnectionStringBuilder();

                csBuilder.DataSource = GlobalSettings.Port == 1234 ? GlobalSettings.Ip : GlobalSettings.Ip + "," + GlobalSettings.Port;
                csBuilder.DataSource = GlobalSettings.DbServer;
                csBuilder.InitialCatalog = GlobalSettings.Database;
                csBuilder.UserID = GlobalSettings.UserName;
                csBuilder.Password = GlobalSettings.Password;
                csBuilder.IntegratedSecurity = true;
                csBuilder.MultipleActiveResultSets = true;
                csBuilder.ConnectTimeout = 120;

                cs = csBuilder.ToString();
                return cs;
            }
        }
    }
}