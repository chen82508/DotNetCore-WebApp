using HR.Areas.Frontend.Service;
using HR.Areas.HR.Service;
using HR.DTOs.Frontend;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using HR.Utils.Export;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mail;
using static System.Net.Mime.MediaTypeNames;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class FrontendResumeController : AreaBaseController
    {
        private readonly IEntityService<ResumeForm> _ResumeFormServ;
        private readonly IEntityService<ConsistencyTest> _ConsistencyTestServ;
        private readonly IEntityService<ResumeAttachment> _ResumeAttachServ;

        private readonly IExport _Export;

        public FrontendResumeController(
            IEntityService<ResumeForm> resumeFormServ,
            IEntityService<ConsistencyTest> consistencyTestServ,
            IEntityService<ResumeAttachment> resumeAttachServ,
            IExport export)
        {
            _ResumeFormServ = resumeFormServ;
            _ConsistencyTestServ = consistencyTestServ;
            _ResumeAttachServ = resumeAttachServ;
            _Export = export;
        }

        public IActionResult Index()
        {
            (_ResumeFormServ as ResumeFormService)?.DeleteTempData();

            return View();
        }

        #region 取得資料
        [HttpPost]
        [Route("~/API/Resume/ResumeList")]
        public IEnumerable<ResumeForm> GetResumeList(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                return (_ResumeFormServ as ResumeFormService).GetDataByConditions(form);
            }
            return _ResumeFormServ.GetData();
        }

        [HttpGet]
        [Route("~/API/Resume/FrontendResume")]
        public IEnumerable<ResumeForm> GetFrontendResume()
        {
            return _ResumeFormServ.GetData();
        }

        [HttpPost]
        [Route("~/API/Resume/FrontendResume/Total")]
        public int GetResumeTotal(IFormCollection form)
        {
            return (_ResumeFormServ as ResumeFormService).GetDataTotal(form);
        }

        [HttpGet]
        [Route("~/API/Resume/Photo/{id}")]
        public IActionResult GetResumePhoto(string id)
        {
            KeyValuePair<byte[], string> photo = (_ResumeFormServ as ResumeFormService).GetPhoto(id);

            return Ok(new
            {
                src = Convert.ToBase64String(photo.Key),
                contentType = photo.Value,
            });
        }

        [HttpGet]
        [Route("~/API/Resume/Test/{id}")]
        public IActionResult GetResumeTest(string id)
        {
            ConsistencyTest test = (_ConsistencyTestServ as ConsistencyTestService).GetData(id);

            return Ok(test);
        }

        [HttpGet]
        [Route("~/API/Resume/Attachment/{id}")]
        public IEnumerable<ResumeAttachment> GetResumeAttachments(string id)
        {
            return (_ResumeAttachServ as ResumeAttachmentService).GetDataById(id);
        }
        #endregion

        [HttpGet]
        [Route("~/API/Resume/Download/{id}/{seq}")]
        public IActionResult PdfAttachment(string id, int seq)
        {
            ResumeAttachment attachment = (_ResumeAttachServ as ResumeAttachmentService).GetDataByPK(id, seq);
            using MemoryStream ms = new(attachment.File);
            return new FileStreamResult(ms, attachment.contentType);
        }

        [HttpGet]
        [Route("~/API/Resume/Print/{id}")]
        public IActionResult PrintResume(string id)
        {
            string path = Directory.GetCurrentDirectory();
            byte[] templateFile = System.IO.File.ReadAllBytes($"{path}/Utils/Assets/WordTemplate/ResumeTemplate.docx");
            Dictionary<string, string> MainData = _Export.GetDataDictionary(id);
            Dictionary<string, string> ExpData = (_Export as ExportService).GetExperienceDictionary(id);
            return File(WordRender.GenerateDocx(templateFile, MainData, ExpData), "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "Resume.docx");
        }

        [HttpDelete]
        [Route("~/API/Resume/Delete/{id}")]
        public IActionResult DeleteResume(string id)
        {
            (_ResumeAttachServ as ResumeAttachmentService)?.DeleteData(id);
            (_ConsistencyTestServ as ConsistencyTestService)?.DeleteTestData(id);
            (_ResumeFormServ as ResumeFormService)?.DeleteData(id);

            return Ok();
        }
    }
}
