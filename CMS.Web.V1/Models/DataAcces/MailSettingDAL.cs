using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class MailSettingDAL : DataAccess
    {
        public static Mail GetMailSetting()
        {
            Mail mail = new Mail();
            DataTable dt = DAL.GetMailSetting();
            foreach (DataRow row in dt.Rows)
            {
                mail.Host = row.Field<string>("Host");
                mail.Port = row.Field<int>("Port");
                mail.SSL = row.Field<bool>("SSL");
                mail.From = row.Field<string>("MailFrom");
                mail.Password = row.Field<string>("MailPassword");
                mail.To = row.Field<string>("MailTo");
            }
            return mail;
        }
        public static bool UpdateMailSetting(string host,int port,bool ssl,string from,string password,string to)
        {
            return DAL.UpdateMailSetting(host,port,ssl,from,password,to);
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetMailSetting()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_MailSetting_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdateMailSetting( string pHost,int pPort,bool pSSL,string pMailFrom,string pMailPassword,string pMailTo)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_MailSetting_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] {pHost,pPort,pSSL,pMailFrom,pMailPassword,pMailTo });
        }
    }
}