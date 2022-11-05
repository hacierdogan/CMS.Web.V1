using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Controllers
{
    public class NewsController : BaseController
    {
        // GET: News
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetBlogOrNews()
        {
            List<Content> blogList = ContentDAL.GetAllContent((int)ContentType.Blog).Where(w => w.Status == true).ToList();
            foreach (var blog in blogList)
            {
                blog.Detail = blog.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList();
            }
            return JsonConvert.SerializeObject(blogList.OrderBy(o => o.DisplayOrder));
        }
    }
}