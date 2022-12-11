using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using WebApp03.Repository;

namespace WebApp03.Services.Impl;

public class ProxyService<E, I>: IProxyService<E, I>
{
    private IDbContextHolder _dbCtxHolder;
    public DbContext CurrentContext => _dbCtxHolder.getCurrentCtx();

    public IDbConnection DbConnection => CurrentContext.Database.Connection;

    public DbContextTransaction DbContextTx => CurrentContext.Database.BeginTransaction();

    public DbTransaction DbTx => CurrentContext.Database.Connection.BeginTransaction();
    public int Insert(E entity)
    {
        throw new System.NotImplementedException();
    }

    public IEnumerable<E> SelectAll()
    {
        throw new System.NotImplementedException();
    }

    public E SelectById(I id)
    {
        throw new System.NotImplementedException();
    }

    public int Delete(I id)
    {
        throw new System.NotImplementedException();
    }

    public int Update(E entity)
    {
        throw new System.NotImplementedException();
    }

    public ProxyService(IDbContextHolder dbCtxHolder)
    {
        _dbCtxHolder = dbCtxHolder;
    }
    
    
}