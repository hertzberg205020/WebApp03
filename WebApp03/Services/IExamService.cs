using System.Collections.Generic;
using WebApp03.Models;

namespace WebApp03.Services;

public interface IExamService: ICoreService<Exam, int>
{
    Page<ExamModel> Page(int pageNo, int subjectId);
    IEnumerable<Emp> FindValidEmpList(int subjectId);

    int Insert(ExamModel examModel);

    int Update(ExamModel examModel);
    
    Page<IDictionary<string, object>> PageByEmpId(int pageNo, int empId);
}