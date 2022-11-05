using CMS.Web.V1.Models.DataAcces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Controllers
{
    public class PartialController : Controller
    {
        public PartialViewResult _Menu()
        {
            return PartialView();
        }
        public PartialViewResult HomeSlider()
        {
            return PartialView();
        }
        public PartialViewResult HomeAbout()
        {
            return PartialView();
        }
        public PartialViewResult HomeServices()
        {
            return PartialView();
        }
        public PartialViewResult HomeTabs()
        {
            return PartialView();
        }
        public PartialViewResult HomeGallery()
        {
            return PartialView();
        }
        public PartialViewResult HomeComments()
        {
            return PartialView();
        }
        public PartialViewResult HomeCounts()
        {
            return PartialView();
        }
        public PartialViewResult HomeBlog()
        {
            return PartialView();
        }
        public ActionResult HomeReferences()
        {
            return PartialView();
        }
        public PartialViewResult HomeContact()
        {
            return PartialView();
        }
        public PartialViewResult _Footer()
        {
            return PartialView();
        }
        public PartialViewResult _SocialMedia()
        {
            return PartialView();
        }
        public PartialViewResult _Popup()
        {
            return PartialView();
        }
        public ActionResult _SeoSetting()
        {
            return View(SeoSettingDAL.GetSeoSetting());
        }
        public PartialViewResult _PageLoader()
        {
            return PartialView();
        }
    }
}