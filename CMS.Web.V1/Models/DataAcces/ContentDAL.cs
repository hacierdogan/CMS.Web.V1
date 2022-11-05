using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class ContentDAL : DataAccess
    {
        public static List<Content> GetAllContent(int type)
        {
            List<Content> contentList = new List<Content>();
            List<ContentDetail> detailList = GetAllContentDetail();
            DataTable dt = DAL.GetListContent(type);
            foreach (DataRow row in dt.Rows)
            {
                Content content = new Content();
                {
                    content.Id = row.Field<int>("Id");
                    content.PicturePath = row.Field<string>("PicturePath");
                    content.Status = row.Field<bool>("Status");
                    content.CreatedDate = row.Field<DateTime>("CreatedDate");
                    content.DisplayOrder = row.Field<int>("DisplayOrder");
                    content.TabButtonText= row.Field<string>("TabButtonText");
                    
                    content.Detail = detailList.Where(w=>w.ContentId==content.Id).ToList();
                }
                contentList.Add(content);
            }
            return contentList;
        }
        public static int AddContent(int type, string path, bool status, DateTime date,int displayOrder,string tabButtonText)
        {
            DataTable dt = DAL.AddContent(type, path, status, date, displayOrder,tabButtonText);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateContent(int id, int type, string path, bool status,int displayOrder,string tabButtonText)
        {
            return DAL.EditContent(id, type, path, status, displayOrder,tabButtonText);
        }
        public static bool DeleteContent(int id)
        {
            return DAL.DeleteContent(id);
        }
        public static List<ContentDetail> GetAllContentDetail()
        {
            DataTable detail_dt = DAL.GetListContentDetail();
            List<ContentDetail> detailList = new List<ContentDetail>();
            foreach (DataRow detailRow in detail_dt.Rows)
            {
                ContentDetail detail = new ContentDetail();
                detail.ContentId = detailRow.Field<int>("ContentId");
                detail.DetailId = detailRow.Field<int>("DetailId");
                detail.LanguageId = detailRow.Field<int>("LanguageId");
                detail.Language = detailRow.Field<string>("Name");
                detail.Title = detailRow.Field<string>("Title");
                detail.Description = detailRow.Field<string>("Description");
                detailList.Add(detail);
            }
            return detailList;
        }
        public static bool AddContentDetail(int contentId, int languageId, string title, string description)
        {
            return DAL.AddContentDetail(contentId, languageId, title, description);
        }
        public static bool UpdateContentDetail(int id, int contentId, int languageId, string title, string description)
        {
            return DAL.EditContentDetail(id, contentId, languageId, title, description);
        }
    }
    public partial class DataAccessLayer 
    {
        public DataTable GetListContent(int pType)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Content_GetList_ByType", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pType });
        }

        public DataTable GetListContentDetail()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_ContentDetail_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }

        public DataTable AddContent(int pType, string pPath, bool pStatus, DateTime pDate, int pDisplayOrder,string pTabButtonText)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Content_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pType, pPath, pStatus, pDate, pDisplayOrder, pTabButtonText });
        }

        public bool EditContent(int pId, int pType, string pPath, bool pStatus,int pDisplayOrder,string pTabButtonText)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Content_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pType, pPath, pStatus, pDisplayOrder, pTabButtonText });
        }

        public bool DeleteContent(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Content_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }

        public bool AddContentDetail(int pContentId, int pLanguageId, string pTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_ContentDetail_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pContentId, pLanguageId, pTitle, pDescription });
        }

        public bool EditContentDetail(int pDetailId, int pContentId, int pLanguageId, string pTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_ContentDetail_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pDetailId, pContentId, pLanguageId, pTitle, pDescription });
        }

    }
}

