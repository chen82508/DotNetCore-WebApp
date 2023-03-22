using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class JobInfoService : IEntityService<JobInfo>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_VACMST> _VacMstRepo;
        private readonly IRepository<HR_VACANCY_HIS> _VacHisRepo;
        private readonly IRepository<HR_VACTYP_RELATE> _VacTypRelateRepo;

        public JobInfoService(HRSysContext context)
        {
            _HRContext = context;
            _VacMstRepo = new HRGenericRepository<HR_VACMST>(_HRContext);
            _VacHisRepo = new HRGenericRepository<HR_VACANCY_HIS>(_HRContext);
            _VacTypRelateRepo = new HRGenericRepository<HR_VACTYP_RELATE>(_HRContext);
        }

        #region 取得資料
        /// <summary>
        /// 取得資料總數
        /// </summary>
        /// <param name="form">資料篩選條件</param>
        /// <returns></returns>
        public int GetDataTotal(IFormCollection form)
        {
            return FilterByForm(form).Count();
        }

        public IEnumerable<JobInfo> GetData()
        {
            List<HR_VACMST> vACMSTs = _VacMstRepo.GetDataByCondition(vac => vac.HVM_ACT_TYP == "A").Skip(0).Take(20).ToList();
            List<HR_VACANCY_HIS> vACANCY_HIs = _VacHisRepo.GetAllData().GroupBy(his => his.HVH_HVM_VAC_SN).Select(his => his.OrderByDescending(x => x.HVH_HIS_SN).First()).ToList();

            return vACMSTs.GroupJoin(vACANCY_HIs, L => L.HVM_VAC_SN, R => R.HVH_HVM_VAC_SN, (L, R) => new JobInfo
            {
                ID = L.HVM_VAC_SN,
                Name = L.HVM_VAC_NAM,
                IsOpen = L.HVM_VAC_STS,
                ItvStart = !R.Any() ? "" : R.Select(x => x.HVH_OPN_DAT).First().ToString("yyyy/MM/dd"),
                ItvEnd = !R.Any() ? "" : R.Select(x => x.HVH_CLS_DAT).First().ToString("yyyy/MM/dd"),
                DaysIndicator = L.HVM_DAY_IDT,
                Company = L.HVM_CMP_SN,
            });
        }

        public IEnumerable<JobInfo> GetAllData()
        {
            List<HR_VACMST> vACMSTs = _VacMstRepo.GetDataByCondition(vac => vac.HVM_ACT_TYP == "A").ToList();
            List<HR_VACANCY_HIS> vACANCY_HIs = _VacHisRepo.GetAllData().GroupBy(his => his.HVH_HVM_VAC_SN).Select(his => his.OrderByDescending(x => x.HVH_HIS_SN).First()).ToList();

            return vACMSTs.GroupJoin(vACANCY_HIs, L => L.HVM_VAC_SN, R => R.HVH_HVM_VAC_SN, (L, R) => new JobInfo
            {
                ID = L.HVM_VAC_SN,
                Name = L.HVM_VAC_NAM,
                IsOpen = L.HVM_VAC_STS,
                ItvStart = !R.Any() ? "" : R.Select(x => x.HVH_OPN_DAT).First().ToString("yyyy/MM/dd"),
                ItvEnd = !R.Any() ? "" : R.Select(x => x.HVH_CLS_DAT).First().ToString("yyyy/MM/dd"),
                DaysIndicator = L.HVM_DAY_IDT,
                Company = L.HVM_CMP_SN,
            });
        }

        /// <summary>
        /// 依條件取得資料
        /// </summary>
        /// <param name="form">傳入的條件</param>
        /// <returns></returns>
        public IEnumerable<JobInfo> GetDataByConditions(IFormCollection form)
        {
            IEnumerable<HR_VACMST> qryMsts = FilterByForm(form);

            int currPageIdx = form.Keys.Contains("pagination[current]") ? Convert.ToInt32(form["pagination[current]"]) : 1;
            int pageSize = form.Keys.Contains("pagination[size]") ? Convert.ToInt32(form["pagination[size]"]) : 20;
            List<HR_VACMST> VacMsts = qryMsts.Skip((currPageIdx - 1) * pageSize).Take(pageSize).ToList();
            List<HR_VACANCY_HIS> vACANCY_HIs = _VacHisRepo.GetAllData().GroupBy(his => his.HVH_HVM_VAC_SN).Select(his => his.OrderByDescending(x => x.HVH_HIS_SN).First()).ToList();

            return VacMsts.GroupJoin(vACANCY_HIs, L => L.HVM_VAC_SN, R => R.HVH_HVM_VAC_SN, (L, R) => new JobInfo
            {
                ID = L.HVM_VAC_SN,
                Name = L.HVM_VAC_NAM,
                IsOpen = L.HVM_VAC_STS,
                ItvStart = !R.Any() ? "" : R.Select(x => x.HVH_OPN_DAT).First().ToString("yyyy/MM/dd"),
                ItvEnd = !R.Any() ? "" : R.Select(x => x.HVH_CLS_DAT).First().ToString("yyyy/MM/dd"),
                DaysIndicator = L.HVM_DAY_IDT,
                Company = L.HVM_CMP_SN,
            });
        }

        /// <summary>
        /// 過濾條件
        /// </summary>
        /// <param name="form">資料篩選條件</param>
        /// <returns></returns>
        private IEnumerable<HR_VACMST> FilterByForm(IFormCollection form)
        {
            IEnumerable<HR_VACMST> qryMsts = _VacMstRepo.GetAllData();
            if (form.Keys.Contains("isOpen") && !string.IsNullOrEmpty($"{form["isOpen"]}"))
            {
                int isOpenFlag = Convert.ToInt32(form["isOpen"]);
                bool isOpen = Convert.ToBoolean(isOpenFlag);
                qryMsts = qryMsts.Where(x => x.HVM_VAC_STS == isOpen);
            }

            return qryMsts;
        }

        /// <summary>
        /// 依ID取得資料
        /// </summary>
        /// <param name="id">資料ID</param>
        /// <returns></returns>
        public JobInfo GetDataById(int id)
        {
            HR_VACMST Vacancy = _VacMstRepo.GetDataByCondition(vac => vac.HVM_VAC_SN == id).First();
            List<int> Relations = _VacTypRelateRepo.GetDataByCondition(rel => rel.HVT_HVM_VAC_SN == id).Select(x => x.HVT_HJT_JOB_SN).ToList();
            List<HR_VACANCY_HIS> Histories = _VacHisRepo.GetDataByCondition(his => his.HVH_HVM_VAC_SN == id).OrderByDescending(x => x.HVH_HIS_SN).ToList();

            return new()
            {
                ID = id,
                Name = Vacancy.HVM_VAC_NAM,
                IsOpen = Vacancy.HVM_VAC_STS,
                Categories = Relations,
                Company = Vacancy.HVM_CMP_SN,
                DaysIndicator = Vacancy.HVM_DAY_IDT,
                History = Histories.Select(x => $"{x.HVH_OPN_DAT:yyyy/MM/dd} ~ {(x.HVH_OPN_DAT == x.HVH_CLS_DAT ? "現在" : $"{x.HVH_CLS_DAT:yyyy/MM/dd}")}").ToList(),
            };
        }

        /// <summary>
        /// 取得關聯的職務類型流水號
        /// </summary>
        /// <param name="VacancyId">職缺資料ID</param>
        /// <returns></returns>
        public IEnumerable<int> GetRelations(int VacancyId)
        {
            return _VacTypRelateRepo.GetDataByCondition(rel => rel.HVT_HVM_VAC_SN == VacancyId)
                .Select(x => x.HVT_HJT_JOB_SN);
        }
        #endregion

        public JobInfo AddData(JobInfo entity)
        {
            HR_VACMST vACMST = new()
            {
                HVM_VAC_NAM = entity.Name,
                HVM_VAC_STS = entity.IsOpen,
                HVM_CMP_SN = entity.Company,
                HVM_ACT_TYP = "A",
                HVM_DAY_IDT = entity.DaysIndicator,
            };
            _VacMstRepo.Insert(vACMST);
            // 先儲存主檔來取得新資料的流水號
            _VacMstRepo.SaveChange();

            foreach (int jobTypSN in entity.Categories)
            {
                _VacTypRelateRepo.Insert(new()
                {
                    HVT_HJT_JOB_SN = jobTypSN,
                    HVT_HVM_VAC_SN = vACMST.HVM_VAC_SN,
                });
            }

            // 判斷是否預設開啟
            if (entity.IsOpen)
            {
                DateTime dtCurrent = DateTime.Now;
                _VacHisRepo.Insert(new()
                {
                    HVH_HVM_VAC_SN = vACMST.HVM_VAC_SN,
                    HVH_OPN_DAT = dtCurrent,
                    HVH_CLS_DAT = dtCurrent,
                });
            }
            _HRContext.SaveChanges();

            HR_VACANCY_HIS? defaultHis = _VacHisRepo.GetDataByCondition(x => x.HVH_HVM_VAC_SN == vACMST.HVM_VAC_SN).FirstOrDefault();

            return new()
            {
                ID = vACMST.HVM_VAC_SN,
                Name = vACMST.HVM_VAC_NAM,
                IsOpen = vACMST.HVM_VAC_STS,
                DaysIndicator = entity.DaysIndicator,
                ItvStart = defaultHis != null ? $"{defaultHis.HVH_OPN_DAT:yyyy/MM/dd}" : "",
                ItvEnd = defaultHis != null ? $"{defaultHis.HVH_CLS_DAT:yyyy/MM/dd}" : "",
            };
        }

        #region 更新資料
        public JobInfo UpdateData(JobInfo entity)
        {
            HR_VACMST? mVacMst = _VacMstRepo.GetDataByCondition(vac => vac.HVM_VAC_SN == entity.ID).FirstOrDefault();
            if (mVacMst != null)
            {
                mVacMst.HVM_VAC_NAM = entity.Name;
                mVacMst.HVM_CMP_SN = entity.Company;
                mVacMst.HVM_VAC_STS = entity.IsOpen;
                mVacMst.HVM_DAY_IDT = entity.DaysIndicator;
                _VacMstRepo.Update(mVacMst);
            }

            List<HR_VACTYP_RELATE> mRelations = _VacTypRelateRepo.GetDataByCondition(rel => rel.HVT_HVM_VAC_SN == entity.ID).ToList();
            if (mRelations.Count > 0)
            {
                _HRContext.HR_VACTYP_RELATE.RemoveRange(mRelations);
            }
            foreach (int jobTypSN in entity.Categories)
            {
                HR_VACTYP_RELATE nRelateion = new()
                {
                    HVT_HJT_JOB_SN = jobTypSN,
                    HVT_HVM_VAC_SN = entity.ID,
                };
                _VacTypRelateRepo.Insert(nRelateion);
            }

            _HRContext.SaveChanges();

            List<HR_VACANCY_HIS> vACANCY_HIs = _VacHisRepo.GetDataByCondition(his => his.HVH_HVM_VAC_SN == entity.ID)
                .OrderByDescending(his => his.HVH_HIS_SN).ToList();

            return new()
            {
                ID = entity.ID,
                Name = entity.Name,
                IsOpen = entity.IsOpen,
                DaysIndicator = entity.DaysIndicator,
                ItvStart = !vACANCY_HIs.Any() ? "" : vACANCY_HIs.Select(x => x.HVH_OPN_DAT).First().ToString("yyyy/MM/dd"),
                ItvEnd = !vACANCY_HIs.Any() ? "" : vACANCY_HIs.Select(x => x.HVH_CLS_DAT).First().ToString("yyyy/MM/dd"),
            };
        }

        /// <summary>
        /// 更新歷程
        /// </summary>
        /// <param name="hvmVacSN">職缺資料流水號</param>
        /// <returns></returns>
        public IEnumerable<string> UpdateHistory(int hvmVacSN, bool isOpened, DateTime? closeDate = null)
        {
            DateTime dtClose = closeDate != null ? closeDate.Value : DateTime.Now;

            IEnumerable<HR_VACANCY_HIS> Histories = _VacHisRepo.GetDataByCondition(his => his.HVH_HVM_VAC_SN == hvmVacSN);
            if (!Histories.Any() || isOpened)
            {
                DateTime dtCurrent = DateTime.Now;
                HR_VACANCY_HIS nVacancyHis = new()
                {
                    HVH_HVM_VAC_SN = hvmVacSN,
                    HVH_OPN_DAT = dtCurrent,
                    HVH_CLS_DAT = dtClose,
                };
                _VacHisRepo.Insert(nVacancyHis);
            }
            else
            {
                HR_VACANCY_HIS latestHis = Histories.OrderByDescending(his => his.HVH_HIS_SN).First();
                latestHis.HVH_CLS_DAT = dtClose;

                _VacHisRepo.Update(latestHis);
            }

            HR_VACMST vACMST = _VacMstRepo.GetDataByCondition(vac => vac.HVM_VAC_SN == hvmVacSN).First();
            vACMST.HVM_VAC_STS = isOpened;
            _VacMstRepo.Update(vACMST);

            _HRContext.SaveChanges();

            return _VacHisRepo.GetDataByCondition(his => his.HVH_HVM_VAC_SN == hvmVacSN).OrderByDescending(his => his.HVH_HIS_SN)
                .Select(his => $"{his.HVH_OPN_DAT:yyyy/MM/dd} ~ {(his.HVH_OPN_DAT == his.HVH_CLS_DAT ? "現在" : $"{his.HVH_CLS_DAT:yyyy/MM/dd}")}");
        }

        /// <summary>
        /// 更新關閉資訊
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public JobInfo UpdateCloseInfo(JobInfo entity)
        {
            HR_VACMST? mVacMst = _VacMstRepo.GetDataByCondition(vac => vac.HVM_VAC_SN == entity.ID).FirstOrDefault();
            if (mVacMst != null)
            {
                mVacMst.HVM_VAC_STS = false;
                mVacMst.HVM_CLS_DAT = entity.CloseDate;
                mVacMst.HVM_CLS_RSN = entity.CloseReason;
                _VacMstRepo.Update(mVacMst);
            }
            UpdateHistory(entity.ID, false, entity.CloseDate);
            return entity;
        }
        #endregion

        public JobInfo DeleteData(int dataId)
        {
            // 與各關聯資料間有FK聯繫，故先從關聯資料開始移除
            List<HR_VACANCY_HIS> dHistories = _VacHisRepo.GetDataByCondition(his => his.HVH_HVM_VAC_SN == dataId).ToList();
            if (dHistories.Any())
            {
                _VacHisRepo.RemoveRange(dHistories);
            }

            List<HR_VACTYP_RELATE> dRelations = _VacTypRelateRepo.GetDataByCondition(rel => rel.HVT_HVM_VAC_SN == dataId).ToList();
            if (dRelations.Any())
            {
                _VacTypRelateRepo.RemoveRange(dRelations);
            }

            HR_VACMST? dVacancy = _VacMstRepo.GetDataByCondition(vac => vac.HVM_VAC_SN == dataId).FirstOrDefault();
            if (dVacancy != null)
            {
                _VacMstRepo.Remove(dVacancy);
            }

            _HRContext.SaveChanges();

            return new() { ID = dataId };
        }
    }
}
