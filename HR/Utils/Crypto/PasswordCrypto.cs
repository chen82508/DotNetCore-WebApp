using HR.Interface;
using HR.Utils.Crypto.Base;
using System.Security.Cryptography;
using System.Text;

namespace HR.Utils.Crypto
{
    public class PasswordCrypto : ICrypto
    {
        private readonly AES _BaseAES;
        private readonly IConfiguration _Configuration;

        private string Sha256Key
        {
            get { return _Configuration.GetValue<string>("Sha256"); }
        }

        public PasswordCrypto(IConfiguration configuration)
        {
            _Configuration = configuration;
            _BaseAES = new(Sha256Key);
        }

        public string Decrypt(string CipherText)
        {
            byte[] CipherTextBytes = Convert.FromBase64String(CipherText);
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _BaseAES.Key;
            aes.IV = _BaseAES.IV;

            ICryptoTransform transform = aes.CreateDecryptor();
            return Encoding.UTF8.GetString(transform.TransformFinalBlock(CipherTextBytes, 0, CipherTextBytes.Length));
        }

        public string Encrypt(string ClearText)
        {
            byte[] ClearTextBytes = Encoding.UTF8.GetBytes(ClearText);
            Aes aes = Aes.Create();
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.Key = _BaseAES.Key;
            aes.IV = _BaseAES.IV;

            ICryptoTransform transform = aes.CreateEncryptor();
            return Convert.ToBase64String(transform.TransformFinalBlock(ClearTextBytes, 0, ClearTextBytes.Length));
        }
    }
}
