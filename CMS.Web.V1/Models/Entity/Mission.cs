using System;

namespace CMS.Web.V1.Models.Entity
{
    public class Mission
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string TimeStr { get; set; }
        public bool Status { get; set; }
    }
}