using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Company
    {
        public string Name { get; set; }
        public string TradeName { get; set; }
        public string Address { get; set; }
        public string Location { get; set; }
        public string Mail { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string LogoPath { get; set; }
        public string LogoBasePath { get; set; }
        public string PdfPath { get; set; }
    }
}