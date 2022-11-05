using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class CompanyInformationController : AdminBaseController
    {
        protected Company companyItem
        {
            get { return (Company)Session["companyItem"]; }
            set { Session["companyItem"] = value; }
        }
        // GET: Admin/CompanyInformation
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetCompany()
        {
            try
            {
                return JsonConvert.SerializeObject(CompanyDAL.GetListCompanyInformation().FirstOrDefault());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetCompanyInformation", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string WriteCompany(Company company)
        {
            if (company != null)
            {
                company.Name = company.Name == null ? string.Empty : company.Name;
                company.TradeName = company.TradeName == null ? string.Empty : company.TradeName;
                company.Address = company.Address == null ? string.Empty : company.Address;
                company.Location = company.Location == null ? string.Empty : company.Location;
                company.Mail = company.Mail == null ? string.Empty : company.Mail;
                company.Phone = company.Phone == null ? string.Empty : company.Phone;
                company.Fax = company.Fax == null ? string.Empty : company.Fax;
                company.LogoPath = company.LogoPath == null ? string.Empty : company.LogoPath;
                company.LogoBasePath = company.LogoBasePath == null ? string.Empty : company.LogoBasePath;
                companyItem = company;
            }
            return JsonConvert.SerializeObject("");
        }
       
        [HttpPost]
        public JsonResult SaveCompany()
        {
            bool result = false;
            try
            {
                string pathUrl = Server.MapPath("/files/upload/company_images/");

                if (!Directory.Exists(pathUrl))
                {
                    Directory.CreateDirectory(pathUrl);
                }

                if (Request.Files.Count > 0)
                {
                    string fileName = "";
                    string path = "";
                    HttpPostedFileBase postedFile = Request.Files[0];
                    if ((postedFile != null) && (postedFile.ContentLength > 0) && !string.IsNullOrEmpty(postedFile.FileName))
                    {
                        var arr = Path.GetFileName(postedFile.FileName).Split('.');
                        //fileName = Guid.NewGuid() + "." + arr[1];
                        fileName = "logo" + "." + arr[1];
                        path = Path.Combine(Server.MapPath("~/files/upload/company_images/"), fileName);
                        postedFile.SaveAs(path);

                        if (System.IO.File.Exists(Server.MapPath(companyItem.LogoPath)))
                        {
                            System.IO.File.Delete(Server.MapPath(companyItem.LogoPath));
                        }
                        companyItem.LogoPath = "/files/upload/company_images/" + fileName;
                    }
                }
                if (companyItem != null)
                {
                    result = CompanyDAL.UpdateCompany(companyItem.Name, companyItem.TradeName, companyItem.Address,companyItem.Location, companyItem.Mail, companyItem.Phone, companyItem.Fax, companyItem.LogoPath, companyItem.LogoBasePath,companyItem.PdfPath);
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveCompanyInformation", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }

        [HttpPost]
        public JsonResult SavePdf()
        {
            Company company = CompanyDAL.GetListCompanyInformation().FirstOrDefault();
            string contentHtml = System.IO.File.ReadAllText(Server.MapPath("/files/pdftemplate/company.html"));
            contentHtml = contentHtml.Replace("{Date}", DateTime.Now.ToString("dd.MM.yyyy"))
                .Replace("{Logo}", company.LogoBasePath)
                .Replace("{CompanyName}", company.Name)
                .Replace("{TradeName}", company.TradeName)
                .Replace("{Mail}", company.Mail)
                .Replace("{Phone}", company.Phone)
                .Replace("{Fax}", company.Fax)
                .Replace("{Address}", company.Address)
                .Replace("{StartDate}", GlobalSettings.SiteStartDate)
                .Replace("{HostingCompany}", GlobalSettings.HostingCompany)
                .Replace("{HostEndDate}", GlobalSettings.HostingDate)
                .Replace("{DoaminCompany}", GlobalSettings.DomainCompany)
                .Replace("{DomainEndDate}", GlobalSettings.DomainDate)
                .Replace("{Domain}", GlobalSettings.DomainAddress)
                ;
            //string path = "files/Pdf/"+ DateTime.Now.ToString("dd-MM-yyyy") + "_"+Guid.NewGuid() + ".pdf";
            string path = "files/Pdf/company_info.pdf";

            if (System.IO.File.Exists(Server.MapPath(company.PdfPath)))
            {
                System.IO.File.Delete(Server.MapPath(company.PdfPath));
            }
            company.PdfPath = "/"+path;
            WriteCompany(company);
            bool result = CompanyDAL.UpdateCompany(companyItem.Name, companyItem.TradeName, companyItem.Address, companyItem.Location, companyItem.Mail, companyItem.Phone, companyItem.Fax, companyItem.LogoPath, companyItem.LogoBasePath, companyItem.PdfPath);

            return Json(TuesPechkinInitializerService.StartConvertToHtml(contentHtml, path, Request, paperOrientation: TuesPechkin.GlobalSettings.PaperOrientation.Portrait));
        }

    }
}