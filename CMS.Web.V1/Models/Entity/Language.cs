namespace CMS.Web.V1.Models.Entity
{
    public class Language
    {
        public int Id { get; set; }
        public string LangName { get; set; }
        public string LangCode { get; set; }
        public string FlagCode { get; set; }
        public bool Status { get; set; }
        public bool Deleted { get; set; }
    }
}