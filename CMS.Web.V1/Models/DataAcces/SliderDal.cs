using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace CMS.Web.V1.Models.DataAcces
{
    public class SliderDAL : DataAccess
    {
        //SLIDER
        public static List<Slider> GetAllSlider()
        {
            List<Slider> sliderList = new List<Slider>();
            List<SliderDetail> details = GetAllSliderDetail();

            DataTable dt = DAL.GetListSlider();

            foreach (DataRow row in dt.Rows)
            {
                Slider slider = new Slider();
                {
                    slider.Id = row.Field<int>("Id");
                    slider.Name = row.Field<string>("Name");
                    slider.PicturePath = row.Field<string>("PicturePath");
                    slider.StartDate = row.Field<DateTime>("StartDate");
                    slider.EndDate = row.Field<DateTime>("EndDate");
                    slider.Status = row.Field<bool>("Status");
                    slider.DisplayOrder = row.Field<int>("DisplayOrder");

                    slider.Detail = details.Where(w => w.SliderId == slider.Id).ToList();
                }
                sliderList.Add(slider);
            }
            return sliderList;
        }
        public static List<Slider> GetAllSliderByLanguageId(int languageId)
        {
            List<Slider> sliderList = new List<Slider>();

            DataTable dt = DAL.GetListSliderByLanguage(languageId);

            foreach (DataRow row in dt.Rows)
            {
                Slider slider = new Slider();
                {
                    slider.PicturePath = row.Field<string>("PicturePath");

                    List<SliderDetail> detailList = new List<SliderDetail>();
                    SliderDetail detail = new SliderDetail();
                    detail.Title = row.Field<string>("Title");
                    detail.SubTitle = row.Field<string>("SubTitle");
                    detail.Description = row.Field<string>("Description");
                    detailList.Add(detail);
                    slider.Detail = detailList;

                }
                sliderList.Add(slider);
            }
            return sliderList;
        }
        public static int AddSlider(string name, string path, DateTime startDate, DateTime endDate, bool status, int displayOrder)
        {
            DataTable dt = DAL.AddSlider(name, path, startDate, endDate, status, displayOrder);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateSlider(int id, string name, string path, DateTime startDate, DateTime endDate, bool status, int displayOrder)
        {
            return DAL.EditSlider(id, name, path, startDate, endDate, status, displayOrder);
        }
        public static bool DeleteSlider(int id)
        {
            return DAL.DeleteSlider(id);
        }

        //SLIDER_DETAIL
        public static List<SliderDetail> GetAllSliderDetail()
        {
            DataTable detail_dt = DAL.GetListSliderDetail();
            List<SliderDetail> detailList = new List<SliderDetail>();
            foreach (DataRow detailRow in detail_dt.Rows)
            {
                SliderDetail detail = new SliderDetail();
                detail.SliderId = detailRow.Field<int>("SliderId");
                detail.LanguageId = detailRow.Field<int>("LanguageId");
                detail.Language = detailRow.Field<string>("Name");
                detail.Title = detailRow.Field<string>("Title");
                detail.SubTitle = detailRow.Field<string>("SubTitle");
                detail.Description = detailRow.Field<string>("Description");
                detailList.Add(detail);
            }
            return detailList;
        }
        public static int AddSliderDetail(int sliderId, int languageId, string title, string subTitle, string description)
        {
            DataTable dt = DAL.AddSliderDetail(sliderId, languageId, title, subTitle, description);
            return Convert.ToInt32(dt.Rows[0][0]);
        }
        public static bool UpdateSliderDetail(int sliderId, int languageId, string title, string subTitle, string description)
        {
            return DAL.EditSliderDetail(sliderId, languageId, title, subTitle, description);
        }
    }
    public partial class DataAccessLayer
    {
        //SLIDER
        public DataTable GetListSlider()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Slider_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable GetListSliderByLanguage(int pId)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Slider_GetList_ByLanguageId", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }
        public DataTable AddSlider(string pName, string pPath, DateTime pStartDate, DateTime pEndDate, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Slider_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pName, pPath, pStartDate, pEndDate, pStatus, pDisplayOrder });
        }
        public bool EditSlider(int pId, string pName, string pPath, DateTime pStartDate, DateTime pEndDate, bool pStatus, int pDisplayOrder)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Slider_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId, pName, pPath, pStartDate, pEndDate, pStatus, pDisplayOrder });
        }
        public bool DeleteSlider(int pId)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Slider_Delete", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pId });
        }

        //SLIDER_DETAIL
        public DataTable GetListSliderDetail()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_SliderDetail_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public DataTable AddSliderDetail(int pSliderId, int pLanguageId, string pTitle, string pSubTitle, string pDescription)
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_SliderDetail_Insert", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pSliderId, pLanguageId, pTitle, pSubTitle, pDescription });
        }
        public bool EditSliderDetail(int pSliderId, int pLanguageId, string pTitle, string pSubTitle, string pDescription)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_SliderDetail_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pSliderId, pLanguageId, pTitle, pSubTitle, pDescription });
        }
    }
}

