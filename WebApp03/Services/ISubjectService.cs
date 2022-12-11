using WebApp03.Models;

namespace WebApp03.Services;

public interface ISubjectService: ICoreService<Subject, int>
{
    Page<SubjectModel> Page(int pageNo, string name);
    Page<SubjectModel> Page(int pageNo);
    Page<Subject> PageWithEntity(int pageNo, string name);
    Page<Subject> PageWithEntity(int pageNo);
}