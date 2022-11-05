using CMS.Web.V1.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class PartialController : AdminBaseController
    {
        // GET: Admin/Prtial
        public ActionResult Index()
        {
            return View();
        }
        public PartialViewResult TopMenu()
        {
            return PartialView();
        }
        public PartialViewResult LeftMenu()
        {
            return PartialView();
        }
               
    }
}