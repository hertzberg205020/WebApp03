using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using AutoMapper;
using WebApp03.Mappings;
using WebApp03.Models;
using WebApp03.Repository;

namespace WebApp03.Services.Impl;

public class ExamService: IExamService
{
    private readonly IProxyService<Exam, int> _proxy;
    private readonly IExamDao _examDao;
    private IEmpDao _empDao;
    private ISubjectDao _subjectDao;
    
    public DbContext CurrentContext => _proxy.CurrentContext;

    public IDbConnection DbConnection => _proxy.DbConnection;

    public DbContextTransaction DbContextTx => _proxy.DbContextTx;

    public DbTransaction DbTx => _proxy.DbTx;

    public ExamService(IProxyService<Exam, int> proxy, IExamDao examDao, IEmpDao empDao, ISubjectDao subjectDao)
    {
        _proxy = proxy;
        _examDao = examDao;
        _empDao = empDao;
        _subjectDao = subjectDao;
    }

    public int Insert(Exam exam)
    {
        if (_examDao.SelectByEmpIdAndSubjectId(exam.EmpId, exam.SubjectId) != null)
        {
            throw new ArgumentException("成績重複登記");
        }

        try
        {
            return _examDao.Insert(exam);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int Insert(ExamModel examModel)
    {
        Emp emp = _empDao.SelectByEmpNo(examModel.EmpNo);
        return this.Insert(new Exam{EmpId = emp.Id, 
                                    SubjectId = examModel.SubjectId, 
                                    Score = examModel.Score});
    }

    public IEnumerable<Exam> SelectAll()
    {
        try
        {
            return _proxy.SelectAll();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Exam SelectById(int id)
    {
        try
        {
            return _proxy.SelectById(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int Delete(int id)
    {
        try
        {
            return _examDao.Delete(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int Update(Exam entity)
    {
        try
        {
            return _examDao.Update(entity);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public int Update(ExamModel examModel)
    {
        return this.Update(new Exam { Id = examModel.Id, Score = examModel.Score });
    }

    public Page<IDictionary<string, object>> PageByEmpId(int pageNo, int empId)
    {
        try
        {
            var page = new Page<IDictionary<string, object>>();
            // 獲取總資料筆數
            var counts = _examDao.QueryCountsByEmpId(empId);
            page.TotalCounts = counts;
            
            if (counts == 0)
            {
                return new Page<IDictionary<string, object>> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
            }
            
            // 獲取總頁數
            var pages = counts / Page<Exam>.PAGE_SIZE;
            if (counts % Page<Exam>.PAGE_SIZE != 0)
            {
                pages++;
            }
            page.PageTotal = pages;
        
            page.PageNo = pageNo;
        
            var data = _examDao.QueryItemsByEmpId(page.PageNo, empId);

            page.Items = data;
            return page;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
    }

    public Page<ExamModel> Page(int pageNo, int subjectId)
    {
        try
        {
            var page = new Page<ExamModel>();
            // 獲取總資料筆數
            var counts = _examDao.QueryCountsBySubjectId(subjectId);
            page.TotalCounts = counts;
            
            if (counts == 0)
            {
                return new Page<ExamModel> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
            }
            
            // 獲取總頁數
            var pages = counts / Page<ExamModel>.PAGE_SIZE;
            if (counts % Page<ExamModel>.PAGE_SIZE != 0)
            {
                pages++;
            }
            page.PageTotal = pages;
        
            page.PageNo = pageNo;
        
            var data = _examDao.QueryItemsBySubjectId(page.PageNo, subjectId);

            page.Items = data;
            return page;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public IEnumerable<Emp> FindValidEmpList(int subjectId)
    {
        return _examDao.FindValidEmpList(subjectId);
    }
}