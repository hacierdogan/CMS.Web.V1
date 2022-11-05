using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class SocialMedia
    {
        public string FacebookUrl { get; set; }
        public bool IsFacebook { get; set; }
        public string TwitterUrl { get; set; }
        public bool IsTwitter { get; set; }
        public string YoutubeUrl { get; set; }
        public bool IsYoutube { get; set; }
        public string LinkedinUrl { get; set; }
        public bool IsLinkedin { get; set; }
        public string InstagramUrl { get; set; }
        public bool IsInstagram { get; set; }
    }
}