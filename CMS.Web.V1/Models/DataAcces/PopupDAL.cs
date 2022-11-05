using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class PopupDAL : DataAccess
    {
        public static List<Popup> GetAllPopup()
        {
            List<Popup> popupList = new List<Popup>();
            List<PopupDetail> details = GetAllPopupDetail();

            DataTable dt = DAL.GetListPopup();

            foreach (DataRow row in dt.Rows)
            {
                Popup popup = new Popup();
                {
                    popup.Id = row.Field<int>("Id");
                    popup.PicturePath = row.Field<string>("Path");
                    popup.Type = row.Field<int>("Type");
                    popup.Status = row.Field<bool>("Status");
                    popup.Size = row.Field<bool>("Size");
                    popup.StartDate = row.Field<DateTime>("StartDate");
                    popup.EndDate = row.Field<DateTime>("EndDate");

                    popup.Detail = details.ToList();
                }
                popupList.Add(popup);
            }
            return popupList;
        }
        public static bool UpdatePopup(string path,int type,bool size, DateTime startDate, DateTime endDate,bool status)
        {
            return DAL.UpdatePopup(1,path,type,size, startDate, endDate,status);
        }
        public static bool UpdatePopupDetail(int detailId, string title,string description)
        {
            return DAL.UpdatePopupDetail(detailId,title,description);
        }

        public static List<PopupDetail> GetAllPopupDetail()
        {
            DataTable detail_dt = DAL.GetListPopupDetail();
            List<PopupDetail> detailList = new List<PopupDetail>();
            foreach (DataRow detailRow in detail_dt.Rows)
            {
                PopupDetail detail = new PopupDetail();
                detail.DetailId = detailRow.Field<int>("DetailId");
                detail.Title = detailRow.Field<string>("Title");
                detail.Description = detailRow.Field<string>("Description");
                detail.LanguageId = detailRow.Field<int>("LanguageId");
                detail.Language = detailRow.Field<string>("Language");
                detailList.Add(detail);
            }
            return detailList;
        }

    }
    public partial class DataAccessLayer
    {
        public DataTable GetListPopup()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Popup_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable GetListPopupDetail()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_PopupDetail_Get", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool UpdatePopup(int pId,string pPath, int pType, bool pSize, DateTime pStartDate, DateTime pEndDate, bool pStatus)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Popup_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId,pPath, pType, pSize, pStartDate, pEndDate, pStatus });
        }
        public bool UpdatePopupDetail(int pDetailId, string pTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_PopupDetail_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pDetailId, pTitle, pDescription });
        }
    }
}
