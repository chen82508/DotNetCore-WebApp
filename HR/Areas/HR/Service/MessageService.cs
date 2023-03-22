using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class MessageService : IEntityService<Message>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_MESSAGE> _MsgRepo;

        public MessageService(HRSysContext context)
        {
            _HRContext = context;
            _MsgRepo = new HRGenericRepository<HR_MESSAGE>(_HRContext);
        }

        public IEnumerable<Message> GetData()
        {
            return _MsgRepo.GetDataByCondition(msg => msg.HMS_MSG_ACT_TYP == "A")
                .OrderByDescending(msg => msg.HMT_MSG_UPD_DAT)
                .Select(msg => new Message
                {
                    CID = msg.HMS_MSG_MGT_SN,
                    ID = msg.HMS_MSG_SN,
                    Subject = msg.HMS_MSG_SUB,
                    Content = msg.HMS_MSG_CONT,
                });
        }

        public Message AddData(Message entity)
        {
            HR_MESSAGE msg = new()
            {
                HMS_MSG_MGT_SN = entity.CID,
                HMS_MSG_SUB = entity.Subject,
                HMS_MSG_CONT = entity.Content,
                HMS_MSG_ACT_TYP = "A",
                HMT_MSG_ADD_DAT = DateTime.Now,
                HMT_MSG_UPD_DAT = DateTime.Now,
            };
            _MsgRepo.Insert(msg);
            _MsgRepo.SaveChange();

            return new()
            {
                CID = msg.HMS_MSG_MGT_SN,
                ID = msg.HMS_MSG_SN,
                Subject = msg.HMS_MSG_SUB,
                Content = msg.HMS_MSG_CONT
            };
        }

        public Message UpdateData(Message entity)
        {
            HR_MESSAGE? mMsg = _MsgRepo.GetDataByCondition(msg => msg.HMS_MSG_SN == entity.ID).FirstOrDefault();
            if (mMsg != null)
            {
                mMsg.HMS_MSG_SUB = entity.Subject;
                mMsg.HMS_MSG_CONT = entity.Content;
                mMsg.HMT_MSG_UPD_DAT = DateTime.Now;
                _MsgRepo.Update(mMsg);
                _MsgRepo.SaveChange();

                return new()
                {
                    CID = mMsg.HMS_MSG_MGT_SN,
                    ID = mMsg.HMS_MSG_SN,
                    Subject = entity.Subject,
                    Content = entity.Content,
                };
            }
            return new();
        }

        public Message DeleteData(int dataId)
        {
            HR_MESSAGE? dMsg = _MsgRepo.GetDataByCondition(msg => msg.HMS_MSG_SN == dataId).FirstOrDefault();
            if (dMsg != null)
            {
                _MsgRepo.Remove(dMsg);
                _MsgRepo.SaveChange();
            }

            return new()
            {
                CID = dMsg.HMS_MSG_MGT_SN,
                ID = dMsg.HMS_MSG_SN,
                Subject = dMsg.HMS_MSG_SUB,
                Content = dMsg.HMS_MSG_CONT
            };
        }
    }
}
