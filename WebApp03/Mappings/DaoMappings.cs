using AutoMapper;
using WebApp03.Models;

namespace WebApp03.Mappings;

public class DaoMappings: Profile
{
    public DaoMappings()
    {
        CreateMap<Exam, ExamModel>()
            .ForMember(x => x.Id, y => y.MapFrom(e => e.Id))
            .ForMember(x => x.Score, y => y.MapFrom(e => e.Score))
            .ForMember(x => x.EmpId, y => y.MapFrom(e => e.EmpId))
            .ForMember(x => x.EmpName, y => y.MapFrom(e => e.Emp.Name))
            .ForMember(x => x.EmpNo, y => y.MapFrom(e => e.Emp.EmpNo))
            .ForMember(x => x.SubjectId, y => y.MapFrom(e => e.SubjectId))
            .ForMember(x => x.SubjectName, y => y.MapFrom(e => e.Subject.Name))
            .ReverseMap(); 

        // ...其他的對映內容 (使用 CreateMap<> 建立下一組)
    }
}