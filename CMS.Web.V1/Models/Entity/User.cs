using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Mail { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public bool Status { get; set; }
        public DateTime LastPaswordChangeDate { get; set; }
        public string LastPaswordChangeDateStr { get { return LastPaswordChangeDate.ToString("dd.MM.yyy HH:mm");}}
    }
}