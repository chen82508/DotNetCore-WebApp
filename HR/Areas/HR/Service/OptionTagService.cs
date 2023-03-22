using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class OptionTagService : IEntityService<OptionTag>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<SY_OPTION_TAG> _OptTagRepo;
        
        public OptionTagService(HRSysContext context)
        {
            _HRContext = context;
            _OptTagRepo = new HRGenericRepository<SY_OPTION_TAG>(_HRContext);
        }

        public IEnumerable<OptionTag> GetData()
        {
            return _OptTagRepo.GetDataByCondition(jtt => jtt.SY_SOT_ACT_TYP == "A")
                .Select(jtt => new OptionTag
                {
                    ID = jtt.SY_SOT_SN,
                    Name = jtt.SY_SOT_NAM
                });
        }

        public OptionTag AddData(OptionTag entity)
        {
            return new();
        }

        public OptionTag UpdateData(OptionTag entity)
        {
            return new();
        }

        public OptionTag DeleteData(int dataId)
        {
            return new();
        }
    }
}
