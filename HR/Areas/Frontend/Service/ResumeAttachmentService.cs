using HR.Database;
using HR.DTOs.Frontend;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.Frontend.Service
{
    public class ResumeAttachmentService : IEntityService<ResumeAttachment>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<FE_RESUME_ATTACHMENT> _FeResumeAttach;

        public ResumeAttachmentService(HRSysContext hRContext)
        {
            _HRContext = hRContext;
            _FeResumeAttach = new HRGenericRepository<FE_RESUME_ATTACHMENT>(_HRContext);
        }

        public ResumeAttachment AddData(ResumeAttachment entity)
        {
            return entity;
        }

        public void AddMultipleData(IEnumerable<ResumeAttachment> entities)
        {
            IEnumerable<FE_RESUME_ATTACHMENT> attachments = entities.Select(x => new FE_RESUME_ATTACHMENT
            {
                FRA_ACH_RSM_ID = x.Id,
                FRA_ACH_SEQ = x.Seq,
                FRA_ACH_NAM = x.Name,
                FRA_ACH_FIL = x.File,
                FRA_ACH_EXT = x.Extension,
                FRA_ACH_CNT_TYP = x.contentType
            });

            _FeResumeAttach.InsertRange(attachments);
            _HRContext.SaveChanges();
        }

        public ResumeAttachment DeleteData(int dataId)
        {
            return new();
        }

        public void DeleteData(string dataId, int dataSeq = 0)
        {
            if (dataSeq != 0)
            {
                FE_RESUME_ATTACHMENT? attachment = _FeResumeAttach.GetDataByCondition(x => x.FRA_ACH_RSM_ID == dataId && x.FRA_ACH_SEQ == dataSeq).FirstOrDefault();
                if (attachment != null)
                {
                    _FeResumeAttach.Remove(attachment);
                }
            }
            else
            {
                IEnumerable<FE_RESUME_ATTACHMENT> attachments = _FeResumeAttach.GetDataByCondition(x => x.FRA_ACH_RSM_ID == dataId);
                if (attachments.Any())
                {
                    _FeResumeAttach.RemoveRange(attachments);
                }
            }

            _HRContext.SaveChanges();
        }

        public IEnumerable<ResumeAttachment> GetData()
        {
            return new List<ResumeAttachment>();
        }

        public IEnumerable<ResumeAttachment> GetDataById(string id)
        {
            return _FeResumeAttach.GetDataByCondition(x => x.FRA_ACH_RSM_ID == id).OrderBy(x => x.FRA_ACH_SEQ)
                .Select(x => new ResumeAttachment
                {
                    Id = x.FRA_ACH_RSM_ID,
                    Seq = x.FRA_ACH_SEQ,
                    Name = x.FRA_ACH_NAM,
                    Extension = x.FRA_ACH_EXT,
                    contentType = x.FRA_ACH_CNT_TYP,
                });
        }

        public ResumeAttachment GetDataByPK(string id, int seq)
        {
            return _FeResumeAttach.GetDataByCondition(x => x.FRA_ACH_RSM_ID == id && x.FRA_ACH_SEQ == seq)
                .Select(x => new ResumeAttachment
                {
                    Id = x.FRA_ACH_RSM_ID,
                    Seq = x.FRA_ACH_SEQ,
                    Name = x.FRA_ACH_NAM,
                    File = x.FRA_ACH_FIL,
                    Extension = x.FRA_ACH_EXT,
                    contentType = x.FRA_ACH_CNT_TYP,
                }).First();
        }

        public ResumeAttachment UpdateData(ResumeAttachment entity)
        {
            return entity;
        }
    }
}
