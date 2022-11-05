using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class MessageController : AdminBaseController
    {
        // GET: Admin/Message
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetMessageList(int type)
        {
            try
            {
                List<Message> list = new List<Message>();
                list = MessageDAL.GetAllMessage().Where(w => w.Type == type && w.Deleted == false).OrderByDescending(o => o.CreateDate).ToList();
                foreach (var item in list)
                {
                    item.DateStr = GetTime(item.CreateDate);
                }
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetMessageList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string GetMessageDetail(int id)
        {
            try
            {
                Message message = MessageDAL.GetAllMessage().FirstOrDefault(w => w.Id == id);
                message.DateStr = GetTime(message.CreateDate);
                MessageDAL.UpdateMessage(message.Id, message.Type, message.Deleted, true);
                return JsonConvert.SerializeObject(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetMessageDetail", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateMessageType(List<Message> messageList, int type)
        {
            MessageBox message;
            bool result = false;
            try
            {
                foreach (var item in messageList)
                {
                    result = MessageDAL.UpdateMessage(item.Id, type, item.Deleted, item.IsRead);//arsiv or gelen
                }
                message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UpdateMessageType", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult UpdateMessageRead(List<Message> messageList)
        {
            MessageBox message;
            bool result = false;
            try
            {
                foreach (var item in messageList)
                {
                    result = MessageDAL.UpdateMessage(item.Id, item.Type, item.Deleted, true);//delete
                }
                message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UpdateMessageType", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult DeleteMessage(List<Message> messageList)
        {
            MessageBox message;
            bool result = false;
            try
            {
                foreach (var item in messageList)
                {
                    result = MessageDAL.UpdateMessage(item.Id, item.Type, true, item.IsRead);//delete
                }
                message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UpdateMessageType", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetMessageCount()
        {
            try
            {
                List<Message> list = new List<Message>();
                list = MessageDAL.GetAllMessage().Where(w => w.Deleted == false).OrderByDescending(o => o.CreateDate).ToList();

                return Json(
                    new
                    {
                        Inbox = list.Where(w => w.Type == 1).Count(),
                        InboxUnRead = list.Where(w => w.Type == 1 && !w.IsRead).Count(),
                        Archive = list.Where(w => w.Type == 2).Count(),
                        ArchiveUnRead = list.Where(w => w.Type == 2 && !w.IsRead).Count(),
                        Delete = list.Where(w => w.Type == 3).Count(),
                        DeleteUnRead = list.Where(w => w.Type == 3 && !w.IsRead).Count(),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = Int32.MaxValue,
                        RecursionLimit = Int32.MaxValue
                    }
                );
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetMessageList", ex, GetUserIpAddress());
                throw;
            }
        }
    }
}