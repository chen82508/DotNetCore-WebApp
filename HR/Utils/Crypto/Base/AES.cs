using System.Security.Cryptography;
using System.Text;

namespace HR.Utils.Crypto.Base
{
    public class AES
    {
        private readonly string _HashSource;

        public AES(string hashSrc)
        {
            _HashSource = hashSrc;
        }

        /// <summary>
        /// 經過SHA256 hash處理的AES金鑰
        /// </summary>
        public byte[] Key
        {
            get
            {
                SHA256 sha256 = SHA256.Create();
                return sha256.ComputeHash(Encoding.UTF8.GetBytes(_HashSource));
            }
        }
        /// <summary>
        /// 經過MD5 hash處理的AES初始向量
        /// </summary>
        public byte[] IV
        {
            get
            {
                MD5 md5 = MD5.Create();
                return md5.ComputeHash(Encoding.UTF8.GetBytes(_HashSource));
            }
        }
    }
}
