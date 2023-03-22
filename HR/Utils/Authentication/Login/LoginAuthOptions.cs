namespace HR.Utils.Authentication.Login
{
    public class LoginAuthOptions
    {
        /// <summary>
        /// 登入頁路由
        /// </summary>
        public string LoginRoute { get; set; } = "/Login";
        /// <summary>
        /// 存入Session的鑰鍵名稱
        /// </summary>
        public string SessionKeyName { get; set; } = "uid";
        /// <summary>
        /// 返回URL引數名稱
        /// </summary>
        public string ReturnUrlKey { get; set; } = "return";
    }
}
