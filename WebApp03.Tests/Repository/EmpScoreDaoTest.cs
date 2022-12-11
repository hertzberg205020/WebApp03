using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp03.Models;
using WebApp03.Repository;
using WebApp03.Repository.Impl;

namespace WebApp03.Tests.Repository
{
    [TestClass]
    public class EmpScoreDaoTest
    {
        private IEmpScoreDao _dao; 
        
        [TestInitialize]
        public void TestInitialize()
        {
            _dao  = new EmpScoreDao(new ProxyDao<EmpScore, long>(
                new DbContextHolder(new ExamDbContext())
            ));
        }

        [TestMethod]
        public void UpdateTest()
        {
            Random rnd = new Random();
            
            for (int i = 2; i < 42; i++)
            {
                var data = new EmpScore
                {
                    Id = i,
                    Course1 = rnd.Next(30, 101),
                    Course2 = rnd.Next(30, 101),
                    Course3 = rnd.Next(30, 101),
                };
                _dao.Update(data);
            }
            
        }

        [TestMethod]
        public void SelectBuIdTest()
        {
            for (int i = 1; i < 42; i++)
            {
                var res = _dao.SelectById(i);
                Console.WriteLine(res);
            }
        }

        [TestMethod]
        public void Query4ItemsWithStatsTest()
        {
            var data = _dao.Query4ItemsWithStats(1);
            foreach (var item in data)
            {
                foreach (var prop in item)
                {
                    Console.WriteLine($"{prop.Key}: {prop.Value}");
                }
                Console.WriteLine("===");
            }
        }
    }
}