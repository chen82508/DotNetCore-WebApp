using HR.Areas.Frontend.Service;
using HR.Areas.HR.Service;
using HR.DTOs.Frontend;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;

namespace HR.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class ProfileController : FrontendBasesController
    {
        private readonly IConfiguration _Configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEntityService<ResumeForm> _ResumeFormService;
        private readonly IEntityService<JobInfo> _JobInfoService;
        private readonly IEntityService<ResumeAttachment> _ResumeAttachService;

        private readonly string? AgreeStatus;
        private readonly string? UserGuid;

        private readonly CookieOptions _Options;

        public ProfileController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEntityService<ResumeForm> resumeFormService,
            IEntityService<ResumeAttachment> resumeAttachService,
            IEntityService<JobInfo> jobInfoService)
        {
            _Configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _ResumeFormService = resumeFormService;
            _ResumeAttachService = resumeAttachService;
            _JobInfoService = jobInfoService;

            AgreeStatus = _HttpContextAccessor.HttpContext?.Request.Cookies["Agree"];
            UserGuid = _HttpContextAccessor.HttpContext?.Request.Cookies["UserGuid"];

            _Options = new()
            {
                Expires = DateTime.Now.AddMinutes(_Configuration.GetValue<int>("FrontendFormCookieExpire")),
                //SameSite = SameSiteMode.Lax,
                //Secure = true,
                HttpOnly = true,
            };
        }

        public IActionResult Consent()
        {
            #region 進入履歷表單前都先檢查是否存在Cookie
            if (!string.IsNullOrEmpty(AgreeStatus))
            {
                _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("Agree");
            }

            if (!string.IsNullOrEmpty(UserGuid))
            {
                _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("UserGuid");
            }
            #endregion

            return View();
        }

        public IActionResult FillIn()
        {
            if (string.IsNullOrEmpty(AgreeStatus) || AgreeStatus != "on")
            {
                return View("Consent");
            }

            if (string.IsNullOrEmpty(UserGuid))
            {
                HttpContext.Response.Cookies.Append("UserGuid", Guid.NewGuid().ToString(), _Options);
            }

            return View();
        }

        [HttpPost]
        [Route("~/API/Profile/Agree")]
        public IActionResult Agree(IFormCollection form)
        {
            if ($"{form["consent-agree"]}" != "on")
            {
                return View("Consent");
            }

            HttpContext.Response.Cookies.Append("Agree", $"{form["consent-agree"]}", _Options);

            return Redirect("/Frontend/Profile/FillIn");
        }

        [HttpPost]
        [Route("~/API/Profile/SubmitForm")]
        public IActionResult SubmitForm(IFormFile photo, IFormFileCollection attachments, string resume)
        {
            ResumeForm resumeForm = new() { Id = UserGuid };

            if (!string.IsNullOrEmpty(resume))
            {
                JToken? jToken = JsonConvert.DeserializeObject<JToken>(resume);

                JToken? jBasicInfo = jToken?["basicInfo"];
                JToken? jExperiences = jToken?["experiences"];
                JToken? jStatus = jToken?["status"];
                JToken? jQA = jToken?["QA"];

                resumeForm.BaseInfo = JsonConvert.SerializeObject(jBasicInfo);
                resumeForm.Experiences = JsonConvert.SerializeObject(jExperiences);
                resumeForm.Status = JsonConvert.SerializeObject(jStatus);
                resumeForm.QA = JsonConvert.SerializeObject(jQA);
            }

            if (photo != null)
            {
                byte[] photoData;
                using Stream stream = photo.OpenReadStream();
                using MemoryStream ms = new();
                stream.CopyTo(ms);
                photoData = ms.ToArray();
                resumeForm.Photo = photoData;
                resumeForm.ContentType = photo.ContentType;
            }

            _ResumeFormService.AddData(resumeForm);

            if (attachments != null || attachments?.Count > 0)
            {
                List<ResumeAttachment> resumeAttachments = new();

                byte[] attachData;
                int iAttachSeq = 1;
                foreach (IFormFile attach in attachments)
                {
                    using Stream stream = attach.OpenReadStream();
                    using MemoryStream ms = new();
                    stream.CopyTo(ms);
                    attachData = ms.ToArray();

                    resumeAttachments.Add(new()
                    {
                        Id = UserGuid,
                        Seq = iAttachSeq++,
                        File = attachData,
                        Name = Path.GetFileNameWithoutExtension(attach.FileName),
                        Extension = Path.GetExtension(attach.FileName).ToLower().Replace(".", ""),
                        contentType = attach.ContentType
                    });
                }

                (_ResumeAttachService as ResumeAttachmentService)?.AddMultipleData(resumeAttachments);
            }

            if (!string.IsNullOrEmpty(AgreeStatus))
            {
                _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("Agree");
            }

            // 重新紀錄Guid
            HttpContext.Response.Cookies.Append("UserGuid", UserGuid, _Options);

            return Ok(new { goTo = "/Frontend/ConsistencyTest/Consent" });
        }

        [HttpGet]
        [Route("~/FrontAPI/Job/JobInfos")]
        public IEnumerable<JobInfo> GetJobInfos()
        {
            return (_JobInfoService as JobInfoService).GetAllData().Where(x => x.IsOpen);
        }
    }
}
