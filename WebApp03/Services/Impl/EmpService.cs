using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp03.Models;
using WebApp03.Repository.Impl;
using WebApp03.Repository;

namespace WebApp03.Services.Impl
{
    public class EmpService: IEmpService
    {
        private readonly IEmpDao _dao;
        private readonly IProxyService<Emp, int> _proxy;

        public EmpService(IEmpDao dao, IProxyService<Emp, int> proxy)
        {
            _dao = dao;
            _proxy = proxy;
        }

        public DbContext CurrentContext => _proxy.CurrentContext;

        public IDbConnection DbConnection => _proxy.DbConnection;

        public DbContextTransaction DbContextTx => _proxy.DbContextTx;

        public DbTransaction DbTx => _proxy.DbTx;

        public int Insert(Emp emp)
        {
            try
            {
                return _dao.Insert(emp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<Emp> SelectAll()
        {
            try
            {
                return _dao.SelectAll();
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        
        /// <summary>
        /// 依頁數查詢頁面訊息，包含頁數、總頁數、總資料筆數、當前頁員工數據
        /// </summary>
        /// <param name="pageNo">頁碼</param>
        /// <returns></returns>
        public Page<Emp> Page(int pageNo)
        {
            Page<Emp> page = new Page<Emp>();
            // 獲取總資料筆數
            var counts = _dao.QueryTotalCounts();
            page.TotalCounts = counts;
            // 獲取總頁數
            var pages = counts / Page<Emp>.PAGE_SIZE;
            if (counts % Page<Emp>.PAGE_SIZE != 0)
            {
                pages++;
            }
            page.PageTotal = pages;
            
            page.PageNo = pageNo;

            // 查詢數據資料
            page.Items = _dao.Query4Items(page.PageNo);
            return page;
        }
        
        

        public Emp SelectById(int id)
        {
            try
            {
                return _dao.SelectById(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Delete(int id)
        {
            try
            {
                return _dao.Delete(id);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Update(Emp emp)
        {
            try
            {
                return _dao.Update(emp);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public Page<Emp> Page(int pageNo, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return this.Page(pageNo);
            }
            Page<Emp> page = new Page<Emp>();
            // 獲取總資料筆數
            var counts = _dao.QueryCountsByName(name);
            page.TotalCounts = counts;
            
            if (counts == 0)
            {
                return new Page<Emp> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
            }
            
            // 獲取總頁數
            var pages = counts / Page<Emp>.PAGE_SIZE;
            if (counts % Page<Emp>.PAGE_SIZE != 0)
            {
                pages++;
            }
            page.PageTotal = pages;
            
            // PageNo Property有在set作保護
            page.PageNo = pageNo;

            // 查詢數據資料
            page.Items = _dao.QueryItemsByName(page.PageNo, name);
            return page;
        }
    }
}