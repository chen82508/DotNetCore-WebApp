using HR.Areas.HR.Service;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Utils.Base;
using Microsoft.AspNetCore.Mvc;

namespace HR.Areas.HR.Controllers
{
    [Area("HR")]
    public class JobController : AreaBaseController
    {
        private readonly IEntityService<JobType> _JobTypeService;
        private readonly IEntityService<JobTypeTag> _JobTypeTagService;
        private readonly IEntityService<JobInfo> _JobInfoService;

        public JobController(
            IEntityService<JobType> jobTypeService, IEntityService<JobTypeTag> jobTypeTagService,
            IEntityService<JobInfo> jobInfoService)
        {
            _JobTypeService = jobTypeService;
            _JobTypeTagService = jobTypeTagService;
            _JobInfoService = jobInfoService;
        }

        public IActionResult JobType()
        {
            return View();
        }

        public IActionResult Info()
        {
            return View();
        }

        #region 職缺類別 - 取得資料
        [HttpPost]
        [Route("~/API/Job/Types")]
        public IEnumerable<JobType> GetJobTypes()
        {
            return _JobTypeService.GetData();
        }

        [HttpPost]
        [Route("~/API/Job/TypeTags")]
        public IEnumerable<JobTypeTag> GetJobTypeTags()
        {
            return _JobTypeTagService.GetData();
        }
        #endregion

        #region 職缺類別 - 新增資料
        [HttpPost]
        [Route("~/API/Job/Add/Type")]
        public JobType AddType(JobType jobTyp)
        {
            return _JobTypeService.AddData(jobTyp);
        }

        [HttpPost]
        [Route("~/API/Job/Add/TypeTag")]
        public JobTypeTag AddTypeTag(JobTypeTag jobTypTag)
        {
            return _JobTypeTagService.AddData(jobTypTag);
        }
        #endregion

        #region 職缺類別 - 更新資料
        [HttpPatch]
        [Route("~/API/Job/Update/Type")]
        public JobType UpdateType(JobType jobType)
        {
            return _JobTypeService.UpdateData(jobType);
        }

        [HttpPatch]
        [Route("~/API/Job/Update/TypeTag")]
        public JobTypeTag UpdateTypeTag(JobTypeTag jobTypeTag)
        {
            return _JobTypeTagService.UpdateData(jobTypeTag);
        }
        #endregion

        #region 職缺類別 - 刪除資料
        [HttpDelete]
        [Route("~/API/Job/Delete/Type/{id}")]
        public JobType DeleteType(int id)
        {
            return _JobTypeService.DeleteData(id);
        }

        [HttpDelete]
        [Route("~/API/Job/Delete/TypeTag/{id}")]
        public JobTypeTag DeteteTypeTag(int id)
        {
            return _JobTypeTagService.DeleteData(id);
        }
        #endregion

        #region 職缺資訊 - 取得資料
        [HttpPost]
        [Route("~/API/Job/JobInfos/Total")]
        public int GetJobInfoTotal(IFormCollection form)
        {
            return (_JobInfoService as JobInfoService).GetDataTotal(form);
        }

        [HttpPost]
        [Route("~/API/Job/JobInfos")]
        public IEnumerable<JobInfo> GetJobInfos(IFormCollection form)
        {
            if (form.Keys.Count > 0)
            {
                return (_JobInfoService as JobInfoService).GetDataByConditions(form);
            }
            return _JobInfoService.GetData();
        }

        [HttpPost]
        [Route("~/API/Job/JobInfos/All")]
        public IEnumerable<JobInfo> GetAllJobInfos()
        {
            return (_JobInfoService as JobInfoService).GetAllData();
        }

        [HttpPost]
        [Route("~/API/Job/JobInfos/Active")]
        public IEnumerable<JobInfo> GetActiveJobInfos(IFormCollection form)
        {
            return _JobInfoService.GetData().Where(x => x.IsOpen);
        }

        [HttpGet]
        [Route("~/API/Job/JobInfos/{id}")]
        public JobInfo GetJobInfo(int id)
        {
            return (_JobInfoService as JobInfoService).GetDataById(id);
        }

        [HttpGet]
        [Route("~/API/Job/Relations/{id}")]
        public IEnumerable<int> GetRelations(int id)
        {
            return (_JobInfoService as JobInfoService).GetRelations(id);
        }
        #endregion

        #region 職缺資訊 - 新增資料
        [HttpPost]
        [Route("~/API/Job/Add/JobInfo")]
        public JobInfo AddJob(JobInfo jobInfo)
        {
            return _JobInfoService.AddData(jobInfo);
        }
        #endregion

        #region 職缺資訊 - 更新資料
        [HttpPatch]
        [Route("~/API/Job/Update/JobInfo")]
        public JobInfo UpdateJobInfo(JobInfo jobInfo)
        {
            return _JobInfoService.UpdateData(jobInfo);
        }

        [HttpPatch]
        [Route("~/API/Job/Update/OpenHistory/{id}/{status}")]
        public IEnumerable<string> UpdateOpenHistory(int id, bool status)
        {
            return (_JobInfoService as JobInfoService).UpdateHistory(id, status);
        }

        [HttpPatch]
        [Route("~/API/Job/Update/CloseJob")]
        public JobInfo CloseJob(JobInfo jobInfo)
        {
            return (_JobInfoService as JobInfoService).UpdateCloseInfo(jobInfo);
        }
        #endregion

        #region 職缺資訊 - 刪除資料
        [HttpDelete]
        [Route("~/API/Job/Delete/JobInfo/{id}")]
        public JobInfo DeleteJob(int id)
        {
            return _JobInfoService.DeleteData(id);
        }
        #endregion
    }
}
