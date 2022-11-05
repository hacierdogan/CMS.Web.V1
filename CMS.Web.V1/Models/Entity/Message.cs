using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CMS.Web.V1.Models.Entity
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Mail { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string MessageStr { get; set; }
        public bool Deleted { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreateDate { get; set; }
        public string DateStr { get; set; }
        public bool Selected { get; set; }
        public int Type { get; set; }
        public string TypeStr
        {
            get
            {
                switch (Type)
                {
                    case 1:
                        return "Inbox";
                    case 2:
                        return "Archive";
                    case 3:
                        return "Delete";
                    default:
                        return string.Empty;
                }
            }
        }
        public string TypeStr2
        {
            get
            {
                if (Type == 1)
                    return "Gelen";
                if (Type == 2)
                    return "Arşiv";
                if (Type == 3)
                    return "Silinen";
                return string.Empty;
            }
        }

    }
}