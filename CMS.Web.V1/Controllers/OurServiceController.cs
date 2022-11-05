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
    public class OurServiceController : BaseController
    {
        // GET: OurService
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetOurServiceList()
        {
            List<OurService> ourServiceList = OurServiceDAL.GetAllOurService().Where(w => w.Status == true).ToList();
            foreach (var serviceItem in ourServiceList)
            {
                serviceItem.Detail=serviceItem.Detail.Where(w => w.LanguageId == CurrentLanguage.Id).ToList();
            }
            return JsonConvert.SerializeObject(ourServiceList.OrderBy(o => o.DisplayOrder));
        }
    }
}