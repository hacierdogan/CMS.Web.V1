namespace CMS.Web.V1.Models.Entity
{
    public class LanguageResource
    {
        public int ResourceId { get; set; }
        public string ResourceKey { get; set; }
        public string ResourceValue { get; set; }
        public string LangCode { get; set; }
        public string LangName { get; set; }
        public int LangId { get; set; }
    }
}