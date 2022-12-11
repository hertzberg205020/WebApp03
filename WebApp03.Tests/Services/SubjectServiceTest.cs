using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;
using WebApp03.Services;
using WebApp03.Services.Impl;

namespace WebApp03.Tests.Services
{
    [TestClass]
    public class SubjectServiceTest
    {
        private ISubjectService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            // Console.WriteLine(ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString);
            // _service = new SubjectService(new SubjectDao());
            var dbCtxHolder = new DbContextHolder(new ExamDbContext());
            var dao = new SubjectDao(new ProxyDao<Subject, int>(dbCtxHolder));
            var proxy = new ProxyService<Subject, int>(dbCtxHolder);
            _service = new SubjectService(dao, proxy);
        }

        [TestMethod]
        public void PageTest()
        {
            var res = _service.Page(2);
            Console.WriteLine(res);
        }

        [TestMethod]
        public void PageWithEntity()
        {
            var res = _service.PageWithEntity(1);
        }
    }
}