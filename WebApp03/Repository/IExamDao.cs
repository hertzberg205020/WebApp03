using System.Collections.Generic;
using System.Linq;
using WebApp03.Models;

namespace WebApp03.Repository
{
    public interface IExamDao: ICoreDao<Exam, int>
    {
        Exam? SelectByEmpIdAndSubjectId(int empId, int subjectId);
        IQueryable<ExamModel> FindByEmpId(int empId);
        IQueryable<ExamModel> FindBySubjectId(int subjectId);
        IQueryable<ExamModel> FindBySubjectIdAndEmpIdList(int subjectId ,List<int> empIds);
        
        IEnumerable<ExamModel> QueryItemsBySubjectId(int pageNo, int subjectId);
        
        /// <summary>
        /// 找出特定課目下尚未有成績的學生
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        IEnumerable<Emp> FindValidEmpList(int subjectId);
        
        /// <summary>
        /// 依科目編號查詢有成績的學生數量
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        int QueryCountsBySubjectId(int subjectId);

        IEnumerable<IDictionary<string, object>> QueryItemsByEmpId(int pageNo, int empId);
        int QueryCountsByEmpId(int empId);
    }
}