using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class UserDAL : DataAccess
    {
        public static List<User> GetUserList()
        {
            List<User> userList = new List<User>();
            DataTable dt = DAL.GetUserList();
            foreach (DataRow row in dt.Rows)
            {

                User user = new User();
                user.Id = row.Field<int>("Id");
                user.Name = row.Field<string>("Name");
                user.Mail = row.Field<string>("Mail");
                user.Password = row.Field<string>("Password");
                user.Role = row.Field<string>("Role");
                user.LastPaswordChangeDate = row.Field<DateTime>("LastPaswordChangeDate");
                user.Status = row.Field<bool>("Status");
                userList.Add(user);
            }
            return userList;
        }
        
        public static bool UpdateUser(int id, string name, string mail, string password, string role,bool status,bool isResetPassword)
        {
            return DAL.UpdateUser(id, name, mail, password, role,status, isResetPassword);
        }
        public static bool AddUser(string name, string mail, string password, string role)
        {
            return DAL.AddUser(name,mail,password,role);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetUserList()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "User_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdateUser(int pId, string pName, string pMail, string pPassword, string pRole,bool pStatus,bool pIsResetPassword)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "User_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] {pId, pName, pMail, pPassword, pRole, pStatus, pIsResetPassword });
        } 
        public bool AddUser(string pName, string pMail, string pPassword, string pRole)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "User_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pName, pMail, pPassword, pRole});
        }
    }
}