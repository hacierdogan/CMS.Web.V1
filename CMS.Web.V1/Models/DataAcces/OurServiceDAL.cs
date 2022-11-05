using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class OurServiceDAL : DataAccess
    {
        public static List<OurService> GetAllOurService()
        {
            List<OurService> serviceList = new List<OurService>();
            List<OurServiceDetail> detailList = GetAllOurServiceDetail();

            DataTable dt = DAL.GetListOurService();
            foreach (DataRow row in dt.Rows)
            {
                OurService service = new OurService();
                {
                    service.Id = row.Field<int>("Id");
                    service.Icon = row.Field<string>("Icon");
                    service.Status = row.Field<bool>("Status");
                    service.DisplayOrder = row.Field<int>("DisplayOrder");
                    
                    service.Detail = detailList.Where(w=>w.OurServiceId==service.Id).ToList();
                }
                serviceList.Add(service);
            }
            return serviceList;
        }
        public static int AddOurService(string icon, bool status,int displayOrder)
        {
            DataTable dt = DAL.AddOurService(icon, status,displayOrder);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateOurService(int id, string icon, bool status,int displayOrder)
        {
            return DAL.EditOurService(id, icon, status,displayOrder);
        }
        public static bool DeleteOurService(int id)
        {
            return DAL.DeleteOurService(id);
        }
        public static List<OurServiceDetail> GetAllOurServiceDetail()
        {
            List<OurServiceDetail> detailList = new List<OurServiceDetail>();
            DataTable detail_dt = DAL.GetListOurServiceDetail();
            foreach (DataRow detailRow in detail_dt.Rows)
            {
                OurServiceDetail detail = new OurServiceDetail();
                detail.OurServiceId = detailRow.Field<int>("OurServiceId");
                detail.LanguageId = detailRow.Field<int>("LanguageId");
                detail.Language = detailRow.Field<string>("Name");
                detail.Title = detailRow.Field<string>("Title");
                detail.Description = detailRow.Field<string>("Description");
                detailList.Add(detail);
            }
            return detailList;
        }
        public static bool AddOurServiceDetail(int ourServiceId, int languageId, string title, string decription)
        {
            return DAL.AddOurServiceDetail(ourServiceId, languageId, title, decription);
        }
        public static bool UpdateOurServiceDetail(int ourServiceId, int languageId, string title, string description)
        {
            return DAL.EditOurServiceDetail(ourServiceId, languageId, title, description);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetListOurService()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_OurService_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable AddOurService(string pIcon, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_OurService_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pIcon, pStatus, pDisplayOrder });
        }
        public bool EditOurService(int pId, string pIcon, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_OurService_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pIcon, pStatus, pDisplayOrder });
        }
        public bool DeleteOurService(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_OurService_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }
        //Detail
        public DataTable GetListOurServiceDetail()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_OurServiceDetail_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool AddOurServiceDetail(int pOurServiceId, int pLanguageId, string pTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_OurServiceDetail_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pOurServiceId, pLanguageId, pTitle, pDescription });
        }
        public bool EditOurServiceDetail(int pOurServiceId, int pLanguageId, string pTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_OurServiceDetail_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pOurServiceId, pLanguageId, pTitle, pDescription });
        }
    }
}