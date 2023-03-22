using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class MessageTagService : IEntityService<MessageTag>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_MESSAGE_TAG> _MsgTagRepo;

        public MessageTagService(HRSysContext context)
        {
            _HRContext = context;
            _MsgTagRepo = new HRGenericRepository<HR_MESSAGE_TAG>(_HRContext);
        }

        public IEnumerable<MessageTag> GetData()
        {
            return _MsgTagRepo.GetDataByCondition(mt => mt.HMT_MGT_ACT_TYP == "A")
                .OrderBy(mt => mt.HMT_MGT_SEQ)
                .Select(mt => new MessageTag
                {
                    ID = mt.HMT_MGT_SN,
                    Title = mt.HMT_MGT_NAM
                });
        }

        public MessageTag AddData(MessageTag entity)
        {
            int MaxSeq = _MsgTagRepo.GetAllData().Count();
            HR_MESSAGE_TAG MsgTag = new()
            {
                HMT_MGT_NAM = entity.Title,
                HMT_MGT_ACT_TYP = "A",
                HMT_MGT_ADD_DAT = DateTime.Now,
                HMT_MGT_UPD_DAT = DateTime.Now,
                HMT_MGT_SEQ = MaxSeq + 1,
            };

            _MsgTagRepo.Insert(MsgTag);
            _MsgTagRepo.SaveChange();

            return new()
            {
                ID = MsgTag.HMT_MGT_SN,
                Title = MsgTag.HMT_MGT_NAM
            };
        }

        public MessageTag UpdateData(MessageTag entity)
        {
            return new();
        }

        public MessageTag DeleteData(int dataId)
        {
            return new();
        }
    }
}
