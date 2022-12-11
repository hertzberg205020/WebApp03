using System.Collections.Generic;
using System.Linq;
using WebApp03.Models;

namespace WebApp03.Repository
{
    public interface ISubjectDao: ICoreDao<Subject, int>
    {
        Subject SelectByName(string name);
        int QueryCountsByName(string name);
        IEnumerable<Subject> QueryItemsByName(int pageNo, string name);
    }
}