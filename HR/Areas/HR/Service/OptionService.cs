using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class OptionService : IEntityService<Option>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<SY_OPTION> _OptionRepo;

        public OptionService(HRSysContext context)
        {
            _HRContext = context;
            _OptionRepo = new HRGenericRepository<SY_OPTION>(_HRContext);
        }

        public IEnumerable<Option> GetData()
        {
            return _OptionRepo.GetDataByCondition(opt => opt.SY_OPT_ACT_TYP == "A")
                .Select(opt => new Option
                {
                    TID = opt.SY_OPT_SOT_SN,
                    ID = opt.SY_OPT_SN,
                    Name = opt.SY_OPT_NAM
                });
        }

        /// <summary>
        /// 取得職缺所屬公司選項
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Option> GetOptionsByTagSn(int tagSN)
        {
            return _OptionRepo.GetDataByCondition(opt => opt.SY_OPT_ACT_TYP == "A" && opt.SY_OPT_SOT_SN == tagSN)
                .Select(opt => new Option
                {
                    TID = opt.SY_OPT_SOT_SN,
                    ID = opt.SY_OPT_SN,
                    Name = opt.SY_OPT_NAM
                });
        }

        public Option AddData(Option entity)
        {
            SY_OPTION option = new()
            {
                SY_OPT_SOT_SN = entity.TID,
                SY_OPT_NAM = entity.Name,
                SY_OPT_ACT_TYP = "A",
                SY_OPT_ADD_DAT = DateTime.Now,
                SY_OPT_UPD_DAT = DateTime.Now,
            };
            _OptionRepo.Insert(option);
            _OptionRepo.SaveChange();

            return new()
            {
                TID = option.SY_OPT_SOT_SN,
                ID = option.SY_OPT_SN,
                Name = option.SY_OPT_NAM
            };
        }

        public Option UpdateData(Option entity)
        {
            SY_OPTION? mOption = _OptionRepo.GetDataByCondition(o => o.SY_OPT_SN == entity.ID).FirstOrDefault();
            if (mOption != null)
            {
                mOption.SY_OPT_NAM = entity.Name;
                mOption.SY_OPT_UPD_DAT = DateTime.Now;
            }
            _OptionRepo.Update(mOption);
            _OptionRepo.SaveChange();

            return new()
            {
                TID = mOption.SY_OPT_SOT_SN,
                ID = mOption.SY_OPT_SN,
                Name = mOption.SY_OPT_NAM
            };
        }

        public Option DeleteData(int dataId)
        {
            SY_OPTION? dOption = _OptionRepo.GetDataByCondition(o => o.SY_OPT_SN == dataId).FirstOrDefault();
            if (dOption != null)
            {
                _OptionRepo.Remove(dOption);
                _OptionRepo.SaveChange();
            }

            return new()
            {
                TID = dOption.SY_OPT_SOT_SN,
                ID = dOption.SY_OPT_SN,
                Name = dOption.SY_OPT_NAM
            };
        }
    }
}
