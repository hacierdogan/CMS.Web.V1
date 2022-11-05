using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class ReferanceDAL : DataAccess
    {
        //Referance
        public static List<Referance> GetAllReferance()
        {
            List<Referance> referanceList = new List<Referance>();

            DataTable dt = DAL.GetListReferance();

            foreach (DataRow row in dt.Rows)
            {
                Referance referance = new Referance();
                {
                    referance.Id = row.Field<int>("Id");
                    referance.Title = row.Field<string>("Title");
                    referance.Url = row.Field<string>("Url");
                    referance.PicturePath = row.Field<string>("PicturePath");
                    referance.DisplayOrder = row.Field<int>("DisplayOrder");
                    referance.Status = row.Field<bool>("Status");

                }
                referanceList.Add(referance);
            }
            return referanceList;
        }
        public static int AddReferance(string title,string url, string path, bool status, int displayOrder)
        {
            DataTable dt = DAL.AddReferance(title, url, path, status, displayOrder);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateReferance(int id, string title, string url, string path,  bool status, int displayOrder)
        {
            return DAL.EditReferance(id, title, url, path, status, displayOrder);
        }
        public static bool DeleteReferance(int id)
        {
            return DAL.DeleteReferance(id);
        }

       
     
    }
    public partial class DataAccessLayer
    {
        //Referance
        public DataTable GetListReferance()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Referance_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable AddReferance(string pTitle, string pUrl, string pPath, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Referance_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pTitle, pUrl,pPath, pStatus, pDisplayOrder });
        }
        public bool EditReferance(int pId, string pTitle, string pUrl, string pPath, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Referance_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pTitle, pUrl, pPath, pStatus, pDisplayOrder });
        }
        public bool DeleteReferance(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Referance_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }

    }
}

