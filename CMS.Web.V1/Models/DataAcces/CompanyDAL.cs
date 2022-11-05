using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CMS.Web.V1.Models.DataAcces
{
    public class CompanyDAL : DataAccess
    {
        public static List<Company> GetListCompanyInformation()
        {
            List<Company> companyList = new List<Company>();
            DataTable dt = DAL.GetCompany();
            foreach (DataRow row in dt.Rows)
            {
                Company company = new Company();
                {
                    company.Name = row.Field<string>("Name");
                    company.TradeName = row.Field<string>("TradeName");
                    company.Address = row.Field<string>("Address");
                    company.Location = row.Field<string>("Location");
                    company.Mail = row.Field<string>("Mail");
                    company.Phone = row.Field<string>("Phone");
                    company.Fax = row.Field<string>("Fax");
                    company.LogoPath = row.Field<string>("LogoPath");
                    company.LogoBasePath = row.Field<string>("LogoBasePath");
                    company.PdfPath = row.Field<string>("PdfPath");
                 
                }
                companyList.Add(company);
            }
            return companyList;
        }
        public static bool UpdateCompany(string name, string tradeName, string address,string location, string mail, string phone, string fax, string logoPath, string logoBasePath,string pdfPath)
        {
            return DAL.EditCompany(name, tradeName, address, location, mail, phone, fax, logoPath, logoBasePath,pdfPath);
        }
    }
    public partial class DataAccessLayer
    {
        public DataTable GetCompany()
        {
            return DatabaseContext.ExecuteReader(CommandType.StoredProcedure, "Admin_Company_GetList", MethodBase.GetCurrentMethod().GetParameters(), new object[] { });
        }
        public bool EditCompany(string pName, string pTradeName, string pAddress,string pLocation, string pMail, string pPhone,string pFax,string pLogoPath, string pLogoBasePath,string pPdfPath)
        {
            return DatabaseContext.ExecuteNonQuery(CommandType.StoredProcedure, "Admin_Company_Edit", MethodBase.GetCurrentMethod().GetParameters(), new object[] { pName, pTradeName, pAddress, pLocation, pMail, pPhone, pFax, pLogoPath, pLogoBasePath, pPdfPath });
        }

    }
}