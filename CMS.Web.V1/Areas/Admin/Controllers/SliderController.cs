using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class SliderController : AdminBaseController
    {
        protected Slider sliderItem
        {
            get { return (Slider)Session["sliderItem"]; }
            set { Session["sliderItem"] = value; }
        }
        // GET: Admin/Slider
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetSliderList()
        {
            try
            {
                return JsonConvert.SerializeObject(SliderDAL.GetAllSlider());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetSliderList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WriteSlider(Slider slider)
        {
            if (slider != null)
            {
                foreach (var item in slider.Detail)
                {
                    item.Title = item.Title == null ? string.Empty : item.Title;
                    item.SubTitle = item.SubTitle == null ? string.Empty : item.SubTitle;
                    item.Description = item.Description == null ? string.Empty : item.Description;
                }
                sliderItem = slider;
            }
            return JsonConvert.SerializeObject("");
        }

        [HttpPost]
        public JsonResult SaveSlider()
        {
            MessageBox message ;
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/slider_images/");

                if (!Directory.Exists(pathUrl))
                {
                    Directory.CreateDirectory(pathUrl);
                }

                if (Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = "";
                    HttpPostedFileBase postedFile = Request.Files[0];
                    if ((postedFile != null) && (postedFile.ContentLength > 0) && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        var arr = Path.GetFileName(postedFile.FileName).Split('.');
                        fileName = Guid.NewGuid() + "." + arr[1];
                        path = Path.Combine(Server.MapPath("~/files/upload/slider_images/"), fileName);
                        postedFile.SaveAs(path);

                        if ( sliderItem.Id!=0 && System.IO.File.Exists(Server.MapPath(sliderItem.PicturePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(sliderItem.PicturePath));
                        }
                        sliderItem.PicturePath = "/files/upload/slider_images/" + fileName;
                    }
                }

                if (sliderItem != null)
                {
                    
                    if (sliderItem.Id == 0)
                    {
                        int createdSliderId = SliderDAL.AddSlider(sliderItem.Name, sliderItem.PicturePath, sliderItem.StartDate, sliderItem.EndDate, sliderItem.Status,sliderItem.DisplayOrder);
                        foreach (var item in sliderItem.Detail)
                        {
                            SliderDAL.AddSliderDetail(createdSliderId, item.LanguageId, item.Title, item.SubTitle, item.Description);
                        }
                        result = true;
                    }
                    else
                    {
                        bool updatedSlider = SliderDAL.UpdateSlider(sliderItem.Id, sliderItem.Name, sliderItem.PicturePath, sliderItem.StartDate, sliderItem.EndDate, sliderItem.Status,sliderItem.DisplayOrder);
                        foreach (var item in sliderItem.Detail)
                        {
                            SliderDAL.UpdateSliderDetail(sliderItem.Id, item.LanguageId, item.Title, item.SubTitle, item.Description);
                        }
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveSlider", ex, GetUserIpAddress());
                result = false;
            }
            message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteSlider(Slider slider)
        {
            bool result = false;
            try
            {
                if (slider.Id > 0)
                {
                    result = SliderDAL.DeleteSlider(slider.Id);

                    if (System.IO.File.Exists(Server.MapPath(slider.PicturePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(slider.PicturePath));
                    }                  
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "DeleteSlider", ex, GetUserIpAddress());
                result = false;
            }

            var message = result  ? new MessageBox(MessageBoxType.Success, "İşlem Başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}