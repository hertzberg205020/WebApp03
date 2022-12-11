using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using AutoMapper;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;

namespace WebApp03.Services.Impl;

public class SubjectService: ISubjectService
{
    private readonly IProxyService<Subject, int> _proxy;
    private readonly ISubjectDao _dao;

    public DbContext CurrentContext => _proxy.CurrentContext;

    public IDbConnection DbConnection => _proxy.DbConnection;

    public DbContextTransaction DbContextTx => _proxy.DbContextTx;

    public DbTransaction DbTx => _proxy.DbTx;
    
    public SubjectService(ISubjectDao subjectDao, IProxyService<Subject, int> proxy)
    {
        _dao = subjectDao;
        _proxy = proxy;
    }
    public int Insert(Subject subject)
    {
        try
        {
            return _dao.Insert(subject);
        }
        catch (Exception e)
        {
            throw e;
        }
    }

    public IEnumerable<Subject> SelectAll()
    {
        try
        {
            return _dao.SelectAll();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// 獲取當前頁面訊息
    /// 此處使用AutoMapper轉換DTO與ViewModel
    /// https://igouist.github.io/post/2020/07/automapper/
    /// </summary>
    /// <param name="pageNo">當前頁碼</param>
    /// <returns>頁面資訊</returns>
    public Page<SubjectModel> Page(int pageNo)
    {
        var page = new Page<SubjectModel>();
        // 獲取總資料筆數
        var counts = _dao.QueryTotalCounts();
        page.TotalCounts = counts;
        // 獲取總頁數
        var pages = counts / Page<SubjectModel>.PAGE_SIZE;
        if (counts % Page<SubjectModel>.PAGE_SIZE != 0)
        {
            pages++;
        }
        page.PageTotal = pages;
            
        page.PageNo = pageNo;

        // 查詢數據資料
        var data = _dao.Query4Items(page.PageNo);
        
        // AutoMapper轉換DTO, viewModel
        // https://igouist.github.io/post/2020/07/automapper/
        var config = new MapperConfiguration(cfg => 
            cfg.CreateMap<Subject, SubjectModel>()); // 註冊Model間的對映
        var mapper = config.CreateMapper(); // 建立 Mapper
        var result = mapper.Map<IEnumerable<SubjectModel>>(data); // 轉換型別

        page.Items = result;
        return page;
    }

    public Page<Subject> PageWithEntity(int pageNo, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return this.PageWithEntity(pageNo);
        }
        var page = new Page<Subject>();
        // 獲取總資料筆數
        var counts = _dao.QueryCountsByName(name);
        page.TotalCounts = counts;
            
        if (counts == 0)
        {
            return new Page<Subject> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
        }
            
        // 獲取總頁數
        var pages = counts / Page<Subject>.PAGE_SIZE;
        if (counts % Page<Subject>.PAGE_SIZE != 0)
        {
            pages++;
        }
        page.PageTotal = pages;
            
        // PageNo Property有在set作保護
        page.PageNo = pageNo;

        // 查詢數據資料
        var data = _dao.QueryItemsByName(page.PageNo, name);

        page.Items = data;
        return page;
    }

    public Page<Subject> PageWithEntity(int pageNo)
    {
        var page = new Page<Subject>();
        // 獲取總資料筆數
        var counts = _dao.QueryTotalCounts();
        page.TotalCounts = counts;
        // 獲取總頁數
        var pages = counts / Page<Subject>.PAGE_SIZE;
        if (counts % Page<Subject>.PAGE_SIZE != 0)
        {
            pages++;
        }
        page.PageTotal = pages;
            
        page.PageNo = pageNo;

        // 查詢數據資料
        var data = _dao.Query4Items(page.PageNo);

        page.Items = data;
        return page;
    }

    public Subject SelectById(int id)
    {
        try
        {
            return _dao.SelectById(id);
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
            return _dao.Delete(id);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public int Update(Subject subject)
    {
        try
        {
            return _dao.Update(subject);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Page<SubjectModel> Page(int pageNo, string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return this.Page(pageNo);
        }
        var page = new Page<SubjectModel>();
        // 獲取總資料筆數
        var counts = _dao.QueryCountsByName(name);
        page.TotalCounts = counts;
            
        if (counts == 0)
        {
            return new Page<SubjectModel> { TotalCounts = 0, PageNo = 1, PageTotal = 1, Items = null };
        }
            
        // 獲取總頁數
        var pages = counts / Page<Subject>.PAGE_SIZE;
        if (counts % Page<Subject>.PAGE_SIZE != 0)
        {
            pages++;
        }
        page.PageTotal = pages;
            
        // PageNo Property有在set作保護
        page.PageNo = pageNo;

        // 查詢數據資料
        var data = _dao.QueryItemsByName(page.PageNo, name);
        
        var config = new MapperConfiguration(cfg => 
            cfg.CreateMap<Subject, SubjectModel>()); // 註冊Model間的對映
        var mapper = config.CreateMapper(); // 建立 Mapper
        var result = mapper.Map<IEnumerable<SubjectModel>>(data); // 轉換型別

        page.Items = result;
        return page;
    }
}