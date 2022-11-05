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
    public class PopupController : AdminBaseController
    {
        protected Popup popupItem
        {
            get { return (Popup)Session["popupItem"]; }
            set { Session["popupItem"] = value; }
        }

        // GET: Admin/Popup
        public ActionResult Index()
        {

            return View();
        }

        [HttpPost]
        public string GetPopupList()
        {
            try
            {
                return JsonConvert.SerializeObject(PopupDAL.GetAllPopup().FirstOrDefault());

            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetPopupList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WritePopup(Popup popup)
        {
            if (popup != null)
            {
                foreach (var item in popup.Detail)
                {
                    item.Title = item.Title == null ? string.Empty : item.Title;
                    item.Description = item.Description == null ? string.Empty : item.Description;
                }
                popupItem = popup;
            }
            return JsonConvert.SerializeObject("");
        }

        [HttpPost]
        public JsonResult SavePopup()
        {
            MessageBox message;
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/popup_images/");

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
                        path = Path.Combine(Server.MapPath("~/files/upload/popup_images/"), fileName);
                        postedFile.SaveAs(path);

                        if (popupItem.Id != 0 && System.IO.File.Exists(Server.MapPath(popupItem.PicturePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(popupItem.PicturePath));
                        }
                        popupItem.PicturePath = "/files/upload/popup_images/" + fileName;
                    }
                }

                if (popupItem != null)
                {
                    bool updatedSlider = PopupDAL.UpdatePopup(popupItem.PicturePath, popupItem.Type, popupItem.Size, popupItem.StartDate, popupItem.EndDate,popupItem.Status);
                    foreach (var item in popupItem.Detail)
                    {
                        PopupDAL.UpdatePopupDetail(item.DetailId, item.Title, item.Description);
                    }
                    result = true;

                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SavePopup", ex, GetUserIpAddress());
                result = false;
            }
            message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

    }
}