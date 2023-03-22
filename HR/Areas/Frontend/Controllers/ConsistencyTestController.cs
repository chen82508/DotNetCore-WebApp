using HR.Areas.Frontend.Service;
using HR.DTOs.Frontend;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using HR.Utils.Mail;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace HR.Areas.Frontend.Controllers
{
    [Area("Frontend")]
    public class ConsistencyTestController : FrontendBasesController
    {
        private readonly IConfiguration _Configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly IEntityService<ConsistencyTest> _ConsistencyTestService;
        private readonly IEntityService<ResumeForm> _ResumeFormService;
        private readonly string? StartStatus;
        private readonly string? UserGuid;

        public ConsistencyTestController(
            IConfiguration configuration,
            IHttpContextAccessor httpContextAccessor,
            IEntityService<ConsistencyTest> consistencyTestService,
            IEntityService<ResumeForm> resumeFormService)
        {
            _Configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _ConsistencyTestService = consistencyTestService;

            StartStatus = _HttpContextAccessor.HttpContext?.Request.Cookies["StartTest"];
            UserGuid = _HttpContextAccessor.HttpContext?.Request.Cookies["UserGuid"];
            _ResumeFormService = resumeFormService;
        }

        public IActionResult Consent()
        {
            if (string.IsNullOrEmpty(UserGuid))
            {
                return Redirect("/Frontend/Profile/Consent");
            }

            // 進入測驗說明頁都先檢查是否存在Cookie
            if (!string.IsNullOrEmpty(StartStatus))
            {
                _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("StartTest");
            }

            return View();
        }

        public IActionResult Test()
        {
            if (string.IsNullOrEmpty(StartStatus) || StartStatus != "Y")
            {
                return View("Consent");
            }

            return View();
        }

        public IActionResult Finish()
        {
            return View();
        }

        [HttpPost]
        [Route("~/API/ConsistencyTest/Start")]
        public IActionResult Start()
        {
            if (string.IsNullOrEmpty(StartStatus) || StartStatus == "Y")
            {
                CookieOptions options = new()
                {
                    Expires = DateTime.Now.AddMinutes(15),
                    //SameSite = SameSiteMode.Lax,
                    //Secure = true,
                    HttpOnly = true,
                };
                _HttpContextAccessor.HttpContext?.Response.Cookies.Append("StartTest", "Y", options);

                return Redirect("/Frontend/ConsistencyTest/Test");
            }
            return View("Consent");
        }

        [HttpPost]
        [Route("~/API/ConsistencyTest/ReceiveTestData")]
        public IActionResult ReceiveTestData(string testContent)
        {
            try
            {
                JToken? TestData = JsonConvert.DeserializeObject<JToken>(testContent);

                JToken? Choosen = TestData?["choosen"];
                JToken? Sequence = TestData?["sequence"];
                string ExcludeJson = JsonConvert.SerializeObject(Choosen?.Where(x => !x.Value<bool>("selected")));
                int leftSeconds = Convert.ToInt32(TestData?["left_seconds"]);

                if (!string.IsNullOrEmpty(UserGuid))
                {
                    _ConsistencyTestService.AddData(new ConsistencyTest
                    {
                        Id = UserGuid,
                        Choosen = JsonConvert.SerializeObject(Choosen),
                        Exculde = ExcludeJson,
                        Sequence = JsonConvert.SerializeObject(Sequence),
                        Seconds = 900 - leftSeconds
                    });

                    (_ResumeFormService as ResumeFormService)?.UpdateTempData(UserGuid);

                    string MailHost = _Configuration.GetSection("Smtp").GetValue<string>("Host");
                    int MailPort = _Configuration.GetSection("Smtp").GetValue<int>("Port");
                    string MailUserName = _Configuration.GetSection("Smtp").GetValue<string>("Username");
                    string MailPassword = _Configuration.GetSection("Smtp").GetValue<string>("Password");
                    string MailFromAddress = _Configuration.GetSection("Smtp").GetValue<string>("FromAddress");
                    string[] MailToAddresses = _Configuration.GetSection("Smtp").GetValue<string>("ToAddress").Split(";", StringSplitOptions.RemoveEmptyEntries);
                    MailServe serve = new(MailHost, MailPort, MailUserName, MailPassword);
                    serve.SetFromAddress(MailFromAddress);
                    serve.SetFromName("人力資源系統通知");
                    serve.SetToAddresses(MailToAddresses);
                    serve.SetSubject("履歷進件通知信");
                    serve.SetContent("有一筆新的履歷進件<br><br>此為系統通知，請勿回覆。");

                    serve.SendMail();
                }
                
                return Ok(new { goTo = "/Frontend/ConsistencyTest/Finish" });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                #region 完整結束後，移除關鍵性的Cookie紀錄
                if (!string.IsNullOrEmpty(StartStatus))
                {
                    _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("StartTest");
                }

                if (!string.IsNullOrEmpty(UserGuid))
                {
                    _HttpContextAccessor.HttpContext?.Response.Cookies.Delete("UserGuid");
                }
                #endregion
            }
        }
    }
}
