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
    public class EmpDaoTest
    {
        private IEmpDao _dao; 
        
        [TestInitialize]
        public void TestInitialize()
        {
            _dao  = new EmpDao(new ProxyDao<Emp, int>(new DbContextHolder(new ExamDbContext())));
        }   
        
        [TestMethod]
        public void UpdateTest()
        {
            _dao.Update(new Emp
            {
                Id = 17,
                Name = "Alex",
                EmpNo = "N000000099"
            });
        }

        [TestMethod]
        public void Query4ItemsTest()
        {
            var res = _dao.Query4Items(1);
            foreach (var emp in res)
            {
                Console.WriteLine(emp);
            }
        }

        [TestMethod]
        public void QueryCountsByNameTest()
        {
            var res = _dao.QueryCountsByName("Yuri");
            Assert.AreEqual(2, res);
        }

        [TestMethod]
        public void QueryItemsByNameTest()
        {
            var res = _dao.QueryItemsByName(1, "Yuri");
            foreach (var emp in res)
            {
                Console.WriteLine(emp);
            }
        }

        [TestMethod]
        public void SelectByEmpNo()
        {
            // var res = _dao.SelectByEmpNo("N011821913");
            var res = _dao.SelectByEmpNo("N008504706");
            Console.WriteLine(res);
        }
    }
}