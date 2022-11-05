using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Threading.Tasks;

namespace CMS.Web.V1.Models.DataAcces
{
    public class LogGeneralErrors : DataAccess
    {
        public static Task<bool> LogGeneral(LogGeneralErrorType type, ClientType client, string source, Exception ex, string ipAddress)
        {
            return Task.Run(() => DAL.AddLog((int)type, (int)client, source, ex.Message,ex.ToString(), ipAddress));
        }
        public static List<LogGeneral> GetAllLogGeneral(int type)
        {
            List<LogGeneral> logList = new List<LogGeneral>();
            DataTable dt = DAL.GetListLog(type);
            foreach (DataRow row in dt.Rows)
            {
                LogGeneral log = new LogGeneral();
                {
                    log.Id = row.Field<int>("LogId");
                    log.LogType = (LogGeneralErrorType)Convert.ToInt32(row["Type"]);
                    log.Client = (ClientType)Convert.ToInt32(row["Client"]);
                    log.Source = row.Field<string>("Source");
                    log.DateTime = row.Field<DateTime>("DateTime");
                    log.Message = row.Field<string>("Message");
                    log.Explanation = row.Field<string>("Explanation");
                    log.IpAddress = row.Field<string>("Ip");
                }
                logList.Add(log);
            }
            return logList;
        }
    }

    public partial class DataAccessLayer
    {
        public bool AddLog(int pLogType, int pClient, string pSource, string pMessage, string pExplanation, string pIpAddress)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "LogGeneralError_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pLogType, pClient, pSource, pMessage, pExplanation, pIpAddress });
        }
        public DataTable GetListLog(int pType)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "LogGeneralError_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] {pType });
        }
    }
}