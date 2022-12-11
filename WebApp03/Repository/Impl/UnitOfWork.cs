using System;
using System.Collections;
using System.Data.Entity;
using WebApp03.Models;

namespace WebApp03.Repository.Impl;

/// <summary>
/// 實作Entity Framework Unit Of Work的class
/// 可能不太實用
/// 每個DAL都會有還會有自定義的方法
/// </summary>
public class UnitOfWork<E, I> where E: BaseEntity<I>
{
    private readonly DbContext _context;
 
    private bool _disposed;
    private Hashtable _repositories;
 
    /// <summary>
    /// 設定此Unit of work(UOF)的Context。
    /// </summary>
    /// <param name="context">設定UOF的context</param>
    public UnitOfWork(DbContext context)
    {
        _context = context;
    }
 
    /// <summary>
    /// 儲存所有異動。
    /// </summary>
    public void Save()
    {
        _context.SaveChanges();       
    }
 
    /// <summary>
    /// 清除此Class的資源。
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
 
    /// <summary>
    /// 清除此Class的資源。
    /// </summary>
    /// <param name="disposing">是否在清理中？</param>
    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
 
        _disposed = true;
    }
 
    /// <summary>
    /// 取得某一個Entity的Repository。
    /// 如果沒有取過，會initialise一個
    /// 如果有就取得之前initialise的那個。
    /// </summary>
    /// <typeparam name="E">此Context裡面的Entity Type</typeparam>
    /// <returns>Entity的Repository</returns>
    public ICoreDao<E, I> Repository<E>() where E : BaseEntity<I>
    {
        if (_repositories == null)
        {
            _repositories = new Hashtable();
        }
 
        var type = typeof(E).Name;
 
        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(ICoreDao<E, I>);
 
            var repositoryInstance =
                Activator.CreateInstance(repositoryType
                    .MakeGenericType(typeof(E)), _context);
 
            _repositories.Add(type, repositoryInstance);
        }
 
        return (ICoreDao<E, I>)_repositories[type];
    }
}