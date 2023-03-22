using HR.Database;
using HR.DTOs.Frontend;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Areas.Frontend.Service
{
    public class ConsistencyTestService : IEntityService<ConsistencyTest>
    {
        private readonly HRSysContext _HRContext;
        private readonly IRepository<FE_CONSISTENCY_TEST> _FeConsistencyTest;

        public ConsistencyTestService(HRSysContext hRContext)
        {
            _HRContext = hRContext;
            _FeConsistencyTest = new HRGenericRepository<FE_CONSISTENCY_TEST>(_HRContext);
        }

        public ConsistencyTest AddData(ConsistencyTest entity)
        {
            FE_CONSISTENCY_TEST cONSISTENCY_TEST = new()
            {
                FCT_CST_RSM_ID = entity.Id,
                FCT_CST_STG_1 = entity.Choosen,
                FCT_CST_STG_2 = entity.Exculde,
                FCT_CST_STG_3 = entity.Sequence,
                FCT_CST_USE_SEC = entity.Seconds
            };

            _FeConsistencyTest.Insert(cONSISTENCY_TEST);
            _HRContext.SaveChanges();

            return entity;
        }

        public ConsistencyTest DeleteData(int dataId)
        {
            return new();
        }

        public void DeleteTestData(string Id)
        {
            FE_CONSISTENCY_TEST? dTestData = _FeConsistencyTest.GetDataByCondition(x => x.FCT_CST_RSM_ID == Id).FirstOrDefault();
            if (dTestData != null)
            {
                _FeConsistencyTest.Remove(dTestData);
            }

            _HRContext.SaveChanges();
        }

        public IEnumerable<ConsistencyTest> GetData()
        {
            return new List<ConsistencyTest>();
        }

        public ConsistencyTest GetData(string Id)
        {
            return _FeConsistencyTest.GetDataByCondition(x => x.FCT_CST_RSM_ID == Id).Select(x => new ConsistencyTest
                {
                    Id = x.FCT_CST_RSM_ID,
                    Choosen = x.FCT_CST_STG_1,
                    Exculde = x.FCT_CST_STG_2,
                    Sequence = x.FCT_CST_STG_3,
                    Seconds = x.FCT_CST_USE_SEC
                }).First();
        }

        public ConsistencyTest UpdateData(ConsistencyTest entity)
        {
            return entity;
        }
    }
}
