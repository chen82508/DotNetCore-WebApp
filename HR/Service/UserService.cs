using HR.Database;
using HR.DTOs.HR;
using HR.Interface;
using HR.Models.Entity;
using HR.Models.Repository.Base;

namespace HR.Service
{
    public class UserService : IEntityService<UserInfo>
    {
        private readonly HRSysContext _HRContext;
        private readonly ICrypto _Crypto;
        private readonly IRepository<SY_USER> _UserRepo;
        public UserService(HRSysContext context, ICrypto crypto)
        {
            _HRContext = context;
            _UserRepo = new HRGenericRepository<SY_USER>(_HRContext);
            _Crypto = crypto;
        }

        public IEnumerable<UserInfo> GetData()
        {
            return new List<UserInfo>();
        }

        public UserInfo AddData(UserInfo entity)
        {
            SY_USER nUser = new()
            {
                SY_USR_ID = entity.ID,
                SY_USR_PWD = _Crypto.Encrypt(entity.Password),
                SY_USR_EML = entity.Email,
                SY_ACT_TYP = "A"
            };

            _UserRepo.Insert(nUser);
            _UserRepo.SaveChange();

            return new()
            {
                ID = nUser.SY_USR_ID,
                Password = nUser.SY_USR_PWD,
                Email = nUser.SY_USR_EML
            };
        }

        public UserInfo UpdateData(UserInfo entity)
        {
            return new();
        }

        public bool ModifyPassword(string userId, IFormCollection form)
        {
            if (userId == "admin")
            {
                return true;
            }

            SY_USER? user = _UserRepo.GetDataByCondition(u => u.SY_USR_ID == userId).FirstOrDefault();

            if (user != null)
            {
                string oldPwsd = $"{form["pwd"]}";
                if (oldPwsd != _Crypto.Decrypt(user.SY_USR_PWD))
                {
                    return false;
                }

                string newPwsd = $"{form["newPwd"]}";
                string cfmPwsd = $"{form["cfmPwd"]}";
                if (newPwsd != cfmPwsd)
                {
                    return false;
                }

                user.SY_USR_PWD = _Crypto.Encrypt(newPwsd);
                _UserRepo.Update(user);
                _UserRepo.SaveChange();

                return true;
            }
            return false;
        }

        public UserInfo DeleteData(int dataId)
        {
            return new();
        }
    }
}
