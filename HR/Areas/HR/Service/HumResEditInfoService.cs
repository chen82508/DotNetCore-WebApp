using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;
using System.Security.Claims;

namespace HR.Areas.HR.Service
{
    public class HumResEditInfoService : IEntityService<HumResEditInfo>
    {
        private readonly IHttpContextAccessor _HttpContext;
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_HUMIFO_MST> _HumInfoMstRepo;
        private readonly IRepository<HR_HUMSTS_CHGHIS> _HumStsChgHisRepo;
        private readonly IRepository<HR_HUMVAC_RELATE> _HumVacRelRepo;
        private readonly IRepository<HR_INTERVW_HIS> _ItvwHisRepo;
        private readonly IRepository<SY_OPTION> _OptRepo;
        private readonly IRepository<SY_USER> _UsrRepo;

        public HumResEditInfoService(IHttpContextAccessor contextAccessor, HRSysContext context)
        {
            _HttpContext = contextAccessor;
            _HRContext = context;
            _HumInfoMstRepo = new HRGenericRepository<HR_HUMIFO_MST>(_HRContext);
            _HumStsChgHisRepo = new HRGenericRepository<HR_HUMSTS_CHGHIS>(_HRContext);
            _HumVacRelRepo = new HRGenericRepository<HR_HUMVAC_RELATE>(_HRContext);
            _ItvwHisRepo = new HRGenericRepository<HR_INTERVW_HIS>(_HRContext);
            _OptRepo = new HRGenericRepository<SY_OPTION>(_HRContext);
            _UsrRepo = new HRGenericRepository<SY_USER>(_HRContext);
        }

        #region 取得資料
        public IEnumerable<HumResEditInfo> GetData()
        {
            return new List<HumResEditInfo>();
        }

        /// <summary>
        /// 依ID取得資料
        /// </summary>
        /// <param name="id">資料ID</param>
        /// <returns></returns>
        public HumResEditInfo GetDataById(int id)
        {
            HR_HUMIFO_MST mst = _HumInfoMstRepo.GetDataByCondition(hr => hr.HHI_HUM_SN == id).First();
            HR_HUMSTS_CHGHIS? latestChg = _HumStsChgHisRepo.GetDataByCondition(chg => chg.HSC_HHI_HUM_SN == id).OrderByDescending(chg => chg.HSC_CHG_SN).FirstOrDefault();
            List<SY_USER> users = _UsrRepo.GetAllData().ToList();
            List<HumItvw> itvwHis = _ItvwHisRepo.GetDataByCondition(itvw => itvw.HIH_HHI_HUM_SN == id)
                .OrderByDescending(x => x.HIH_ITV_DAT).AsEnumerable()
                .GroupJoin(users, L => L.HIH_ADD_USR, R => R.SY_USR_ID, (Itv, Usr) => new HumItvw
                {
                    ID = Itv.HIH_ITV_HIS_SN,
                    Time = $"{Itv.HIH_ITV_DAT:yyyy/MM/dd HH:mm}",
                    Content = Itv.HIH_ITV_TXT,
                    Creator = Usr.Any() ? Usr.Select(u => u.SY_USR_NAM).First() : "",
                }).ToList();

            return new()
            {
                ID = mst.HHI_HUM_SN,
                Name = mst.HHI_HUM_NAM,
                Sex = mst.HHI_HUM_SEX_SN,
                Tel = mst.HHI_HUM_TEL,
                Email = mst.HHI_HUM_EMAIL,
                Src = mst.HHI_HUM_SRC_SN,
                Status = mst.HHI_HUM_STS_SN,
                Principal = mst.HHI_PPL_USR,
                Interviews = itvwHis,
                Reason = latestChg != null ? latestChg.HSC_CHG_RSN_SN : -1,
                OtherReason = latestChg != null && latestChg.HSC_CHG_RSN_SN == 0 ? latestChg.HSC_CHG_RSN : "",
            };
        }

        /// <summary>
        /// 取得關聯的職缺資料流水號
        /// </summary>
        /// <param name="VacancyId">人力資料ID</param>
        /// <returns></returns>
        public IEnumerable<int> GetRelations(int id)
        {
            return _HumVacRelRepo.GetDataByCondition(rel => rel.HHV_HHI_HUM_SN == id)
                .Select(rel => rel.HHV_HVM_VAC_SN);
        }

        /// <summary>
        /// 以姓名及電話查詢資料
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="tel">電話</param>
        /// <returns></returns>
        public IEnumerable<HR_HUMIFO_MST> GetDataByNameAndTel(string name, string tel)
        {
            return _HumInfoMstRepo.GetDataByCondition(x => x.HHI_HUM_NAM == name && x.HHI_HUM_TEL == tel);
        }
        #endregion

        #region 新增資料
        public HumResEditInfo AddData(HumResEditInfo entity)
        {
            string userId = _HttpContext.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            HR_HUMIFO_MST nHumInfoMst = new()
            {
                HHI_HUM_NAM = entity.Name,
                HHI_HUM_SEX_SN = entity.Sex,
                HHI_HUM_TEL = entity.Tel != null ? entity.Tel.Replace("-", "").Trim() : "",
                HHI_HUM_EMAIL = entity.Email,
                HHI_HUM_SRC_SN = entity.Src,
                HHI_HUM_STS_SN = entity.Status,
                HHI_HUM_ADD_DAT = DateTime.Now,
                HHI_HUM_ADD_USR = userId,
                HHI_HUM_CHG_DAT = DateTime.Now,
                HHI_HUM_CHG_USR = userId,
                HHI_PPL_USR = entity.Principal ?? "",
                HHI_HUM_BLK = false
            };
            _HumInfoMstRepo.Insert(nHumInfoMst);
            _HumInfoMstRepo.SaveChange();

            if (entity.Vacancies.Count > 0)
            {
                List<HR_HUMVAC_RELATE> insertRelEntities = new();
                foreach (int vacSN in entity.Vacancies)
                {
                    insertRelEntities.Add(new HR_HUMVAC_RELATE
                    {
                        HHV_HHI_HUM_SN = nHumInfoMst.HHI_HUM_SN,
                        HHV_HVM_VAC_SN = vacSN,
                    });
                }
                _HumVacRelRepo.InsertRange(insertRelEntities);
            }

            #region 新增一筆異動紀錄
            string ReasonContent = entity.Reason == 0 ? entity.OtherReason : "新增資料";

            SY_OPTION? selectedReason = _OptRepo.GetDataByCondition(opt => opt.SY_OPT_SN == entity.Reason).FirstOrDefault();
            ReasonContent = selectedReason != null ? selectedReason.SY_OPT_NAM : ReasonContent;

            HR_HUMSTS_CHGHIS nChgHis = new()
            {
                HSC_HHI_HUM_SN = nHumInfoMst.HHI_HUM_SN,
                HSC_ORI_HHI_STS = entity.OriStatus,
                HSC_NEW_HHI_STS = entity.Status,
                HSC_CHG_RSN_SN = entity.Reason,
                HSC_CHG_RSN = ReasonContent,
            };
            _HumStsChgHisRepo.Insert(nChgHis);
            #endregion

            if (entity.Interviews != null && entity.Interviews.Count > 0)
            {
                entity.Interviews.Reverse();

                List<HR_INTERVW_HIS> insertItvwEntities = new();
                foreach (HumItvw itvw in entity.Interviews)
                {
                    insertItvwEntities.Add(new()
                    {
                        HIH_HHI_HUM_SN = nHumInfoMst.HHI_HUM_SN,
                        HIH_ITV_DAT = Convert.ToDateTime(itvw.Time),
                        HIH_ITV_TXT = itvw.Content,
                        HIH_ADD_USR = userId,
                    });
                }
                _ItvwHisRepo.InsertRange(insertItvwEntities);
            }

            _HRContext.SaveChanges();

            return new() { ID = nHumInfoMst.HHI_HUM_SN };
        }

        public HumResStatusChg AddChangeStatusHis(HumResStatusChg chg)
        {
            HR_HUMSTS_CHGHIS nChgHis = new()
            {
                HSC_HHI_HUM_SN = chg.HumanId,
                HSC_ORI_HHI_STS = chg.Origin,
                HSC_NEW_HHI_STS = chg.Changed,
                HSC_CHG_RSN_SN = chg.Reason,
            };

            _HumStsChgHisRepo.Insert(nChgHis);

            return new();
        }
        #endregion

        public HumResEditInfo UpdateData(HumResEditInfo entity)
        {
            string userId = _HttpContext.HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Name).Value;

            HR_HUMIFO_MST? mHumInfoMst = _HumInfoMstRepo.GetDataByCondition(hr => hr.HHI_HUM_SN == entity.ID).FirstOrDefault();
            if (mHumInfoMst != null)
            {
                mHumInfoMst.HHI_HUM_NAM = entity.Name;
                mHumInfoMst.HHI_HUM_SEX_SN = entity.Sex;
                mHumInfoMst.HHI_HUM_TEL = entity.Tel ?? "";
                mHumInfoMst.HHI_HUM_EMAIL = entity.Email;
                mHumInfoMst.HHI_HUM_SRC_SN = entity.Src;
                mHumInfoMst.HHI_HUM_STS_SN = entity.Status;
                mHumInfoMst.HHI_PPL_USR = entity.Principal ?? "";
                mHumInfoMst.HHI_HUM_CHG_DAT = DateTime.Now;
                mHumInfoMst.HHI_HUM_CHG_USR = userId;

                _HumInfoMstRepo.Update(mHumInfoMst);
            }

            if (entity.Vacancies.Any())
            {
                List<HR_HUMVAC_RELATE> dRelations = _HumVacRelRepo.GetDataByCondition(rel => rel.HHV_HHI_HUM_SN == entity.ID).ToList();
                _HumVacRelRepo.RemoveRange(dRelations);

                List<HR_HUMVAC_RELATE> insertRelEntities = new();
                foreach (int vacSN in entity.Vacancies)
                {
                    insertRelEntities.Add(new HR_HUMVAC_RELATE
                    {
                        HHV_HHI_HUM_SN = entity.ID,
                        HHV_HVM_VAC_SN = vacSN,
                    });
                }
                _HumVacRelRepo.InsertRange(insertRelEntities);
            }

            string ReasonContent = entity.Reason == 0 ? entity.OtherReason : "";

            SY_OPTION? selectedReason = _OptRepo.GetDataByCondition(opt => opt.SY_OPT_SN == entity.Reason).FirstOrDefault();
            ReasonContent = selectedReason != null ? selectedReason.SY_OPT_NAM : ReasonContent;

            HR_HUMSTS_CHGHIS nChgHis = new()
            {
                HSC_HHI_HUM_SN = entity.ID,
                HSC_ORI_HHI_STS = entity.OriStatus,
                HSC_NEW_HHI_STS = entity.Status,
                HSC_CHG_RSN_SN = entity.Reason,
                HSC_CHG_RSN = ReasonContent,
            };
            _HumStsChgHisRepo.Insert(nChgHis);

            if (entity.Interviews != null && entity.Interviews.Count > 0)
            {
                entity.Interviews.Reverse();

                List<HR_INTERVW_HIS> insertItvwEntities = new();
                List<HR_INTERVW_HIS> updateItvwEntities = new();
                foreach (HumItvw itvw in entity.Interviews)
                {
                    if (itvw.ID != 0)
                    {
                        HR_INTERVW_HIS? itvwHis = _ItvwHisRepo.GetDataByCondition(itv => itv.HIH_ITV_HIS_SN == itvw.ID).FirstOrDefault();
                        if (itvwHis != null)
                        {
                            itvwHis.HIH_ITV_TXT = itvw.Content;
                            updateItvwEntities.Add(itvwHis);
                        }
                    }
                    else
                    {
                        // 前端js提供的時間資料是UTC格式，新增時須自行增加8小時。
                        insertItvwEntities.Add(new()
                        {
                            HIH_HHI_HUM_SN = entity.ID,
                            HIH_ITV_DAT = Convert.ToDateTime(itvw.Time),
                            HIH_ITV_TXT = itvw.Content,
                            HIH_ADD_USR = userId,
                        });
                    }
                }
                _ItvwHisRepo.InsertRange(insertItvwEntities);
                _ItvwHisRepo.UpdateRange(updateItvwEntities);
            }

            _HRContext.SaveChanges();

            return new() { ID = entity.ID, Name = entity.Name, Sex = entity.Sex };
        }

        public HumResEditInfo DeleteData(int dataId)
        {
            return new();
        }

        public HumItvw DeleteInterviewHis(int dataId)
        {
            HR_INTERVW_HIS? dItvwHis = _ItvwHisRepo.GetDataByCondition(itvw => itvw.HIH_ITV_HIS_SN == dataId).FirstOrDefault();
            if (dItvwHis != null)
            {
                _ItvwHisRepo.Remove(dItvwHis);
                _ItvwHisRepo.SaveChange();
            }

            return new()
            {
                ID = dataId,
                Time = $"{dItvwHis.HIH_ITV_DAT:yyyy/MM/dd}",
                Content = dItvwHis.HIH_ITV_TXT
            };
        }
    }
}
