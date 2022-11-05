using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class GalleryDAL : DataAccess
    {
        public static List<Gallery> GetAllGallery()
        {
            List<Gallery> galleryList = new List<Gallery>();
            List<GalleryDetail> detailList = GetAllGalleryDetail();
            DataTable dt = DAL.GetListGallery();
            foreach (DataRow row in dt.Rows)
            {
                Gallery gallery = new Gallery();
                {
                    gallery.Id = row.Field<int>("Id");
                    gallery.PicturePath = row.Field<string>("PicturePath");
                    gallery.Status = row.Field<bool>("Status");
                    gallery.DisplayOrder = row.Field<int>("DisplayOrder");
                    gallery.Detail = detailList.Where(w => w.GalleryId == gallery.Id).ToList();
                }
                galleryList.Add(gallery);
            }
            return galleryList;
        }
        public static int AddGallery(string path, bool status,int displayOrder)
        {
            DataTable dt = DAL.AddGallery(path, status, displayOrder);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateGallery(int id, string path, bool status, int displayOrder)
        {
            return DAL.EditGallery(id, path, status, displayOrder);
        }
        public static bool DeleteGallery(int id)
        {
            return DAL.DeleteGallery(id);
        }
        public static List<GalleryDetail> GetAllGalleryDetail()
        {
            DataTable detail_dt = DAL.GetListGalleryDetail();
            List<GalleryDetail> detailList = new List<GalleryDetail>();
            foreach (DataRow detailRow in detail_dt.Rows)
            {
                GalleryDetail detail = new GalleryDetail();
                detail.DetailId = detailRow.Field<int>("DetailId");
                detail.GalleryId = detailRow.Field<int>("GalleryId");
                detail.Title = detailRow.Field<string>("Title");
                detail.ButtonText = detailRow.Field<string>("ButtonText");
                detail.LanguageId = detailRow.Field<int>("LanguageId");
                detail.Language = detailRow.Field<string>("Name");
                detailList.Add(detail);
            }
            return detailList;
        }
        public static bool AddGalleryDetail(int galleryId, int languageId, string title, string buttonText)
        {
            return DAL.AddGalleryDetail(galleryId, languageId, title, buttonText);
        }
        public static bool UpdateGalleryDetail(int id, string title, string buttonText)
        {
            return DAL.EditGalleryDetail(id,  title, buttonText);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetListGallery()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Gallery_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }

        public DataTable AddGallery(string pPath, bool pStatus,int pDisplayOrder)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Gallery_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] {pPath, pStatus, pDisplayOrder });
        }

        public bool EditGallery(int pId, string pPath, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Gallery_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pPath, pStatus, pDisplayOrder });
        }

        public bool DeleteGallery(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Gallery_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }

        public DataTable GetListGalleryDetail()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_GalleryDetail_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool AddGalleryDetail(int pGalleryId, int pLanguageId, string pTitle, string pButtonText)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_GalleryDetail_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pGalleryId, pLanguageId, pTitle, pButtonText});
        }

        public bool EditGalleryDetail(int pDetailId,string pTitle, string pButtonText)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_GalleryDetail_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pDetailId, pTitle, pButtonText });
        }

    }
}

