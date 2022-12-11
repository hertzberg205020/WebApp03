using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using WebApp03.Models;
using WebApp03.Tool;

namespace WebApp03.Repository.Impl;

/// <summary>
/// ICoreDao 的代理用實現類，
/// 千萬不要用using 包住 DbContext
/// </summary>
/// <typeparam name="E">實體類別</typeparam>
/// <typeparam name="I">主健型別</typeparam>
public class ProxyDao<E, I>: IProxyDao<E, I> where E : BaseEntity<I>
{
    // 每次request的dbConnection都要是新的
    private IDbContextHolder _dbCtxHolder;
    public DbContext CurrentContext
    {
        get
        {
            return _dbCtxHolder.getCurrentCtx();
        }
    }


    public ProxyDao(IDbContextHolder dbCtxHolder)
    {
        _dbCtxHolder = dbCtxHolder;
    }

    public IDbConnection DbConnection => CurrentContext.Database.Connection;

    public DbContextTransaction DbContextTx => CurrentContext.Database.BeginTransaction();

    public DbTransaction DbTx
    {
        get
        {
            return CurrentContext.Database.Connection.BeginTransaction();
        }
    }


    public int Insert(E poco)
    {
        try
        {
            CurrentContext.Set<E>().Add(poco);
            CurrentContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return 0;
        }
        
    }

    public int Delete(I id)
    {
        try
        {
            var target = CurrentContext.Set<E>().Find(id);
            if (target == null) return 0;
            CurrentContext.Set<E>().Remove(target);
            CurrentContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return -1;
        }
    }

    public int Update(E poco)
    {
        throw new NotImplementedException();
    }

    public E SelectById(I id)
    {
        return CurrentContext.Set<E>().Find(id);
    }

    public IEnumerable<E> SelectAll()
    {
        return CurrentContext.Set<E>().ToList();  
    }

    public int QueryTotalCounts()
    {
        return CurrentContext.Set<E>().Count();
    }

    public IEnumerable<E> Query4Items(int pageNo)
    {
        var pageSize = Page<E>.PAGE_SIZE;
        var begin = (pageNo - 1) * pageSize;
        return CurrentContext.Set<E>().OrderBy(e => e.Id)
            .Skip(begin)
            .Take(pageSize);
    }

    public List<E> SqlQuery(string strSql, params object[] sqlParameters)
    {
        sqlParameters ??= Array.Empty<object>();
        return this.CurrentContext.Database.SqlQuery<E>(strSql, sqlParameters).ToList();
    }
}