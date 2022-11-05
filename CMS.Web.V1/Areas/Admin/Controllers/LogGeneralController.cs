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
    public class LogGeneralController : AdminBaseController
    {
        // GET: Admin/LogGeneral
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetLogList(int type)
        {
            try
            {
                return JsonConvert.SerializeObject(LogGeneralErrors.GetAllLogGeneral(type));
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetLogList", ex, GetUserIpAddress());
                throw;
            }
        }
    }
}