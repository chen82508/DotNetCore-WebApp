using System.Linq.Expressions;

namespace HR.Interface
{
    public interface IRepository<T>
    {
        #region 查詢資料
        /// <summary>
        /// 取得全部資料的 IQueryable。
        /// </summary>
        /// <returns>Entity 全部筆數的 IQueryable。</returns>
        IQueryable<T> GetAllData();
        /// <summary>
        /// 取得符合條件下的多筆資料。
        /// </summary>
        /// <param name="expression">要取得的Where條件。</param>
        /// <returns>取得符合條件的內容。</returns>
        IQueryable<T> GetDataByCondition(Expression<Func<T, bool>> expression);
        #endregion

        #region 新增資料
        /// <summary>
        /// 新增一筆資料。
        /// </summary>
        /// <param name="entity">要新增的 Entity 內容</param>
        void Insert(T entity);
        /// <summary>
        /// 新增多筆資料。
        /// </summary>
        /// <param name="entities">要新增的多個 Entity 內容</param>
        void InsertRange(IEnumerable<T> entities);
        #endregion

        #region 更新資料
        /// <summary>
        /// 更新一筆資料的內容。
        /// </summary>
        /// <param name="entity">要更新的 Entity 內容</param>
        void Update(T entity);
        /// <summary>
        /// 更新多筆資料的內容。
        /// </summary>
        /// <param name="entities">要更新的多個 Entity 內容</param>
        void UpdateRange(IEnumerable<T> entities);
        #endregion

        #region 移除資料
        /// <summary>
        /// 移除一筆資料的內容。
        /// </summary>
        /// <param name="entity">要移除的 Entity 內容</param>
        void Remove(T entity);
        /// <summary>
        /// 移除多筆資料的內容。
        /// </summary>
        /// <param name="entities">要移除的多個 Entity 內容</param>
        void RemoveRange(IEnumerable<T> entities);
        #endregion

        /// <summary> 
        /// 儲存異動。 
        /// </summary>
        void SaveChange();
    }
}
