using HR.Areas.HR.Service;
using HR.DTOs.HR;
using HR.Interface;
using HR.Utils.Base;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Web;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class HumanResourceController : AreaBaseController
    {
        private readonly IEntityService<HumResListInfo> _HrListInfoService;
        private readonly IEntityService<HumResEditInfo> _HrEditInfoService;
        private readonly IEntityService<Option> _OptService;

        public HumanResourceController(
            IEntityService<HumResListInfo> hrListService, IEntityService<HumResEditInfo> hrEditService,
            IEntityService<Option> optService)
        {
            _HrListInfoService = hrListService;
            _HrEditInfoService = hrEditService;
            _OptService = optService;
        }

        public IActionResult Index()
        {
            string userName = HttpContext.User.Claims.First(x => x.Type == ClaimTypes.GivenName).Value;
            TempData["userName"] = userName;
            return View();
        }

        #region 資料清單 - 取得資料
        [HttpPost]
        [Route("~/API/HumRes/InfoList")]
        public IEnumerable<HumResListInfo> GetHumResLists(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                return (_HrListInfoService as HumResListInfoService).GetDataByConditions(form);
            }
            return _HrListInfoService.GetData();
        }

        [HttpPost]
        [Route("~/API/HumRes/InfoList/Total")]
        public int GetHumResTotal(IFormCollection form)
        {
            return (_HrListInfoService as HumResListInfoService).GetDataTotal(form);
        }

        [HttpGet]
        [Route("~/API/HumRes/EditInfo/{id}")]
        public HumResEditInfo GetEditInfo(int id)
        {
            return (_HrEditInfoService as HumResEditInfoService).GetDataById(id);
        }

        [HttpGet]
        [Route("~/API/HumRes/Relations/{id}")]
        public IEnumerable<int> GetRelations(int id)
        {
            return (_HrEditInfoService as HumResEditInfoService).GetRelations(id);
        }

        [HttpGet]
        [Route("~/API/HumRes/Attachments/{id}")]
        public IEnumerable<HumResAttachment> GetAttachments(int id)
        {
            return (_HrListInfoService as HumResListInfoService).GetAttachmentsByHumanId(id);
        }
        #endregion

        #region 資料清單 - 新增資料
        [HttpPost]
        [Route("~/API/HumRes/Upload/Attachments/{humId}")]
        public IEnumerable<HumResAttachment> UploadAttachments(int humId, IFormFile file)
        {
            if (file.Length > 0)
            {
                byte[] fileData;
                using Stream stream = file.OpenReadStream();
                using MemoryStream ms = new();
                stream.CopyTo(ms);
                fileData = ms.ToArray();

                (_HrListInfoService as HumResListInfoService)?.AddAttachment(humId, fileData, file);
            }

            return GetAttachments(humId);
        }
        #endregion

        #region 資料清單 - 更新資料
        [HttpPatch]
        [Route("~/API/HumRes/Update/HumResInfo/{id}/{blocked}")]
        public HumResListInfo BlockHuman(int id, bool blocked)
        {
            return _HrListInfoService.UpdateData(new() { ID = id, IsBlocked = blocked });
        }
        #endregion

        #region 資料清單 - 刪除資料
        [HttpDelete]
        [Route("~/API/HumRes/Delete/Info/{id}")]
        public HumResListInfo DeleteHumResInfo(int id)
        {
            return _HrListInfoService.DeleteData(id);
        }

        [HttpDelete]
        [Route("~/API/HumRes/Delete/Attachment/{id}")]
        public HumResAttachment DeleteHumResAttachment(int id)
        {
            return (_HrListInfoService as HumResListInfoService).DeleteAttachment(id);
        }
        #endregion

        #region 資料清單 - 下載檔案
        [HttpGet]
        [Route("~/API/HumRes/Download/Attachment/{id}")]
        public IActionResult DownloadAttachment(int id)
        {
            Models.Entity.HR_HUMATTTACH attachment = (_HrListInfoService as HumResListInfoService).GetAttachmentById(id);
            return File(attachment.HAC_ACH_FIL, attachment.HAC_ACH_CNT_TYP, $"{attachment.HAC_ACH_NAM}.{attachment.HAC_ACH_EXT}");
        }

        [HttpGet]
        [Route("~/API/HumRes/Pdf/Attachment/{id}/{filename}")]
        public IActionResult PdfAttachment(int id)
        {
            Models.Entity.HR_HUMATTTACH attachment = (_HrListInfoService as HumResListInfoService).GetAttachmentById(id);
            MemoryStream ms = new(attachment.HAC_ACH_FIL);
            return new FileStreamResult(ms, attachment.HAC_ACH_CNT_TYP);
        }
        #endregion

        #region 人力資料 - 取得資料
        [HttpGet]
        [Route("~/API/HumRes/Options/{tid}")]
        public IEnumerable<Option> GetReasonOptions(int tid)
        {
            return (_OptService as OptionService).GetOptionsByTagSn(tid);
        }

        [HttpGet]
        [Route("~/API/HumRes/CheckIfExisted/{name}/{tel}")]
        public bool IsExistedData(string name, string tel)
        {
            return ((HumResEditInfoService)_HrEditInfoService).GetDataByNameAndTel(name, tel.Replace("-", "").Trim()).Any();
        }
        #endregion

        #region 人力資料 - 新增資料
        [HttpPost]
        [Route("~/API/HumRes/Add/HumResInfo")]
        public HumResListInfo AddHumanResource(HumResEditInfo editInfo)
        {
            HumResEditInfo addedResult = _HrEditInfoService.AddData(editInfo);
            return (_HrListInfoService as HumResListInfoService).GetAllData().Where(x => x.ID == addedResult.ID).First();
        }

        [HttpPost]
        [Route("~/API/HumRes/Add/StatusChange")]
        public HumResStatusChg AddChangeStatusHis(HumResStatusChg statusChg)
        {
            return (_HrEditInfoService as HumResEditInfoService).AddChangeStatusHis(statusChg);
        }
        #endregion

        #region 人力資料 - 更新資料
        [HttpPatch]
        [Route("~/API/HumRes/Update/HumResInfo")]
        public HumResEditInfo UpdateHumanResource(HumResEditInfo editInfo)
        {
            HumResEditInfo updatedResult = _HrEditInfoService.UpdateData(editInfo);
            return updatedResult;
        }
        #endregion

        #region 人力資料 - 刪除資料
        [HttpDelete]
        [Route("~/API/HumRes/Delete/Interview/{id}")]
        public HumItvw DeleteInterview(int id)
        {
            return (_HrEditInfoService as HumResEditInfoService).DeleteInterviewHis(id);
        }
        #endregion
    }
}
