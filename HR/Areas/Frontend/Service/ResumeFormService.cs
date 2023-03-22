using DocumentFormat.OpenXml.Office2010.Excel;
using HR.Database;
using HR.DTOs.Frontend;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;
using HR.Utils.Enumerate;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HR.Areas.Frontend.Service
{
    public class ResumeFormService : IEntityService<ResumeForm>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<FE_RESUME> _FeResume;
        private readonly IRepository<FE_RESUME_ATTACHMENT> _FeResumeAttach;
        private readonly IRepository<FE_CONSISTENCY_TEST> _FeConsistencyTest;

        public ResumeFormService(HRSysContext hRContext)
        {
            _HRContext = hRContext;
            _FeResume = new HRGenericRepository<FE_RESUME>(_HRContext);
            _FeResumeAttach = new HRGenericRepository<FE_RESUME_ATTACHMENT>(_HRContext);
            _FeConsistencyTest = new HRGenericRepository<FE_CONSISTENCY_TEST>(_HRContext);
        }

        public ResumeForm AddData(ResumeForm entity)
        {
            FE_RESUME fE_RESUME = new()
            {
                FR_RSM_ID = entity.Id,
                FR_RSM_ADD_DAT = DateTime.Now,
                FR_RSM_PHOTO = entity.Photo,
                FR_RSM_PHOTO_TYP = entity.ContentType,
                FR_RSM_BCI = entity.BaseInfo,
                FR_RSM_EXP = entity.Experiences,
                FR_RSM_STATUS = entity.Status,
                FR_RSM_QA = entity.QA,
                FR_ISTEMP = true
            };

            _FeResume.Insert(fE_RESUME);
            _HRContext.SaveChanges();

            return entity;
        }

        public ResumeForm DeleteData(int dataId)
        {
            return new();
        }

        /// <summary>
        /// 刪除暫存的資料
        /// </summary>
        public void DeleteTempData()
        {
            IEnumerable<FE_RESUME> fE_RESUMEs = _FeResume.GetDataByCondition(x => x.FR_ISTEMP);
            if (fE_RESUMEs.Any())
            {
                IEnumerable<string> ResumeIds = fE_RESUMEs.Select(x => x.FR_RSM_ID);

                IEnumerable<FE_CONSISTENCY_TEST> cONSISTENCY_TESTs = _FeConsistencyTest.GetDataByCondition(x => ResumeIds.Contains(x.FCT_CST_RSM_ID));
                if (cONSISTENCY_TESTs.Any())
                {
                    _FeConsistencyTest.RemoveRange(cONSISTENCY_TESTs);
                }

                IEnumerable<FE_RESUME_ATTACHMENT> rESUME_ATTACHMENTs = _FeResumeAttach.GetDataByCondition(x => ResumeIds.Contains(x.FRA_ACH_RSM_ID));
                if (rESUME_ATTACHMENTs.Any())
                {
                    _FeResumeAttach.RemoveRange(rESUME_ATTACHMENTs);
                }

                _FeResume.RemoveRange(fE_RESUMEs);
                _HRContext.SaveChanges();
            }
        }

        public void DeleteData(string dataId)
        {
            FE_RESUME? resume = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == dataId).FirstOrDefault();
            if (resume != null)
            {
                _FeResume.Remove(resume);
                _HRContext.SaveChanges();
            }
        }

        #region 取得資料
        public IEnumerable<ResumeForm> GetData()
        {
            return _FeResume.GetDataByCondition(x => !x.FR_ISTEMP).OrderByDescending(x => x.FR_RSM_ADD_DAT).Select(x => new ResumeForm
            {
                Id = x.FR_RSM_ID,
                BaseInfo = x.FR_RSM_BCI,
                Experiences = x.FR_RSM_EXP,
                Status = x.FR_RSM_STATUS,
                QA = x.FR_RSM_QA,
                Date = $"{x.FR_RSM_ADD_DAT:yyyy/MM/dd}",
            });
        }

        /// <summary>
        /// 取得相片
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public KeyValuePair<byte[], string> GetPhoto(string id)
        {
            byte[] data = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == id).Select(x => x.FR_RSM_PHOTO).First();
            string type = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == id).Select(x => x.FR_RSM_PHOTO_TYP).First();
            if (string.IsNullOrEmpty(type))
            {
                type = "image/png";
            }

            return new KeyValuePair<byte[], string>(data, type);
        }

        /// <summary>
        /// 依條件取得資料
        /// </summary>
        /// <param name="form">傳入的條件</param>
        /// <returns></returns>
        public IEnumerable<ResumeForm> GetDataByConditions(IFormCollection form)
        {
            IEnumerable<FE_RESUME> qryMsts = FilterByForm(form);

            int currPageIdx = form.Keys.Contains("pagination[current]") ? Convert.ToInt32(form["pagination[current]"]) : 1;
            int pageSize = form.Keys.Contains("pagination[size]") ? Convert.ToInt32(form["pagination[size]"]) : 10;
            List<FE_RESUME> feRsms = qryMsts.OrderByDescending(rsm => rsm.FR_RSM_ADD_DAT).Skip((currPageIdx - 1) * pageSize).Take(pageSize).ToList();

            return feRsms.Select(rsm => new ResumeForm
            {
                Id = rsm.FR_RSM_ID,
                BaseInfo = rsm.FR_RSM_BCI,
                Experiences = rsm.FR_RSM_EXP,
                Status = rsm.FR_RSM_STATUS,
                QA = rsm.FR_RSM_QA,
                Date = rsm.FR_RSM_ADD_DAT.ToString("yyyy/MM/dd")
            });
        }

        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <returns></returns>
        public int GetDataTotal(IFormCollection form)
        {
            return FilterByForm(form).Count();
        }

        /// <summary>
        /// 過濾條件
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        private IEnumerable<FE_RESUME> FilterByForm(IFormCollection form)
        {
            IEnumerable<FE_RESUME> qryMsts = _FeResume.GetAllData();

            return qryMsts;
        }
        #endregion

        public ResumeForm UpdateData(ResumeForm entity)
        {
            return entity;
        }

        public void UpdateTempData(string Id)
        {
            FE_RESUME? updResume = _FeResume.GetDataByCondition(x => x.FR_RSM_ID == Id).FirstOrDefault();
            if (updResume != null)
            {
                updResume.FR_ISTEMP = false;
                _FeResume.Update(updResume);
                _HRContext.SaveChanges();
            }
        }
    }
}
