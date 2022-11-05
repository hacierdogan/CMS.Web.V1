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
    public class GalleryController : AdminBaseController
    {
        protected Gallery galleryItem
        {
            get { return (Gallery)Session["galleryItem"]; }
            set { Session["galleryItem"] = value; }
        }

        // GET: Admin/Gallery
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetGalleryList()
        {
            try
            {
                return JsonConvert.SerializeObject(GalleryDAL.GetAllGallery());

            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetGalleryList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WriteGallery(Gallery gallery)
        {
            if (gallery != null)
            {
                foreach (var item in gallery.Detail)
                {
                    item.Title = item.Title == null ? string.Empty : item.Title;
                    item.ButtonText = item.ButtonText == null ? string.Empty : item.ButtonText;
                }
                galleryItem = gallery;
            }
            return JsonConvert.SerializeObject("");
        }

        [HttpPost]
        public JsonResult SaveGallery()
        {
            MessageBox message;
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/gallery_images/");

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
                        path = Path.Combine(Server.MapPath("~/files/upload/gallery_images/"), fileName);
                        postedFile.SaveAs(path);

                        if (galleryItem.Id != 0 && System.IO.File.Exists(Server.MapPath(galleryItem.PicturePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(galleryItem.PicturePath));
                        }
                        galleryItem.PicturePath = "/files/upload/gallery_images/" + fileName;
                    }
                }

                if (galleryItem != null)
                {

                    if (galleryItem.Id == 0)
                    {
                        int createdGalleryId = GalleryDAL.AddGallery(galleryItem.PicturePath, galleryItem.Status, galleryItem.DisplayOrder);
                        
                        foreach (var item in galleryItem.Detail)
                        {
                            result = GalleryDAL.AddGalleryDetail(createdGalleryId, item.LanguageId, item.Title, item.ButtonText);
                        }
                    }
                    else
                    {
                        bool updatedGallery = GalleryDAL.UpdateGallery(galleryItem.Id, galleryItem.PicturePath, galleryItem.Status, galleryItem.DisplayOrder);
                        foreach (var item in galleryItem.Detail)
                        {
                            result = GalleryDAL.UpdateGalleryDetail(item.DetailId,item.Title, item.ButtonText);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveGallery", ex, GetUserIpAddress());
                result = false;
            }
            message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteGallery(Gallery gallery)
        {
            MessageBox message;
            bool result;
            try
            {
                result = GalleryDAL.DeleteGallery(gallery.Id);
                if (result && System.IO.File.Exists(Server.MapPath(gallery.PicturePath)))
                {
                    System.IO.File.Delete(Server.MapPath(gallery.PicturePath));
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "DeleteGallery", ex, GetUserIpAddress());
                result = false;
            }
            message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}