using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.HR.Service
{
    public class JobTypeService : IEntityService<JobType>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<HR_JOBTYP> _JobTypeRepo;
        private readonly IRepository<HR_JOBTYP_TAG> _JobTypTagRepo;

        public JobTypeService(HRSysContext context)
        {
            _HRContext = context;
            _JobTypeRepo = new HRGenericRepository<HR_JOBTYP>(_HRContext);
            _JobTypTagRepo = new HRGenericRepository<HR_JOBTYP_TAG>(_HRContext);
        }

        public IEnumerable<JobType> GetData()
        {
            return _JobTypeRepo.GetDataByCondition(jt => jt.HJT_ACT_TYP == "A")
                .Select(jt => new JobType
                {
                    TID = jt.HJT_JOB_TAG_SN,
                    ID = jt.HJT_JOB_SN,
                    Name = jt.HJT_JOB_NAM
                });
        }

        public JobType AddData(JobType entity)
        {
            HR_JOBTYP jobType = new()
            {
                HJT_JOB_TAG_SN = entity.TID,
                HJT_JOB_NAM = entity.Name,
                HJT_ACT_TYP = "A",
                HJT_JOB_ADD_DAT = DateTime.Now,
                HJT_JOB_UPD_DAT = DateTime.Now
            };
            _JobTypeRepo.Insert(jobType);
            _JobTypeRepo.SaveChange();

            return new()
            {
                TID = jobType.HJT_JOB_TAG_SN,
                ID = jobType.HJT_JOB_SN,
                Name = jobType.HJT_JOB_NAM
            };
        }

        public JobType UpdateData(JobType entity)
        {
            HR_JOBTYP? mJobTyp = _JobTypeRepo.GetDataByCondition(jt => jt.HJT_JOB_SN == entity.ID).FirstOrDefault();
            if (mJobTyp != null)
            {
                mJobTyp.HJT_JOB_NAM = entity.Name;
                mJobTyp.HJT_JOB_UPD_DAT = DateTime.Now;
                _JobTypeRepo.Update(mJobTyp);
                _JobTypeRepo.SaveChange();
            }

            HR_JOBTYP_TAG? mJobTypTag = _JobTypTagRepo.GetDataByCondition(jtt => jtt.HJT_TAG_SN == entity.TID).FirstOrDefault();
            if (mJobTypTag != null)
            {
                mJobTypTag.HJT_TAG_UPD_DAT = DateTime.Now;
                _JobTypTagRepo.Update(mJobTypTag);
                _JobTypTagRepo.SaveChange();
            }

            return new()
            {
                TID = entity.TID,
                ID = entity.ID,
                Name = mJobTyp.HJT_JOB_NAM
            };
        }

        public JobType DeleteData(int dataId)
        {
            HR_JOBTYP? dJobTyp = _JobTypeRepo.GetDataByCondition(jt => jt.HJT_JOB_SN == dataId).FirstOrDefault();
            if (dJobTyp != null)
            {
                _JobTypeRepo.Remove(dJobTyp);
                _JobTypeRepo.SaveChange();
            }

            return new()
            {
                TID = dJobTyp.HJT_JOB_TAG_SN,
                ID = dJobTyp.HJT_JOB_SN,
                Name = dJobTyp.HJT_JOB_NAM
            };
        }
    }
}
