using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using WebApp03.Models;
using WebApp03.Tool;

namespace WebApp03.Repository
{
    public interface ICoreDao<E, I> where E : BaseEntity<I>
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
        /// 新增紀錄
        /// </summary>
        /// <param name="poco">實體類物件</param>
        /// <returns></returns>
        int Insert(E poco);

        /// <summary>
        /// 依主鍵刪除數據紀錄
        /// </summary>
        /// <param name="id">主鍵</param>
        /// <returns></returns>
        int Delete(I id);

        /// <summary>
        /// 跟新數據庫紀錄
        /// </summary>
        /// <param name="poco"></param>
        /// <returns></returns>
        int Update(E poco);

        /// <summary>
        /// 依主鍵查詢對應數據紀錄
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        E SelectById(I id);

        /// <summary>
        /// 查詢表中全部數據
        /// </summary>
        /// <returns></returns>
        IEnumerable<E> SelectAll();

        /// <summary>
        /// 查詢表中資料筆數
        /// </summary>
        /// <returns></returns>
        int QueryTotalCounts();

        /// <summary>
        /// 查詢對應分頁中資料的數據
        /// </summary>
        /// <param name="pageNo"></param>
        /// <returns></returns>
        IEnumerable<E> Query4Items(int pageNo);

        /// <summary>  
        /// 原生SQL查詢
        /// </summary>  
        /// <param name="strSql"></param>  
        /// <param name="sqlParameter">SqlParameter</param>  
        /// <returns></returns>  
        public List<E> SqlQuery(string strSql, params Object[] sqlParameters);
        
    }
}