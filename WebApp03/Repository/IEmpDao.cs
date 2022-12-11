using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApp03.Models;

namespace WebApp03.Repository
{
    public interface IEmpDao: ICoreDao<Emp, Int32>
    {
        int QueryCountsByName(string name);
        IEnumerable<Emp> QueryItemsByName(int pageNo, string name);
        Emp SelectByEmpNo(string empNo);
    }
}