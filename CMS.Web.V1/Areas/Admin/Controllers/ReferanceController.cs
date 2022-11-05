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
    public class ReferanceController : AdminBaseController
    {
        protected Referance referanceItem
        {
            get { return (Referance)Session["referanceItem"]; }
            set { Session["referanceItem"] = value; }
        }
        // GET: Admin/Referance
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetReferanceList()
        {
            try
            {
                return JsonConvert.SerializeObject(ReferanceDAL.GetAllReferance());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetSReferanceList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WriteReferance(Referance referance)
        {
            if (referance != null)
            {
                referance.Title = referance.Title == null ? string.Empty : referance.Title;
                referance.Url = referance.Url == null ? string.Empty : referance.Url;
                referanceItem = referance;
            }
            return JsonConvert.SerializeObject("");
        }

        [HttpPost]
        public JsonResult SaveReferance()
        {
            MessageBox message;
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/referance_images/");

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
                        path = Path.Combine(Server.MapPath("~/files/upload/referance_images/"), fileName);
                        postedFile.SaveAs(path);

                        if (referanceItem.Id != 0 && System.IO.File.Exists(Server.MapPath(referanceItem.PicturePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(referanceItem.PicturePath));
                        }
                        referanceItem.PicturePath = "/files/upload/referance_images/" + fileName;
                    }
                }

                if (referanceItem != null)
                {

                    if (referanceItem.Id == 0)
                    {
                        int createdReferanceId = ReferanceDAL.AddReferance(referanceItem.Title, referanceItem.Url,referanceItem.PicturePath, referanceItem.Status, referanceItem.DisplayOrder);
                        result = true;
                    }
                    else
                    {
                        bool updatedReferance = ReferanceDAL.UpdateReferance(referanceItem.Id, referanceItem.Title, referanceItem.Url, referanceItem.PicturePath, referanceItem.Status, referanceItem.DisplayOrder);
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveReferance", ex, GetUserIpAddress());
                result = false;
            }
            message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteReferance(Referance referance)
        {
            bool result = false;
            try
            {
                if (referance.Id > 0)
                {
                    result = ReferanceDAL.DeleteReferance(referance.Id);

                    if (System.IO.File.Exists(Server.MapPath(referance.PicturePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(referance.PicturePath));
                    }
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "DeleteReferance", ex, GetUserIpAddress());
                result = false;
            }

            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem Başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

    }
}