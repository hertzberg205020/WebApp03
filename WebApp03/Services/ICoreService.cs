using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using WebApp03.Models;

namespace WebApp03.Services
{
    /// <summary>
    /// 通用業務邏輯
    /// </summary>
    /// <typeparam name="E">實體類別</typeparam>
    /// <typeparam name="I">主鍵</typeparam>
    public interface ICoreService<E, I> 
    {
        /// <summary>
        /// DbContext物件: Entity Framework連線物件
        /// </summary>
        DbContext CurrentContext{ get;}
        
        /// <summary>
        /// Dapper連線物件
        /// </summary>
        IDbConnection DbConnection { get; }
        
        /// <summary>
        /// DbContext交易物件
        /// Entity Framework6用
        /// </summary>
        DbContextTransaction DbContextTx { get; }
        
        /// <summary>
        /// DbConnection交易物件
        /// Dapper用
        /// </summary>
        DbTransaction DbTx { get; }
        
        /// <summary>
        /// 新增數據 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Insert(E entity);
        
        /// <summary>
        /// 查詢全部數據 
        /// </summary>
        /// <returns></returns>
        IEnumerable<E> SelectAll();
        
        // Page<E> Page(int pageNo);
        /// <summary>
        /// 依主鍵查找紀錄
        /// </summary>
        /// <param name="id">主鍵值</param>
        /// <returns></returns>
        E SelectById(I id);
        
        /// <summary>
        /// 依主鍵刪除紀錄
        /// </summary>
        /// <param name="id">主鍵值</param>
        /// <returns></returns>
        int Delete(I id);
        
        /// <summary>
        /// 更新數據
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        int Update(E entity);
        
    }
}