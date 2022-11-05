using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class SocialMediaController : AdminBaseController
    {
        // GET: Admin/SocialMedia
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetSocialMedia()
        {
            try
            {
                return JsonConvert.SerializeObject(SocialMediaDAL.GetSocialMedia().FirstOrDefault());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetSocialMedia", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveSocialMedia(SocialMedia social)
        {
            bool result = false;
            try
            {
                social.FacebookUrl = social.FacebookUrl == null ? string.Empty : social.FacebookUrl;
                social.TwitterUrl = social.TwitterUrl == null ? string.Empty : social.TwitterUrl;
                social.YoutubeUrl = social.YoutubeUrl == null ? string.Empty : social.YoutubeUrl;
                social.LinkedinUrl = social.LinkedinUrl == null ? string.Empty : social.LinkedinUrl;
                social.InstagramUrl = social.InstagramUrl == null ? string.Empty : social.InstagramUrl;
                result = SocialMediaDAL.UpdateSocialMedia(social.FacebookUrl, social.IsFacebook, social.TwitterUrl, social.IsTwitter, social.YoutubeUrl, social.IsYoutube, social.LinkedinUrl, social.IsLinkedin, social.InstagramUrl, social.IsInstagram);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UpdateSocialMedia", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
    }
}