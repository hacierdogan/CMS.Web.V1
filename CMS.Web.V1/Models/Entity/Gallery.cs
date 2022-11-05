using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Gallery
    {
        public int Id { get; set; }
        public string PicturePath { get; set; }
        public bool Status { get; set; }
        public int DisplayOrder { get; set; }
        public List<GalleryDetail> Detail { get; set; }
    }
    public class GalleryDetail
    {
        public int GalleryId { get; set; }
        public int DetailId { get; set; }
        public int LanguageId { get; set; }
        public string Language { get; set; }
        public string Title { get; set; }
        public string ButtonText { get; set; }
    }
}