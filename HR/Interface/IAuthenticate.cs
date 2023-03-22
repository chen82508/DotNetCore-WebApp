namespace HR.Interface
{
    public interface IAuthenticate<T>
    {
        /// <summary>
        /// 驗證手續
        /// </summary>
        /// <returns>通過回傳true</returns>
        bool FormDataAuthenticate(T formData);
    }
}
