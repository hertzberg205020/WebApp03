using System.Collections.Generic;
using WebApp03.Models;

namespace WebApp03.Services;

public interface IEmpScoreService: ICoreService<EmpScore, long>
{
    Page<IDictionary<string, object>> Page(int pageNo);
}