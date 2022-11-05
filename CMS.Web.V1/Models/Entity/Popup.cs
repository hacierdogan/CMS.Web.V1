using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Popup
    {
        public int Id { get; set; }
        public string PicturePath { get; set; }
        public int Type { get; set; }
        public bool Size { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get { return StartDate.ToString("dd.MM.yyyy"); } }
        public DateTime EndDate { get; set; }
        public string EndDateStr { get { return EndDate.ToString("dd.MM.yyyy"); } }

        public List<PopupDetail> Detail { get; set; }
    }

    public class PopupDetail
    {
        public int DetailId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
    }
}