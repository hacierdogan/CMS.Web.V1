using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Referance
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string PicturePath { get; set; }
        public int DisplayOrder { get; set; }
        public bool Status { get; set; }
    }
}