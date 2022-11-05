using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Seo
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Keywords { get; set; }
        public string Owner { get; set; }
        public string Author { get; set; }
        public string Year { get; set; }
        public string GoogleAnalytic { get; set; }

    }
}