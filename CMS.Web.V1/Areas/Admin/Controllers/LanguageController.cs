using CMS.Web.V1.Models.DataAcces;
using CMS.Web.V1.Models.Entity;
using CMS.Web.V1.Models.Helper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CMS.Web.V1.Areas.Admin.Controllers
{
    public class LanguageController : AdminBaseController
    {
        // GET: Admin/Language
        #region LANGUAGE

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public string GetLanguageList()
        {
            try
            {
                return JsonConvert.SerializeObject(LanguageDAL.GetAllLanguage());
               
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetLanguageList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveLanguage(Language lang)
        {
            bool result = false;
            try
            {
                if (lang.Id > 0)
                {
                    result = LanguageDAL.UpdateLanguage(lang.Id, lang.LangName, lang.LangCode, lang.FlagCode, lang.Status);
                }
                else
                {
                    result = LanguageDAL.AddLanguage(lang.LangName, lang.LangCode, lang.FlagCode, lang.Status);
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveLanguage", ex, GetUserIpAddress());
                result = false;
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");

            return Json(message);
        }

        [HttpPost]
        public JsonResult DeleteLanguage(Language lang)
        {
            bool result = false;
            try
            {
                if (lang.Id > 0)
                {
                    result = LanguageDAL.DeleteLanguage(lang.Id);
                }
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "DeleteLanguage", ex, GetUserIpAddress());
                result = false;
            }

            var message = result ? new MessageBox(MessageBoxType.Success, "İşlem başarılı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
            return Json(message);
        }
        #endregion

        // GET: Admin/Language/resource
        #region LANGUAGE_RESOURCE

        public ActionResult Resource()
        {
            return View();
        }
        [HttpPost]
        public string GetLanguageResourceList()
        {
            try
            {
                return JsonConvert.SerializeObject(LanguageResourceDAL.GetAllLanguageResource());
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetLanguageResourceList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveResource(List<LanguageResource> resourceList)
        {
            bool result = false;
            try
            {
                var control = resourceList.Where(w => w.ResourceKey == "" || w.ResourceValue == "" || w.ResourceValue == null).ToList();
                if (control.Count() > 0)
                {
                    var error = new MessageBox(MessageBoxType.Error, "Lütfen gerekli alanları boş bırakmayın!");
                    return Json(error);
                }

                List<LanguageResource> loadResourceList = LanguageResourceDAL.GetLanguageAllResourceByKey(resourceList[0].ResourceKey);
                foreach (LanguageResource item in resourceList)
                {
                    var isResource = loadResourceList.FirstOrDefault(f => f.LangCode == item.LangCode);
                    if (isResource == null)
                    {
                        result = LanguageResourceDAL.AddLanguageResource(item.ResourceKey, item.ResourceValue, item.LangId);

                    }
                    else
                    {
                        result = LanguageResourceDAL.UpdateLanguagResourcee(isResource.ResourceId, item.ResourceKey, item.ResourceValue);
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveResource", ex, GetUserIpAddress());
            }
            var message = result ? new MessageBox(MessageBoxType.Success, "Kaynaklar tanımlandı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");


            return Json(message);
        }

        [HttpPost]
        public string GetResourceValues(string resourceKey)
        {
            try
            {
                List<LanguageResource> resourceList = LanguageResourceDAL.GetAllLanguageResource().Where(w => w.ResourceKey == resourceKey).ToList();

                var languages = LanguageDAL.GetAllLanguage();
                foreach (var lang in languages)
                {
                    var resource = resourceList.Where(w => w.ResourceKey == resourceKey && w.LangCode == lang.LangCode).FirstOrDefault();
                    if (resource == null)
                    {
                        LanguageResource newLR = new LanguageResource();
                        newLR.ResourceKey = resourceKey;
                        newLR.ResourceValue = resourceKey;
                        newLR.LangCode = lang.LangCode;
                        newLR.LangName = lang.LangName;
                        newLR.LangId = lang.Id;
                        resourceList.Add(newLR);
                    }
                }
                return JsonConvert.SerializeObject(resourceList);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetResourceValues", ex, GetUserIpAddress());
                throw;
            }
        }

        #endregion


        #region Excel_Resource
        public ActionResult ImportExcelResource()
        {
            return View();
        }

        [HttpPost]
        public string GetResourceListByLangCode()
        {
            try
            {
                List<LanguageResource> resourceList = LanguageResourceDAL.GetAllLanguageResource();
                return JsonConvert.SerializeObject(resourceList.Where(w => w.LangCode == "tr"));
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "GetResourceListByLangCode", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public string UploadResourceList(List<ExcelUpload> uploadList)
        {
            try
            {
                List<LanguageResource> resourceList = LanguageResourceDAL.GetAllLanguageResource();

                ExcelUploadResult resultList = new ExcelUploadResult();

                foreach (var uploadItem in uploadList)
                {
                    LanguageResource resource = new LanguageResource();
                    var isResource = resourceList.FirstOrDefault(f => f.LangCode == "tr" && f.ResourceKey == uploadItem.ResourceKey);

                    if (isResource != null&& !string.IsNullOrEmpty(uploadItem.ResourceValue))
                    {
                        resource.LangId = uploadItem.LangId;
                        resource.LangCode = uploadItem.LangCode;
                        resource.ResourceKey = uploadItem.ResourceKey;
                        resource.ResourceValue = uploadItem.ResourceValue;

                        resultList.SuccessList.Add(resource);
                    }
                    else
                    {

                        resource.LangId = uploadItem.LangId;
                        resource.LangCode = uploadItem.LangCode;
                        resource.ResourceKey = uploadItem.ResourceKey;
                        resource.ResourceValue = uploadItem.ResourceValue;

                        resultList.ErrorList.Add(resource);
                    }
                }
                return JsonConvert.SerializeObject(resultList);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "UploadResourceList", ex, GetUserIpAddress());
                throw;
            }
        }

        [HttpPost]
        public JsonResult SaveExcelResource(List<LanguageResource> resourceList)
        {
            try
            {

                if (resourceList == null)
                {
                    var error = new MessageBox(MessageBoxType.Error, "Lütfen güncellenecek dil çeviri dosyasını yükleyin!");
                    return Json(error);
                }

                List<LanguageResource> loadResourceList = LanguageResourceDAL.GetAllLanguageResource().Where(w => w.LangCode == resourceList[0].LangCode).ToList();
                bool result = false;
                foreach (LanguageResource item in resourceList)
                {
                    var isResource = loadResourceList.FirstOrDefault(f => f.ResourceKey == item.ResourceKey);
                    if (isResource == null)
                    {
                        result = LanguageResourceDAL.AddLanguageResource(item.ResourceKey, item.ResourceValue, item.LangId);
                    }
                    else
                    {
                        result = LanguageResourceDAL.UpdateLanguagResourcee(isResource.ResourceId, item.ResourceKey, item.ResourceValue);
                    }
                }
                var message = result ? new MessageBox(MessageBoxType.Success, "Kaynaklar tanımlandı.") : new MessageBox(MessageBoxType.Error, "İşlem sırasında bir hata oluştu!");
                return Json(message);
            }
            catch (Exception ex)
            {
                LogGeneralErrors.LogGeneral(LogGeneralErrorType.Error, ClientType.Admin, "SaveExcelResource", ex, GetUserIpAddress());
                throw;
            }
        }

        public class ExcelUpload
        {
            public int LangId { get; set; }
            public string LangCode { get; set; }
            public string ResourceKey { get; set; }
            public string ResourceValue { get; set; }
        }

        public class ExcelUploadResult
        {
            public ExcelUploadResult()
            {
                SuccessList = new List<LanguageResource>();
                ErrorList = new List<LanguageResource>();
            }
            public List<LanguageResource> SuccessList { get; set; }
            public List<LanguageResource> ErrorList { get; set; }
        }

        #endregion
    }


}