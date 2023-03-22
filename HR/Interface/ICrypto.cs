namespace HR.Interface
{
    public interface ICrypto
    {
        /// <summary>
        /// 明文加密
        /// </summary>
        /// <param name="ClearText">明文</param>
        /// <returns></returns>
        public string Encrypt(string ClearText);
        /// <summary>
        /// 密文解密
        /// </summary>
        /// <param name="CipherText">密文</param>
        /// <returns></returns>
        public string Decrypt(string CipherText);
    }
}
