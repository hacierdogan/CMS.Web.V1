using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class OurService
    {
        public int Id { get; set; }
        public string Icon { get; set; }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }
        public List<OurServiceDetail> Detail { get; set; }
    }

    public class OurServiceDetail
    {
        public int OurServiceId { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}