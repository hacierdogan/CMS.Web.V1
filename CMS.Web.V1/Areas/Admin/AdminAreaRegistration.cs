using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName { get { return "Admin"; } }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin",
                "admin",
                new { AreaName = "Admin", controller = "Home", action = "Index" }
            );
            // context.MapRoute(
            //    "AdminContentPage",
            //    "admin/content/{id}",
            //    new { AreaName = "Admin", controller = "content", action = "Index", id = UrlParameter.Optional }
            //);
            context.MapRoute(
                "AdminDefault",
                "Admin/{controller}/{action}/{id}",
                new { AreaName = "Admin", controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "CMS.Web.V1.Areas.Admin.Controllers" }
            );
        }
    }
}