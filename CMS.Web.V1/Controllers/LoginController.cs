using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS.Web.V1.Controllers
{
    public class LoginController : BaseController
    {
        List<User> _userList = UserDAL.GetUserList().Where(w => w.Status == true).ToList();
        // GET: Login
        public ActionResult Index()
        {
            //beni hatirla onceden isaretlenmisse
            if (Request.Cookies["rememberCookie"] != null)
            {
                HttpCookie userRememberCookie = Request.Cookies["rememberCookie"];
                AuthorityUser = _userList.FirstOrDefault(w => w.Mail == userRememberCookie.Values["UserMail"] && w.Password == userRememberCookie.Values["UserPassword"]);
                FormsAuthentication.SetAuthCookie(AuthorityUser.Mail, false);
                Exception logMessage = new Exception(message: AuthorityUser.Name + " kullanıcısı çerez kayıtları ile sisteme tekrar giriş yaptı.");
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Information, ClientType.Admin, "Login by Cookie", logMessage, GetUserIpAddress());

                return RedirectToAction("Index", "Home", new { Area = "Admin" });
            }
            else
            {
                User user = new User();
                return View(user);
            }
        }

        [HttpPost]
        public ActionResult Index(User user, string remember)
        {
            try
            {
                AuthorityUser = _userList.FirstOrDefault(w => w.Mail == user.Mail && w.Password == user.Password);

                if (AuthorityUser != null)
                {
                    if (remember == "on")
                    {
                        HttpCookie userRememberCookie = new HttpCookie("rememberCookie");
                        userRememberCookie.Values.Add("UserMail", AuthorityUser.Mail);
                        userRememberCookie.Values.Add("UserPassword", AuthorityUser.Password);

                        userRememberCookie.Expires = DateTime.Now.AddDays(30);//remember 30day
                        Response.Cookies.Add(userRememberCookie);
                    }

                    FormsAuthentication.SetAuthCookie(AuthorityUser.Mail, false);

                    Exception logMessage = new Exception(message: AuthorityUser.Name + " kullanıcısı sisteme giriş yaptı.");
                    LogGeneralErrors.LogGeneral(LogGeneralErrorType.Information, ClientType.Admin, "Login", logMessage, GetUserIpAddress());

                    return RedirectToAction("Index", "Home", new { Area = "Admin" });
                }
                else
                {
                    ModelState.AddModelError("", "Giriş bilgileri hatalı!");
                    ModelState.AddModelError("", "Lütfen bilgilerinizi kontrol edip tekrar deneyin.");
                    return View(user);
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, GetControllerName(), ex, GetUserIpAddress());
                throw;
            }

        }
        public ActionResult LogOut()
        {
            try
            {
                Exception logMessage = new Exception(message: AuthorityUser.Name + " kullanıcısı sistemden çıkış yaptı.");
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Information, ClientType.Admin, "Logout", logMessage, GetUserIpAddress());

                Session["AuthorityUser"] = null;
                Session.Abandon();//tüm sessionları sonlandır

                if (Request.Cookies["rememberCookie"] != null)
                {
                    Response.Cookies["rememberCookie"].Expires = DateTime.Now.AddDays(-1);
                }
                FormsAuthentication.SignOut();

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, GetControllerName(), ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult ResetPassword(string email)
        {
            MessageBox message;
            try
            {
                bool result = false;

                AuthorityUser = _userList.FirstOrDefault(w => w.Mail == email);
                if (AuthorityUser != null)
                {
                    Guid randomkey = Guid.NewGuid();//32 karakterli randomkodu üret
                    var newPassword = randomkey.ToString().Substring(0, 6);
                    AuthorityUser.Password = newPassword.ToUpper();
                    result = UserDAL.UpdateUser(AuthorityUser.Id,AuthorityUser.Name,AuthorityUser.Mail, AuthorityUser.Password, AuthorityUser.Role,AuthorityUser.Status,true);

                    if (result)
                    {
                        Mail mail = MailSettingDAL.GetMailSetting();
                        mail.SubJect = "CMS - ŞİFRE YENİLEME.";
                        mail.Body = "Merhaba <b>" + AuthorityUser.Name + "</b>,<br><br> Şifre Yenileme Talebiniz Alınmıştır." +
                            "<br> Panel Giriş kodunuz: <b>" + newPassword + "</b> <br>" +
                            "<br>Aşağıdaki linke tıklayarak yukarıdaki kod ile yönetim panelinize giriş yapabilir ve şifrenizi güncelleyebilirsiniz." +
                            "</br><br><a style='text-decoration:none;' href='"+GlobalSettings.WebAddress+"login' target='_blank'>Yönetim paneline gitmek için tıklayınız..</a>";
                        mail.To = AuthorityUser.Mail;
                        //mail.Attachments = new Attachment(@"C:\Users\he\Desktop\test.pdf");
                        result = mail.MailSender();
                    }
                }
                message = result ? new MessageBox(MessageBoxType.Success, "Talebiniz üzerine şifre yenileme bağlantısı tanımlı eposta adresinize gönderilmiştir. Şifrenizi yenilemek için tarafınıza gönderilen maildeki adımları izleyin.") : new MessageBox(MessageBoxType.Error, "Bu e-posta adresine tanımlı kullanıcı bulunamadı.");
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, GetControllerName(), ex, GetUserIpAddress());
                message = new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            }
            return Json(message);
        }
    }
}