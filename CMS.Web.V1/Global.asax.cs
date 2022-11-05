using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CMS.Web.V1
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            string version = "v=1.0";
            HttpContext.Current.Application["JSVer"] = version;
            HttpContext.Current.Application["CSSVer"] = version;

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
