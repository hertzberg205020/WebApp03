using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApp03.Models;
using WebApp03.Tool;

namespace WebApp03.Repository.Impl
{
    public class EmpDao:IEmpDao
    {
        private readonly string _connectionStr = DbHelper.ConnString;
        private readonly IProxyDao<Emp, int> _proxy;
        const string TABLE_NAME = "Emp";
        public EmpDao(IProxyDao<Emp, int> proxy)
        {
            _proxy = proxy;
        }
        
        
        public DbContext CurrentContext => _proxy.CurrentContext;

        public IDbConnection DbConnection => _proxy.DbConnection;

        public DbContextTransaction DbContextTx => _proxy.DbContextTx;

        public DbTransaction DbTx => _proxy.DbTx;

        /// <summary>
        /// 使用Dapper增添一名員工
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public int Insert(Emp emp)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"insert into Emp values (@Name, @EmpNo)";
                    return db.Execute(sql, emp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }

        /// <summary>
        /// 以Dapper查詢所有員工數據
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Emp> SelectAll()
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"select id, Name, EmpNo from Emp;";
                    return db.Query<Emp>(sql);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }

        }

        /// <summary>
        /// 分頁使用，求總記錄比數
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int QueryTotalCounts()
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                var sql = $"SELECT count(*) FROM {TABLE_NAME}";
                return db.ExecuteScalar<int>(sql);
            }
        }

        /// <summary>
        /// 分頁使用，依頁碼選取數據
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Emp> Query4Items(int pageNo)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    int begin = (pageNo - 1) * Page<Emp>.PAGE_SIZE;
                    var sql = $@"
                            SELECT  Id, [Name], EmpNo
                            FROM     {TABLE_NAME}
                            ORDER BY Id 
                            OFFSET  @begin ROWS 
                            FETCH NEXT @pageSize ROWS ONLY ";
                    var parameters = new DynamicParameters();
                    parameters.Add("begin", begin);
                    parameters.Add("pageSize", Page<Emp>.PAGE_SIZE);
                    return db.Query<Emp>(sql, parameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }

        public List<Emp> SqlQuery(string strSql, params object[] sqlParameters)
        {
            throw new NotImplementedException();
        }
        

        public Emp SelectById(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"select Id, Name, EmpNo from Emp where Id = @id;";
                    return db.QueryFirstOrDefault<Emp>(sql, new { id = id });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }

        public int Delete(int id)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"delete from Emp where id = @id";
                    DynamicParameters dynamicParameters = new DynamicParameters();
                    dynamicParameters.Add("id", id);
                    return db.Execute(sql, dynamicParameters);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }

        public int Update(Emp emp)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"UPDATE Emp
                                    SET Name = @Name, EmpNo = @EmpNo
                                    WHERE Id = @Id";
                    return db.Execute(sql, emp);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }

        public int QueryCountsByName(string name)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    var sql = $@"SELECT COUNT(*) FROM Emp WHERE Name LIKE concat('%' ,@Name, '%')";
                    return db.ExecuteScalar<int>(sql, new { Name = name });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public IEnumerable<Emp> QueryItemsByName(int pageNo, string name)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    var begin = (pageNo - 1) * Page<Emp>.PAGE_SIZE;
                    var sql = $@"SELECT Id, EmpNo, Name
                                 FROM {TABLE_NAME}
                                 WHERE Name LIKE concat('%' ,@Name, '%')
                                 ORDER BY Id
                                 OFFSET {begin} ROWS
                                 FETCH NEXT {Page<Emp>.PAGE_SIZE} ROWS ONLY";
                    return db.Query<Emp>(sql, new { Name = name });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        public Emp SelectByEmpNo(string empNo)
        {
            using (IDbConnection db = new SqlConnection(_connectionStr))
            {
                try
                {
                    string sql = $@"select Id, Name, EmpNo from Emp where EmpNo = @empNo;";
                    return db.QueryFirstOrDefault<Emp>(sql, new { empNo = empNo });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw e;
                }
            }
        }
    }
}