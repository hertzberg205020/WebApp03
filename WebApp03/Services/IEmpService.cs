using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp03.Models;

namespace WebApp03.Services
{
    public interface IEmpService: ICoreService<Emp, int>
    {
        Page<Emp> Page(int pageNo, string name);
        Page<Emp> Page(int pageNo);
    }
}
