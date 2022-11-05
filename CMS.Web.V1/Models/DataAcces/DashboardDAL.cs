using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class DashboardDAL : DataAccess
    {
        public static Dashboard GetDashboardInformation()
        {
            Dashboard dashboard = new Dashboard();
            DataTable dt = DAL.GetDashboard();
            foreach (DataRow row in dt.Rows)
            {

                dashboard.Slider = row.Field<int>("Slider");
                dashboard.SliderAll = row.Field<int>("SliderAll");
                dashboard.Blog = row.Field<int>("Blog");
                dashboard.BlogAll = row.Field<int>("BlogAll");
                dashboard.Service = row.Field<int>("Service");
                dashboard.ServiceAll = row.Field<int>("ServiceAll");
                dashboard.Message = row.Field<int>("MessageInboxUnRead");
                dashboard.MessageAll = row.Field<int>("MessageInboxAll");


            }
            return dashboard;
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetDashboard()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_DashBoard_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }

    }
}