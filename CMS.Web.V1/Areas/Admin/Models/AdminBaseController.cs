using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Models.Helper
{
    [Authorize(Roles = "Administrator,Manager")]
    public class AdminBaseController : BaseController
    {
      
    }

    //Message Alert
    public enum MessageBoxType
    {
        Success,
        Error,
        Warning,
        Info
    }
    public class MessageBox
    {
        public string Statu { get; set; }
        public string Message { get; set; }
        public string Color { get; set; }
        public string Icon { get; set; }
        public int ResultId { get; set; }
        public MessageBox(MessageBoxType statu, string message, int resultId = -1)
        {
            switch (statu)
            {
                case MessageBoxType.Error:
                    Statu = "error";
                    Color = "error";
                    Icon = "fa fa-ban";
                    break;
                case MessageBoxType.Info:
                    Statu = "info";
                    Color = "info";
                    Icon = "fa fa-info-circle";
                    break;
                case MessageBoxType.Success:
                    Statu = "success";
                    Color = "success";
                    Icon = "fa fa-check";
                    break;
                case MessageBoxType.Warning:
                    Statu = "warning";
                    Color = "warning";
                    Icon = "fa fa-exclamation-triangle";
                    break;
            }
            Message = message;
            ResultId = resultId;
        }
    }

}