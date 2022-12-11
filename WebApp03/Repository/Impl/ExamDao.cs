using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Web.UI.WebControls.Expressions;
using AutoMapper;
using Dapper;
using WebApp03.Mappings;
using WebApp03.Models;

namespace WebApp03.Repository.Impl
{
    public class ExamDao: IExamDao
    {
        // private readonly ExamDbContext _db;
        // private readonly IProxyDao<Exam, int> _proxy = new ProxyDao<Exam, int>(new DbFactory());
        private readonly IProxyDao<Exam, int> _proxy;
        // public ExamDao(ExamDbContext context)
        // {
        //     _db = context;
        // 
        public ExamDao(IProxyDao<Exam, int> proxy)
        {
            _proxy = proxy;
        }

        public DbContext CurrentContext => _proxy.CurrentContext;

        public IDbConnection DbConnection => _proxy.DbConnection;

        public DbContextTransaction DbContextTx => _proxy.DbContextTx;

        public DbTransaction DbTx => _proxy.DbTx;

        public int Insert(Exam entity)
        {
            return _proxy.Insert(entity);
        }

        public int Delete(int id)
        {
            return _proxy.Delete(id);
        }

        public int Update(Exam exam)
        {
            try
            {
                var target = CurrentContext.Set<Exam>().Find(exam.Id);
                if (target != null && exam.Score >= 0)
                {
                    target.Score = exam.Score;
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

        public Exam SelectById(int id)
        {
            return _proxy.SelectById(id);
        }

        public IEnumerable<Exam> SelectAll()
        {
            return _proxy.SelectAll();
        }

        public int QueryTotalCounts()
        {
            return _proxy.QueryTotalCounts();
        }

        public IEnumerable<Exam> Query4Items(int pageNo)
        {
            return _proxy.Query4Items(pageNo);
        }

        public List<Exam> SqlQuery(string strSql, params object[] sqlParameters)
        {
            return _proxy.SqlQuery(strSql, sqlParameters);
        }

        public Exam? SelectByEmpIdAndSubjectId(int empId, int subjectId)
        {
            var data = CurrentContext.Set<Exam>()
                .FirstOrDefault(e => e.EmpId == empId
                                     && e.SubjectId == subjectId);
            return data;
        }

        /// <summary>
        /// 依據員工主鍵查找考試資料
        /// https://igouist.github.io/post/2020/07/automapper/
        /// </summary>
        /// <param name="empId">員工主鍵値</param>
        /// <returns></returns>
        public IQueryable<ExamModel> FindByEmpId(int empId)
        {
            var data = CurrentContext.Set<Exam>()
                .AsNoTracking()
                .Include(e => e.Emp)
                .Include(e => e.Subject)
                .Where(e => e.EmpId == empId);
            
            var config = new MapperConfiguration(cfg => cfg.AddProfile<DaoMappings>());
            var mapper = config.CreateMapper(); // 用設定檔建立 Mapper
            var result = mapper.Map<IEnumerable<ExamModel>>(data); // 轉換型別
            
            return result.AsQueryable();
        }

        public IQueryable<ExamModel> FindBySubjectId(int subjectId)
        {
            var res = CurrentContext.Set<Exam>().AsNoTracking()
                .Include(e => e.Subject)
                .Include(e => e.Emp)
                .Where(e => e.SubjectId == subjectId)
                .Select(e => new ExamModel
                {
                    Id = e.Id,
                    Score = e.Score,
                    EmpId = e.EmpId,
                    EmpName = e.Emp.Name,
                    EmpNo = e.Emp.EmpNo,
                    SubjectId = e.SubjectId,
                    SubjectName = e.Subject.Name
                });
            return res;
        }

        public IQueryable<ExamModel> FindBySubjectIdAndEmpIdList(int subjectId, List<int> empIds)
        {
            // https://ithelp.ithome.com.tw/articles/10104729
            var res = from e in CurrentContext.Set<Exam>()
                where empIds.Contains(e.EmpId) && e.SubjectId == subjectId
                orderby e.Emp.Id
                select new ExamModel
                {
                    Id = e.Id,
                    Score = e.Score,
                    EmpId = e.EmpId,
                    EmpName = e.Emp.Name,
                    SubjectId = e.SubjectId,
                    SubjectName = e.Subject.Name
                };
            return res;
        }

        public IEnumerable<ExamModel> QueryItemsBySubjectId(int pageNo, int subjectId)
        {
            var pageSize = Page<ExamModel>.PAGE_SIZE;
            var begin = (pageNo - 1) * pageSize;
            var data = this.FindBySubjectId(subjectId);
            return data
                .OrderByDescending(e => e.Score)
                .ThenBy(e => e.EmpId)
                .Skip(begin)
                .Take(pageSize)
                .ToList();
        }

        public IEnumerable<Emp> FindValidEmpList(int subjectId)
        {
            var db = DbConnection;
            try
            {
                db.Open();
                var sql = @"SELECT id, name, empno
                            FROM Emp
                            WHERE Id not in (SELECT EmpId 
                                             FROM Exam WHERE SubjectId = @SubjectId);";
                var parameters = new DynamicParameters();
                parameters.Add("@SubjectId", subjectId);
                var data =db.Query<Emp>(sql, parameters);
                return data;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                db.Close();
            }
        }

        public int QueryCountsBySubjectId(int subjectId)
        {
            var data = CurrentContext.Set<Exam>().Count(e => e.SubjectId == subjectId);
            return data;
        }

        public IEnumerable<IDictionary<string, object>> QueryItemsByEmpId(int pageNo, int empId)
        {
            var db = DbConnection;
            try
            {
                db.Open();
                var begin = (pageNo - 1) * Page<Emp>.PAGE_SIZE;
                var sql = @$"SELECT t1.EmpId AS EmpId, e.Name AS EmpName, t1.Score AS Score,
                                    t1.rank_no AS [Rank], s.Name AS SubjectName, s.Id AS SubjectId, 
                                    (SELECT COUNT(1) FROM Exam WHERE Exam.SubjectId = s.Id) AS [TotalNum]
                             FROM (SELECT EmpId, SubjectId, Score,  
                                          RANK() OVER (PARTITION BY SubjectId 
                                                       ORDER BY Score DESC) AS rank_no
                                   FROM Exam) AS t1 
                             JOIN Emp e
                                 ON e.Id = t1.EmpId 
                             JOIN Subject s
                                 ON s.Id = t1.SubjectId
                             WHERE EmpId = @empId
                             ORDER BY s.Id
                             OFFSET {begin} ROWS
                             FETCH NEXT {Page<Exam>.PAGE_SIZE} ROWS ONLY;";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("empId", empId);
                var res = db.Query(sql, parameters);
                List<IDictionary<string, object>> ret = new List<IDictionary<string, object>>();
                foreach (var item in res)
                {
                    var data = item as IDictionary<string, object>;
                    ret.Add(data);
                }

                return ret;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                // 不要用using，不可以調用到Depose()
                db.Close();
            }
        }

        public int QueryCountsByEmpId(int empId)
        {
            return CurrentContext.Set<Exam>().Count(e => e.EmpId == empId);
        }
    }
}