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
    public class AboutController : BaseController
    {
        // GET: About
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetAbout()
        {
            Content content = ContentDAL.GetAllContent((int)ContentType.About).FirstOrDefault();
            content.Detail = content.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList() ;
            return JsonConvert.SerializeObject(content);
        }
    }
}