using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Controllers
{
    public class GalleryController : BaseController
    {
        // GET: Gallery
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult GetGalleryList()
        {
            List<Gallery> galleryList = GalleryDAL.GetAllGallery().Where(w => w.Status == true).OrderBy(o => o.DisplayOrder).ToList();
            foreach (var blog in galleryList)
            {
                blog.Detail = blog.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList();
            }
            return Json(new{ GalleryList = galleryList,ButtonList = galleryList.Select(s => s.Detail[0].ButtonText).Distinct() });
        }
    }
}