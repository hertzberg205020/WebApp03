using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using WebApp03.Models;

namespace WebApp03.Repository.Impl;

public class EmpScoreDao: IEmpScoreDao
{
    private readonly IProxyDao<EmpScore, long> _proxy;
    
    public DbContext CurrentContext => _proxy.CurrentContext;

    public IDbConnection DbConnection => _proxy.DbConnection;

    public DbContextTransaction DbContextTx => _proxy.DbContextTx;

    public DbTransaction DbTx => _proxy.DbTx;

    public EmpScoreDao(IProxyDao<EmpScore, long> proxy)
    {
        _proxy = proxy;
    }
    public int Insert(EmpScore poco)
    {
        return _proxy.Insert(poco);
    }

    public int Delete(long id)
    {
        return _proxy.Delete(id);
    }
    
    public int MarkDeleted(long id)
    {
        try
        {
            var target = CurrentContext.Set<EmpScore>().Find(id);
            if (target == null)
            {
                throw new ArgumentException($"找不到id={id}的數據");
                // return -1;
            }

            target.IsDeleted = true;
            CurrentContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int Update(EmpScore poco)
    {
        try
        {
            var target = CurrentContext.Set<EmpScore>().Find(poco.Id);
            if (target == null)
            {
                throw new ArgumentException($"找不到id={poco.Id}的數據");
            }
            
            target.Course1 = poco.Course1;
            target.Course2 = poco.Course2;
            target.Course3 = poco.Course3;
            
            CurrentContext.SaveChanges();
            return 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public EmpScore SelectById(long id)
    {
        return _proxy.SelectById(id);
    }

    public EmpScore? FindById(long id)
    {
        var data = _proxy.SelectById(id);
        return data.IsDeleted ? null : data;
    }
    

    public IEnumerable<EmpScore> SelectAll()
    {
        return _proxy.SelectAll();
    }

    public int QueryTotalCounts()
    {
        return _proxy.QueryTotalCounts();
    }

    public IEnumerable<EmpScore> Query4Items(int pageNo)
    {
        return _proxy.Query4Items(pageNo);
    }

    public IEnumerable<IDictionary<string, object>> Query4ItemsWithStats(int pageNo)
    {
        try
        {
            var db = DbConnection as SqlConnection;
            var begin = (pageNo - 1) * Page<EmpScore>.PAGE_SIZE;
            var sql = @$"SELECT id Id, name [Name], course1 [Course1], course2 [Course2], course3 [Course3], (course1 + course2 + course3) AS [TotalScore],
                                RANK() OVER (ORDER BY (course1 + course2 + course3) DESC) AS [RankNo],
                                CAST(((course1 + course2 + course3) / 3.0) AS DECIMAL(7, 2)) AS [AverageScore]
                         FROM t_emp_score
                         ORDER BY (course1 + course2 + course3) DESC,
                                  course1 DESC, course2 DESC , course3 DESC ,id ASC 
                         OFFSET {begin} ROWS
                         FETCH NEXT {Page<EmpScore>.PAGE_SIZE} ROWS ONLY;";
            
            // DynamicParameters parameters = new DynamicParameters();
            
            // parameters.Add("empId", empId);
            
            // var res = db.Query(sql, parameters);
            var res = db.Query(sql);

            return res.Select(item => item as IDictionary<string, object>).ToList();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            DbConnection.Close();
        }
    }

    public List<EmpScore> SqlQuery(string strSql, params object[] sqlParameters)
    {
        return _proxy.SqlQuery(strSql, sqlParameters);
    }
}