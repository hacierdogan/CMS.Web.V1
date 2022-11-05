using System.Data;
using System.Data.SqlClient;

namespace CMS.Web.V1.Models.Helper
{
    public sealed class MsSqlHelper
    {
        private MsSqlHelper(){}

        #region Execute_NonQuery
        public static int ExecuteNonQuery(string connectionString,CommandType myCommandType, string commandText, params SqlParameter[] parms)
        {
            int retValue = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                retValue = MsSqlHelper.ExecuteNonQuery(connection, myCommandType, commandText, parms);
            }
            return retValue;
        }
        public static int ExecuteNonQuery(SqlConnection connection, CommandType myCommandType, string commandText, params SqlParameter[] commandParameters)
        {
            int retValue = 0;
            using (SqlCommand SqlCommand = new SqlCommand())
            {
                SqlCommand.Connection = connection;
                SqlCommand.CommandText = commandText;
                SqlCommand.CommandType = myCommandType;
                SqlCommand.CommandTimeout = 9999999;
                if (commandParameters != null)
                {
                    foreach (SqlParameter SqlParameter in commandParameters)
                        SqlCommand.Parameters.Add(SqlParameter);
                }
                retValue = SqlCommand.ExecuteNonQuery();
                SqlCommand.Parameters.Clear();
            }
            return retValue;
        }
        #endregion


        #region Execute_DataTable
        public static DataTable ExecuteDataTable(string connectionString, CommandType myCommandType, string commandText, params SqlParameter[] commandParameters)
        {
            DataTable retValue;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                retValue = MsSqlHelper.ExecuteDataTable(connection, myCommandType, commandText, commandParameters);
            }
            return retValue;
        }
        public static DataTable ExecuteDataTable(SqlConnection connection, CommandType myCommandType, string commandText, params SqlParameter[] commandParameters)
        {
            DataSet retValue = new DataSet();
            using (SqlCommand selectCommand = new SqlCommand())
            {
                selectCommand.Connection = connection;
                selectCommand.CommandText = commandText;
                selectCommand.CommandType = myCommandType;
                selectCommand.CommandTimeout = 9999999;
                if (commandParameters != null)
                {
                    foreach (SqlParameter SqlParameter in commandParameters)
                        selectCommand.Parameters.Add(SqlParameter);
                }
                SqlDataAdapter SqlDataAdapter = new SqlDataAdapter(selectCommand);
                SqlDataAdapter.Fill(retValue);
                selectCommand.Parameters.Clear();
            }
            return retValue.Tables[0];
        }
        #endregion
    }
}