using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class MissionDAL : DataAccess
    {
        public static List<Mission> GetMissionList()
        {
            List<Mission> missionList = new List<Mission>();
            DataTable dt = DAL.GetMissionList();
            foreach (DataRow row in dt.Rows)
            {

                Mission mission = new Mission();
                mission.Id = row.Field<int>("Id");
                mission.Description = row.Field<string>("Description");
                mission.Date = row.Field<DateTime>("CreateDate");
                mission.Status = row.Field<bool>("Status");
                missionList.Add(mission);
            }
            return missionList;
        }
        
        public static bool UpdateMission(int id,bool status,bool delete)
        {
            return DAL.UpdateMission(id,status,delete);
        }
        public static bool AddMission(string description)
        {
            return DAL.AddMission(description);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetMissionList()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Mission_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdateMission(int pId,bool pStatus, bool pDelete)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Mission_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] {pId , pStatus, pDelete});
        } 
        public bool AddMission(string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Mission_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pDescription });
        }
    }
}