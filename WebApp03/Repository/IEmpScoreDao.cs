using System.Collections.Generic;
using WebApp03.Models;

namespace WebApp03.Repository;

public interface IEmpScoreDao: ICoreDao<EmpScore, long>
{
    EmpScore? FindById(long id);
    int MarkDeleted(long id);
    IEnumerable<IDictionary<string, object>> Query4ItemsWithStats(int pageNo);
}