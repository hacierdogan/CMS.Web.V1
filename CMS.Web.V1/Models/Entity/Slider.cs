using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Slider
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PicturePath { get; set; }
        public DateTime StartDate { get; set; }
        public string StartDateStr { get { return StartDate.ToString("dd.MM.yyyy"); } }
        public DateTime EndDate { get; set; }
        public string EndDateStr { get { return EndDate.ToString("dd.MM.yyyy"); } }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }
        public List<SliderDetail> Detail { get; set; }
    }

    public class SliderDetail
    {
        public int SliderId { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
    }
}