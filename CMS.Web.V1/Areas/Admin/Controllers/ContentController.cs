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
    public class ContentController : AdminBaseController
    {
        protected Content contentItem
        {
            get { return (Content)Session["ProductionOrderList"]; }
            set { Session["ProductionOrderList"] = value; }
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetContentList(ContentType type)
        {
            try
            {
                return JsonConvert.SerializeObject(ContentDAL.GetAllContent((int)type));
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetContentList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WriteContent(Content content, ContentType type)
        {
            if (content != null)
            {
                foreach (var item in content.Detail)
                {
                    item.Title = item.Title == null ? string.Empty : item.Title;
                    item.Description = item.Description == null ? string.Empty : item.Description;
                }
                content.TabButtonText = content.TabButtonText == null ? string.Empty : content.TabButtonText;
                contentItem = content;
                contentItem.Type = type;
            }
            return JsonConvert.SerializeObject("");
        }

        [HttpPost]
        public JsonResult SaveContent()
        {
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/content_images/");

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
                        path = Path.Combine(Server.MapPath("~/files/upload/content_images/"), fileName);
                        postedFile.SaveAs(path);

                        if (contentItem.Id != 0 && System.IO.File.Exists(Server.MapPath(contentItem.PicturePath)))
                        {
                            System.IO.File.Delete(Server.MapPath(contentItem.PicturePath));
                        }
                        contentItem.PicturePath = "/files/upload/content_images/" + fileName;
                    }
                }

                if (contentItem != null)
                {
                    if (contentItem.Id == 0)
                    {
                        int createdId = ContentDAL.AddContent((int)contentItem.Type, contentItem.PicturePath, contentItem.Status, DateTime.Now, contentItem.DisplayOrder,contentItem.TabButtonText);
                        foreach (var item in contentItem.Detail)
                        {
                            result = ContentDAL.AddContentDetail(createdId, item.LanguageId, item.Title, item.Description);
                        }
                    }
                    else
                    {
                        ContentDAL.UpdateContent(contentItem.Id, (int)contentItem.Type, contentItem.PicturePath, contentItem.Status, contentItem.DisplayOrder,contentItem.TabButtonText);
                        foreach (var item in contentItem.Detail)
                        {
                            result = ContentDAL.UpdateContentDetail(item.DetailId, contentItem.Id, item.LanguageId, item.Title, item.Description);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveContent", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteContent(Content content)
        {
            bool result = false;
            try
            {
                if (content.Id > 1)
                {
                    if (System.IO.File.Exists(Server.MapPath(content.PicturePath)))
                    {
                        System.IO.File.Delete(Server.MapPath(content.PicturePath));
                    }
                    result = ContentDAL.DeleteContent(content.Id);
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "DeleteContent", ex, GetUserIpAddress());
                throw;
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İçerik silindi.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}