using System;
using System.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApp03.Services;
using WebApp03.Services.Impl;

namespace WebApp03.Tests.Services
{
    [TestClass]
    public class EmpServiceTest
    {
        private IEmpService _service;

        [TestInitialize]
        public void TestInitialize()
        {
            // Console.WriteLine(ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString);
            // _service = new EmpService(ConfigurationManager.ConnectionStrings["ExamDb"].ConnectionString);
        }
        
        [TestMethod]
        public void PageTest()
        {
            var res = _service.Page(10000);
            Console.WriteLine(res);
            foreach (var item in res.Items)
            {
                Console.WriteLine(item);
            }
        }

        [TestMethod]
        public void PageByNameTest()
        {
            var res = _service.Page(1, "Yuri");
            Console.WriteLine(res);
        }
    }

}