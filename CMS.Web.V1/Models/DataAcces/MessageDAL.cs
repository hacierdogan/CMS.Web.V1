using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class MessageDAL : DataAccess
    {
        public static List<Message> GetAllMessage()
        {
            List<Message> messageList = new List<Message>();
            DataTable dt = DAL.GetMessageList();
            foreach (DataRow row in dt.Rows)
            {
                Message message = new Message();
                message.Id = row.Field<int>("Id");
                message.Title = row.Field<string>("Title");
                message.Mail = row.Field<string>("Mail");
                message.Subject = row.Field<string>("Subject");
                message.MessageStr = row.Field<string>("Message");
                message.Deleted = row.Field<bool>("Deleted");
                message.IsRead = row.Field<bool>("IsRead");
                message.Type = row.Field<int>("Type");
                message.CreateDate = row.Field<DateTime>("CreateDate");
                messageList.Add(message);
            }
            return messageList;
        }


        public static bool AddMesage(string title, string mail, string subject, string message)
        {
            return DAL.InserMessage(title, mail, subject, message);
        }
        public static bool UpdateMessage(int id, int type, bool deleted, bool read)
        {
            return DAL.UpdateMessage(id, type, deleted, read);
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetMessageList()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Message_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] {});
        }
        public bool InserMessage(string pTitle, string pMail,string pSubject, string pMessage)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Message_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pTitle, pMail, pSubject, pMessage });
        }
        public bool UpdateMessage(int pId, int pType, bool pDeleted,bool pRead)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Message_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pType, pDeleted, pRead });
        }
    }
}