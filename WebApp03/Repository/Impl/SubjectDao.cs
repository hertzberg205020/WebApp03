using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using WebApp03.Models;

namespace WebApp03.Repository.Impl
{
    public class SubjectDao: ISubjectDao
    {
        // DbContext是Singleton
        // private readonly ExamDbContext _db;
        
        // private readonly ICoreDao<Subject, int> _proxy = new CoreDao<Subject, int>(new DbFactory());
        private readonly IProxyDao<Subject, int> _proxy;
        
        // public SubjectDao(ExamDbContext context)
        // {
        //     _db = context;
        // }
        
        public SubjectDao(IProxyDao<Subject, int> proxy)
        {
            _proxy = proxy;
        }

        public DbContext CurrentContext => _proxy.CurrentContext;

        public IDbConnection DbConnection => _proxy.DbConnection;

        public DbContextTransaction DbContextTx => _proxy.DbContextTx;

        public DbTransaction DbTx => _proxy.DbTx;

        public int Insert(Subject subject)
        {
            // try
            // {
            //     _db.Set<Subject>().Add(subject);
            //     _db.SaveChanges();
            //     return 1;
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return 0;
            // }
            return _proxy.Insert(subject);
        }

        public int Delete(int id)
        {
            // try
            // {
            //     var target = _db.Set<Subject>().Find(id);
            //     if (target != null)
            //     {
            //         _db.Set<Subject>().Remove(target);
            //         _db.SaveChanges();
            //         return 1;
            //     }
            //     return 0;
            // }
            // catch (Exception e)
            // {
            //     Console.WriteLine(e);
            //     return -1;
            // }
            return _proxy.Delete(id);
        }

        public int Update(Subject subject)
        {
            
            try
            {
                var target = CurrentContext.Set<Subject>().Find(subject.Id);
                if (target != null && !string.IsNullOrEmpty(subject.Name))
                {
                    target.Name = subject.Name;
                    CurrentContext.SaveChanges();
                    return 1;
                }

                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public Subject SelectById(int id)
        {
            
            // return _db.Set<Subject>().Find(id);
            return _proxy.SelectById(id);
        }

        public IEnumerable<Subject> SelectAll()
        {
            
            // return _db.Set<Subject>();
            return _proxy.SelectAll();
        }

        public int QueryTotalCounts()
        {
            // return _db.Set<Subject>().Count();
            return _proxy.QueryTotalCounts();
        }

        public IEnumerable<Subject> Query4Items(int pageNo)
        {
            
            // var pageSize = Page<Subject>.PAGE_SIZE;
            // var begin = (pageNo - 1) * pageSize;
            // return _db.Set<Subject>().OrderBy(e => e.Id)
            //     .Skip(begin)
            //     .Take(pageSize)
            //     .ToList();
            return _proxy.Query4Items(pageNo);
        }

        public List<Subject> SqlQuery(string strSql, params object[] sqlParameters)
        {
            return _proxy.SqlQuery(strSql, sqlParameters);
        }

        public Subject SelectByName(string name)
        {
            
            return CurrentContext.Set<Subject>().FirstOrDefault(s => s.Name.Contains(name));
        }

        public int QueryCountsByName(string name)
        {
            
            return CurrentContext.Set<Subject>().Count(s => s.Name.Contains(name));
        }

        public IEnumerable<Subject> QueryItemsByName(int pageNo, string name)
        {
            
            var pageSize = Page<Subject>.PAGE_SIZE;
            var begin = (pageNo - 1) * pageSize;
            return CurrentContext.Set<Subject>().AsNoTracking()
                .Include(s => s.Exams)
                .Where(s => s.Name.Contains(name))
                .OrderBy(s => s.Id)
                .Skip(begin)
                .Take(pageSize)
                .ToList();
        }
    }
}