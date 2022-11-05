using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class DatabaseContext
    {
        public static DataTable ExecuteReader(CommandType commandType, string commandText, ParameterInfo[] parameterNames, object[] parameterValues)
        {
            try
            {
                return MsSqlHelper.ExecuteDataTable(GlobalSettings.ConnectionString, commandType, commandText, GenerateParam(parameterNames, parameterValues));
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.System, "DatabaseContext-ExecuteDataTable", ex, "0.0.0.0");
                return new DataTable();
            }
        }

        public static bool ExecuteNonQuery(CommandType commandType, string commandText, ParameterInfo[] parameterNames, object[] parameterValues)
        {
            try
            {
                MsSqlHelper.ExecuteNonQuery(GlobalSettings.ConnectionString, CommandType.StoredProcedure, commandText, GenerateParam(parameterNames, parameterValues));
                return true;
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.System, "DatabaseContext-ExecuteNonQuery", ex, "0.0.0.0");
                return false;
            }
        }

        // parameter => @ + parameter 
        public static SqlParameter[] GenerateParam(ParameterInfo[] parameterNames, params object[] parameterValues)
        {
            int length = parameterNames.Length;
            SqlParameter[] sqlParams = new SqlParameter[length];

            for (int i = 0; i < length; i++)
            {
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@" + parameterNames[i].Name;
                parameter.Value = parameterValues[i] == null ? "" : parameterValues[i];
                sqlParams[i] = parameter;
            }

            return sqlParams;
        }
       
    }
}