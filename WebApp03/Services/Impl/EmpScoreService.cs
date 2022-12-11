using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using WebApp03.Models;
using WebApp03.Repository;

namespace WebApp03.Services.Impl;

public class EmpScoreService: IEmpScoreService
{
    private readonly IEmpScoreDao _dao;
    private readonly IProxyService<EmpScore, long> _proxy;

    public DbContext CurrentContext => _proxy.CurrentContext;
    public IDbConnection DbConnection => _proxy.DbConnection;
    public DbContextTransaction DbContextTx => _proxy.DbContextTx;
    public DbTransaction DbTx => _proxy.DbTx;

    public EmpScoreService(IEmpScoreDao dao, IProxyService<EmpScore, long> proxy)
    {
        _dao = dao;
        _proxy = proxy;
    }
    
    
    public int Insert(EmpScore entity)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<EmpScore> SelectAll()
    {
        throw new System.NotImplementedException();
    }

    public EmpScore SelectById(long id)
    {
        throw new System.NotImplementedException();
    }

    public int Delete(long id)
    {
        throw new System.NotImplementedException();
    }

    public int Update(EmpScore entity)
    {
        throw new System.NotImplementedException();
    }

    public Page<IDictionary<string, object>> Page(int pageNo)
    {
        try
        {
            var page = new Page<IDictionary<string, object>>();
            // 獲取總資料筆數
            var counts = _dao.QueryTotalCounts();
            page.TotalCounts = counts;
            
            if (counts == 0)
            {
                return new Page<IDictionary<string, object>> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
            }
            
            // 獲取總頁數
            var pages = counts / Page<EmpScore>.PAGE_SIZE;
            if (counts % Page<EmpScore>.PAGE_SIZE != 0)
            {
                pages++;
            }
            page.PageTotal = pages;
        
            page.PageNo = pageNo;
        
            var data = _dao.Query4ItemsWithStats(page.PageNo);

            page.Items = data;
            return page;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}