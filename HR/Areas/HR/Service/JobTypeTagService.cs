using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class JobTypeTagService : IEntityService<JobTypeTag>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_JOBTYP_TAG> _JobTypTagRepo;

        public JobTypeTagService(HRSysContext context)
        {
            _HRContext = context;
            _JobTypTagRepo = new HRGenericRepository<HR_JOBTYP_TAG>(_HRContext);
        }

        public IEnumerable<JobTypeTag> GetData()
        {
            return _JobTypTagRepo.GetDataByCondition(jtt => jtt.HJT_ACT_TYP == "A")
                .OrderByDescending(jtt => jtt.HJT_TAG_UPD_DAT)
                .Select(jtt => new JobTypeTag
                {
                    ID = jtt.HJT_TAG_SN,
                    Name = jtt.HJT_TAG_NAM
                });
        }

        public JobTypeTag AddData(JobTypeTag entity)
        {
            HR_JOBTYP_TAG jobTypTag = new()
            {
                HJT_TAG_NAM = entity.Name,
                HJT_ACT_TYP = "A",
                HJT_TAG_ADD_DAT = DateTime.Now,
                HJT_TAG_UPD_DAT = DateTime.Now,
            };
            _JobTypTagRepo.Insert(jobTypTag);
            _JobTypTagRepo.SaveChange();

            return new()
            {
                ID = jobTypTag.HJT_TAG_SN,
                Name = jobTypTag.HJT_TAG_NAM
            };
        }

        public JobTypeTag UpdateData(JobTypeTag entity)
        {
            HR_JOBTYP_TAG? mJobTypTag = _JobTypTagRepo.GetDataByCondition(jtt => jtt.HJT_TAG_SN == entity.ID).FirstOrDefault();
            if (mJobTypTag != null)
            {
                mJobTypTag.HJT_TAG_NAM = entity.Name;
                mJobTypTag.HJT_TAG_UPD_DAT = DateTime.Now;
            }
            _JobTypTagRepo.Update(mJobTypTag);
            _JobTypTagRepo.SaveChange();

            return new()
            {
                ID = mJobTypTag.HJT_TAG_SN,
                Name = mJobTypTag.HJT_TAG_NAM
            };
        }

        public JobTypeTag DeleteData(int dataId)
        {
            HR_JOBTYP_TAG? dJobTypTag = _JobTypTagRepo.GetDataByCondition(jtt => jtt.HJT_TAG_SN == dataId).FirstOrDefault();
            if (dJobTypTag != null)
            {
                _JobTypTagRepo.Remove(dJobTypTag);
                _JobTypTagRepo.SaveChange();
            }

            return new()
            {
                ID = dJobTypTag.HJT_TAG_SN,
                Name = dJobTypTag.HJT_TAG_NAM
            };
        }
    }
}
