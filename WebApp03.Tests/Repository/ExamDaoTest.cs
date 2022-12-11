using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;

namespace WebApp03.Tests.Repository
{
    [TestClass]
    public class ExamDaoTest
    {
        private IExamDao _dao; 
        
        [TestInitialize]
        public void TestInitialize()
        {
            // _dao  = new SubjectDao();
            _dao  = new ExamDao(new ProxyDao<Exam, int>(
                new DbContextHolder(new ExamDbContext())
            ));
        }

        [TestMethod]
        public void FindBySubjectIdAndEmpIdListTest()
        {
            var list = new List<int>() { 39, 40, 41, 42 };
            var res = _dao.FindBySubjectIdAndEmpIdList(1, list);
            foreach (var item in res)
            {
                Console.WriteLine(item);
                Console.WriteLine("====");
            }
        }

        [TestMethod]
        public void FindBySubjectIdTest()
        {
            var res = _dao.FindBySubjectId(1);
            foreach (var item in res)
            {
                Console.WriteLine(item);
                Console.WriteLine("===");
            }
        }

        [TestMethod]
        public void FindByEmpIdTest()
        {
            var data = _dao.FindByEmpId(38);
            foreach (var item in data)
            {
                Console.WriteLine(item);
                Console.WriteLine("===");
            }
        }

        [TestMethod]
        public void SelectCountsBySubjectIdTest()
        {
            var data = _dao.QueryCountsBySubjectId(1);
            Assert.AreEqual(18, data);
        }

        [TestMethod]
        public void FindValidEmpListTest()
        {
            var data = _dao.FindValidEmpList(1);
            foreach (var emp in data)
            {
                Console.WriteLine(emp.Name);
            }
        }

        [TestMethod]
        public void QueryItemsByEmpIdTest()
        {
            var res = _dao.QueryItemsByEmpId(1, 38);
            foreach (var item in res)
            {
                foreach (var member in item)
                {
                    Console.WriteLine($"{member.Key}: {member.Value}");
                }

                Console.WriteLine("===");
            }
        }
    }
}