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
    public class ReferenceController : BaseController
    {
        // GET: Reference
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetReferenceList()
        {
            List<Referance> referenceList = ReferanceDAL.GetAllReferance().Where(w => w.Status == true).ToList();
            return JsonConvert.SerializeObject(referenceList.OrderBy(o => o.DisplayOrder));
        }
    }
}