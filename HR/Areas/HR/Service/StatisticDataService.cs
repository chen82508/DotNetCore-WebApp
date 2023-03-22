using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class StatisticDataService : IEntityService<StatisticData>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<VW_VAC_OVERVW> _VwVacOvwRepo;
        private readonly IRepository<VW_VACHUM_CHGHIS_STAT> _VwVacHumChgHisStat;

        public StatisticDataService(HRSysContext context)
        {
            _HRContext = context;
            _VwVacOvwRepo = new HRGenericRepository<VW_VAC_OVERVW>(_HRContext);
            _VwVacHumChgHisStat = new HRGenericRepository<VW_VACHUM_CHGHIS_STAT>(_HRContext);
        }

        private class TmpStatus
        {
            public int VAC_SN { get; set; }
            public int HUM_SN { get; set; }
            public int STATUS { get; set; }
        }

        public IEnumerable<StatisticData> GetData()
        {
            IEnumerable<StatisticData> result = _VwVacOvwRepo.GetAllData().Select(vres => new StatisticData
            {
                ID = vres.HVM_VAC_SN,
                Name = vres.HVM_VAC_NAM,
                Itvw = vres.ITVW_CNT,
                Adm = vres.ADM_CNT,
                ChkIn = vres.CHKIN_CNT,
                IsOpened = vres.HVM_VAC_STS
            });

            return result;
        }

        public IEnumerable<VacancyStatistic> GetVacancyStat()
        {
            IEnumerable<VacancyStatistic> result = _VwVacHumChgHisStat.GetAllData().Where(x => x.HVM_VAC_STS).Select(vs => new VacancyStatistic
            {
                ID = vs.HSC_CHG_SN,
                Status = vs.HSC_NEW_HHI_STS,
            });

            return result;
        }

        public StatisticData AddData(StatisticData entity)
        {
            return new();
        }

        public StatisticData UpdateData(StatisticData entity)
        {
            return new();
        }

        public StatisticData DeleteData(int dataId)
        {
            return new();
        }
    }
}
