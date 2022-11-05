using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class ThemeSetting
    {
        public bool Maintenance { get; set; }
        public string DefaultLanguage { get; set; }
        public string LanguageType { get; set; }
        public string FontCDN { get; set; }
        public string FontFamily { get; set; }
        public string Color1 { get; set; }
        public string Color2 { get; set; }
        public string Color3 { get; set; }
        public string Color4 { get; set; }
    }
}