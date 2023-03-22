using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;
using HR.Utils.Enumerate;
using System.Security.Claims;

namespace HR.Areas.HR.Service
{
    public class HumResListInfoService : IEntityService<HumResListInfo>
    {
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_HUMIFO_MST> _HumInfoMstRepo;
        private readonly IRepository<HR_HUMSTS_CHGHIS> _HumStsChgHisRepo;
        private readonly IRepository<HR_HUMVAC_RELATE> _HumVacRelRepo;
        private readonly IRepository<HR_INTERVW_HIS> _ItvwHisRepo;
        private readonly IRepository<HR_HUMATTTACH> _HumAtchRepo;
        private readonly IRepository<VW_HUMIFO_MST> _VwHumInfoMstRepo;
        private readonly IRepository<VW_HUMVAC_RELATE> _VwHumVacRelRepo;

        public HumResListInfoService(IHttpContextAccessor accessor, HRSysContext context)
        {
            _HttpContextAccessor = accessor;
            _HRContext = context;
            _HumInfoMstRepo = new HRGenericRepository<HR_HUMIFO_MST>(_HRContext);
            _HumStsChgHisRepo = new HRGenericRepository<HR_HUMSTS_CHGHIS>(_HRContext);
            _HumVacRelRepo = new HRGenericRepository<HR_HUMVAC_RELATE>(_HRContext);
            _ItvwHisRepo = new HRGenericRepository<HR_INTERVW_HIS>(_HRContext);
            _HumAtchRepo = new HRGenericRepository<HR_HUMATTTACH>(_HRContext);
            _VwHumInfoMstRepo = new HRGenericRepository<VW_HUMIFO_MST>(_HRContext);
            _VwHumVacRelRepo = new HRGenericRepository<VW_HUMVAC_RELATE>(_HRContext);
        }

        #region 取得資料
        public IEnumerable<HumResListInfo> GetData()
        {
            List<VW_HUMIFO_MST> hrMsts = _VwHumInfoMstRepo.GetAllData().OrderByDescending(mst => mst.HHI_HUM_ADD_DAT).Skip(0).Take(10).ToList();
            IEnumerable<VW_HUMVAC_RELATE> relations = _VwHumVacRelRepo.GetAllData();
            int total = _VwHumInfoMstRepo.GetAllData().Count();

            return hrMsts.Select(mst => new HumResListInfo
            {
                ID = mst.HHI_HUM_SN,
                Name = mst.HHI_HUM_NAM,
                Sex = mst.HHI_HUM_SEX_SN,
                Tel = mst.HHI_HUM_TEL,
                Src = mst.HHI_HUM_SRC,
                Arrange = string.Join("\n", relations.Where(rel => rel.HHV_HHI_HUM_SN == mst.HHI_HUM_SN).Select(rel => rel.HVM_VAC_NAM).ToList()),
                Status = ((HumanResourceStatus)mst.HHI_HUM_STS_SN).ToString(),
                CreateAt = $"{mst.HHI_HUM_ADD_DAT:yyyy/MM/dd}",
                Creator = mst.HHI_HUM_ADD_USR,
                ModifyAt = $"{mst.HHI_HUM_CHG_DAT:yyyy/MM/dd}",
                Principal = mst.HHI_PPL_USR_NAM,
                IsBlocked = mst.HHI_HUM_BLK,
            });
        }

        public IEnumerable<HumResListInfo> GetAllData()
        {
            List<VW_HUMIFO_MST> hrMsts = _VwHumInfoMstRepo.GetAllData().ToList();
            IEnumerable<VW_HUMVAC_RELATE> relations = _VwHumVacRelRepo.GetAllData();
            return hrMsts.Select(mst => new HumResListInfo
            {
                ID = mst.HHI_HUM_SN,
                Name = mst.HHI_HUM_NAM,
                Sex = mst.HHI_HUM_SEX_SN,
                Tel = mst.HHI_HUM_TEL,
                Src = mst.HHI_HUM_SRC,
                Arrange = string.Join("\n", relations.Where(rel => rel.HHV_HHI_HUM_SN == mst.HHI_HUM_SN).Select(rel => rel.HVM_VAC_NAM).ToList()),
                Status = ((HumanResourceStatus)mst.HHI_HUM_STS_SN).ToString(),
                CreateAt = $"{mst.HHI_HUM_ADD_DAT:yyyy/MM/dd}",
                Creator = mst.HHI_HUM_ADD_USR,
                ModifyAt = $"{mst.HHI_HUM_CHG_DAT:yyyy/MM/dd}",
                Principal = mst.HHI_PPL_USR_NAM,
                IsBlocked = mst.HHI_HUM_BLK,
            });
        }

        /// <summary>
        /// 依條件取得資料
        /// </summary>
        /// <param name="form">傳入的條件</param>
        /// <returns></returns>
        public IEnumerable<HumResListInfo> GetDataByConditions(IFormCollection form)
        {
            IEnumerable<VW_HUMIFO_MST> qryMsts = FilterByForm(form);

            int currPageIdx = form.Keys.Contains("pagination[current]") ? Convert.ToInt32(form["pagination[current]"]) : 1;
            int pageSize = form.Keys.Contains("pagination[size]") ? Convert.ToInt32(form["pagination[size]"]) : 10;
            List<VW_HUMIFO_MST> hrMsts = qryMsts.OrderByDescending(mst => mst.HHI_HUM_ADD_DAT).Skip((currPageIdx - 1) * pageSize).Take(pageSize).ToList();
            IEnumerable<VW_HUMVAC_RELATE> relations = _VwHumVacRelRepo.GetAllData();

            return hrMsts.Select(mst => new HumResListInfo
            {
                ID = mst.HHI_HUM_SN,
                Name = mst.HHI_HUM_NAM,
                Sex = mst.HHI_HUM_SEX_SN,
                Tel = mst.HHI_HUM_TEL,
                Src = mst.HHI_HUM_SRC,
                Arrange = string.Join("\n", relations.Where(rel => rel.HHV_HHI_HUM_SN == mst.HHI_HUM_SN).Select(rel => rel.HVM_VAC_NAM).ToList()),
                Status = ((HumanResourceStatus)mst.HHI_HUM_STS_SN).ToString(),
                CreateAt = $"{mst.HHI_HUM_ADD_DAT:yyyy/MM/dd}",
                Creator = mst.HHI_HUM_ADD_USR,
                ModifyAt = $"{mst.HHI_HUM_CHG_DAT:yyyy/MM/dd}",
                Principal = mst.HHI_PPL_USR_NAM,
                IsBlocked = mst.HHI_HUM_BLK,
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
        private IEnumerable<VW_HUMIFO_MST> FilterByForm(IFormCollection form)
        {
            IEnumerable<VW_HUMIFO_MST> qryMsts = _VwHumInfoMstRepo.GetAllData();
            if (form.Keys.Contains("conditions[name]") && !string.IsNullOrEmpty($"{form["conditions[name]"]}"))
            {
                qryMsts = qryMsts.Where(mst => mst.HHI_HUM_NAM.Contains($"{form["conditions[name]"]}"));
            }

            if (form.Keys.Contains("conditions[tel]") && !string.IsNullOrEmpty($"{form["conditions[tel]"]}"))
            {
                qryMsts = qryMsts.Where(mst => mst.HHI_HUM_TEL.Contains($"{form["conditions[tel]"]}"));
            }

            if (form.Keys.Contains("conditions[src]") && !string.IsNullOrEmpty($"{form["conditions[src]"]}"))
            {
                int srcSN = Convert.ToInt32(form["conditions[src]"]);
                qryMsts = qryMsts.Where(mst => mst.HHI_HUM_SRC_SN == srcSN);
            }

            if (form.Keys.Contains("conditions[vac]") && !string.IsNullOrEmpty($"{form["conditions[vac]"]}"))
            {
                int vacSN = Convert.ToInt32(form["conditions[vac]"]);
                List<int> relatedToVacancy = _HumVacRelRepo.GetDataByCondition(rel => rel.HHV_HVM_VAC_SN == vacSN).Select(rel => rel.HHV_HHI_HUM_SN).ToList();
                qryMsts = qryMsts.Where(mst => relatedToVacancy.Contains(mst.HHI_HUM_SN));
            }

            if (form.Keys.Contains("conditions[sts]") && !string.IsNullOrEmpty($"{form["conditions[sts]"]}"))
            {
                int stsSN = Convert.ToInt32(form["conditions[sts]"]);
                qryMsts = qryMsts.Where(mst => mst.HHI_HUM_STS_SN == stsSN);
            }

            if (form.Keys.Contains("conditions[principal]") && !string.IsNullOrEmpty($"{form["conditions[principal]"]}"))
            {
                qryMsts = qryMsts.Where(mst => mst.HHI_PPL_USR == $"{form["conditions[principal]"]}");
            }

            return qryMsts;
        }

        /// <summary>
        /// 依據人力資料流水號取得已上傳的附件資料
        /// </summary>
        /// <param name="humId">人力資料流水號</param>
        /// <returns></returns>
        public IEnumerable<HumResAttachment> GetAttachmentsByHumanId(int humId)
        {
            return _HumAtchRepo.GetDataByCondition(atch => atch.HAC_HHI_HUM_SN == humId)
                .Select(atch => new HumResAttachment
                {
                    ID = atch.HAC_ACH_SN,
                    Name = atch.HAC_ACH_NAM,
                    Ext = atch.HAC_ACH_EXT
                });
        }

        /// <summary>
        /// 依據流水號取得附件資料
        /// </summary>
        /// <param name="id">附件資料流水號</param>
        /// <returns></returns>
        public HR_HUMATTTACH GetAttachmentById(int id)
        {
            return _HumAtchRepo.GetDataByCondition(atch => atch.HAC_ACH_SN == id).First();
        }
        #endregion

        #region 新增資料
        public HumResListInfo AddData(HumResListInfo entity)
        {
            return new();
        }

        public void AddAttachment(int humId, byte[] fileBinary, IFormFile file)
        {
            HR_HUMATTTACH nHumAtch = new()
            {
                HAC_HHI_HUM_SN = humId,
                HAC_ACH_FIL = fileBinary,
                HAC_ACH_NAM = Path.GetFileNameWithoutExtension(file.FileName),
                HAC_ACH_CNT_TYP = file.ContentType,
                HAC_ACH_EXT = Path.GetExtension(file.FileName).ToLower().Replace(".", ""),
            };
            _HumAtchRepo.Insert(nHumAtch);
            _HumAtchRepo.SaveChange();
        }
        #endregion

        public HumResListInfo UpdateData(HumResListInfo entity)
        {
            HR_HUMIFO_MST? mHumInfo = _HumInfoMstRepo.GetDataByCondition(mst => mst.HHI_HUM_SN == entity.ID).FirstOrDefault();
            if (mHumInfo != null)
            {
                mHumInfo.HHI_HUM_BLK = entity.IsBlocked;
                mHumInfo.HHI_HUM_CHG_DAT = DateTime.Now;
                mHumInfo.HHI_HUM_CHG_USR = _HttpContextAccessor.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

                _HumInfoMstRepo.Update(mHumInfo);
                _HumInfoMstRepo.SaveChange();

                return new()
                {
                    ID = mHumInfo.HHI_HUM_SN,
                    Name = mHumInfo.HHI_HUM_NAM,
                    Sex = mHumInfo.HHI_HUM_SEX_SN,
                    IsBlocked = mHumInfo.HHI_HUM_BLK
                };
            }

            return new();
        }

        #region 刪除資料
        public HumResListInfo DeleteData(int dataId)
        {
            List<HR_HUMSTS_CHGHIS> dChanges = _HumStsChgHisRepo.GetDataByCondition(chg => chg.HSC_HHI_HUM_SN == dataId).ToList();
            if (dChanges.Count > 0)
            {
                _HumStsChgHisRepo.RemoveRange(dChanges);
            }

            List<HR_HUMVAC_RELATE> dRelations = _HumVacRelRepo.GetDataByCondition(rel => rel.HHV_HHI_HUM_SN == dataId).ToList();
            if (dRelations.Count > 0)
            {
                _HumVacRelRepo.RemoveRange(dRelations);
            }

            List<HR_INTERVW_HIS> dInterviews = _ItvwHisRepo.GetDataByCondition(itvw => itvw.HIH_HHI_HUM_SN == dataId).ToList();
            if (dInterviews.Count > 0)
            {
                _ItvwHisRepo.RemoveRange(dInterviews);
            }

            List<HR_HUMATTTACH> dAttachments = _HumAtchRepo.GetDataByCondition(atch => atch.HAC_HHI_HUM_SN == dataId).ToList();
            if (dAttachments.Count > 0)
            {
                _HumAtchRepo.RemoveRange(dAttachments);
            }

            HR_HUMIFO_MST? dHumResource = _HumInfoMstRepo.GetDataByCondition(hr => hr.HHI_HUM_SN == dataId).FirstOrDefault();
            if (dHumResource != null)
            {
                _HumInfoMstRepo.Remove(dHumResource);
            }

            _HRContext.SaveChanges();

            return new()
            {
                ID = dataId,
                Name = dHumResource.HHI_HUM_NAM,
                Sex = dHumResource.HHI_HUM_SEX_SN,
            };
        }

        /// <summary>
        /// 根據附件資料流水號移除附件
        /// </summary>
        /// <param name="id">附件資料流水號</param>
        /// <returns></returns>
        public HumResAttachment DeleteAttachment(int id)
        {
            HR_HUMATTTACH? dAttachment = _HumAtchRepo.GetDataByCondition(atch => atch.HAC_ACH_SN == id).FirstOrDefault();
            if (dAttachment != null)
            {
                _HumAtchRepo.Remove(dAttachment);
                _HumAtchRepo.SaveChange();
            }

            return new()
            {
                ID = id,
                Name = dAttachment.HAC_ACH_NAM,
                Ext = dAttachment.HAC_ACH_EXT,
            };
        }
        #endregion
    }
}
