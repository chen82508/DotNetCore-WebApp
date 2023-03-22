namespace HR.Interface
{
    public interface IEntityService<T>
    {
        /// <summary>
        /// 取得資料
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> GetData();
        /// <summary>
        /// 新增資料
        /// </summary>
        /// <param name="entity">資料實體</param>
        /// <returns></returns>
        T AddData(T entity);
        /// <summary>
        /// 更新資料
        /// </summary>
        /// <param name="entity">資料實體</param>
        /// <returns></returns>
        T UpdateData(T entity);
        /// <summary>
        /// 刪除資料
        /// </summary>
        /// <param name="entity">資料實體</param>
        /// <returns></returns>
        T DeleteData(int dataId);
    }
}
