using System;
using System.Collections.Generic;

namespace CMS.Web.V1.Models.Entity
{
    public class Content
    {
        public int Id { get; set; }
        public ContentType Type { get; set; }
        public string PicturePath { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public string CreatedDateStr { get { return CreatedDate.ToString("dd.MM.yyyy"); } }
        public int DisplayOrder { get; set; }
        public string TabButtonText { get; set; }
        public List<ContentDetail> Detail { get; set; }
    }
    public class ContentDetail
    {
        public int DetailId { get; set; }
        public int ContentId { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
    public enum ContentType
    {
        About = 1,
        Tab = 2,
        Blog = 3
    }
}