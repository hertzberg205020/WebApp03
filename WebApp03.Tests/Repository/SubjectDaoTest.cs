using System;
using System.Configuration;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;

namespace WebApp03.Tests.Controllers
{
    [TestClass]
    public class SubjectDaoTest
    {
        private ISubjectDao _dao; 
        
        [TestInitialize]
        public void TestInitialize()
        {
            // _dao  = new SubjectDao();
            _dao  = new SubjectDao(new ProxyDao<Subject, int>(
                    new DbContextHolder(new ExamDbContext())
                ));
        }

        [TestMethod]
        public void AddTest()
        {
            var res = _dao.Insert(new Subject { Name = "偏微與複變2" });
            Console.WriteLine(res);
            // Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void UpdateTest()
        {
            var res = _dao.Update(new Subject { Id = 20, Name = "偏微與複變"});
            Assert.AreEqual(1, res);
        }

        [TestMethod]
        public void SelectByIdTest()
        {
            var res = _dao.SelectById(1);
            Console.WriteLine(res);
        }

        [TestMethod]
        public void Query4ItemsTest()
        {
            var res = _dao.Query4Items(3);
            foreach (var subject in res)
            {
                Console.WriteLine(subject);
            }
        }

        [TestMethod]
        public void QueryTotalCounts()
        {
            var res = _dao.QueryTotalCounts();
            Console.WriteLine(res);
        }

        [TestMethod]
        public void SelectByNameTest()
        {
            var res = _dao.SelectByName("製程優化");
            Console.WriteLine(res);
        }

        [TestMethod]
        public void QueryCountsByNameTest()
        {
            var res = _dao.QueryCountsByName("製程優化");
            Console.WriteLine(res);
        }

        [TestMethod]
        public void QueryItemsByName()
        {
            var res = _dao.QueryItemsByName(1, "製程優化");
            foreach (var subject in res)
            {
                Console.WriteLine(subject);
            }
        }
        
        [TestMethod]
        public void AddAndUpdateTest()
        {
            var res1 =_dao.Insert(new Subject
            {
                Name = "測試資料2"
            });
            Console.WriteLine(res1);
            var res2 = _dao.Update(new Subject { Id = 20, Name = "變更資料" });
            Console.WriteLine(res2);
        }
    }
}