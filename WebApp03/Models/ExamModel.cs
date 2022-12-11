namespace WebApp03.Models;

public class ExamModel
{
    public int Id { get; set; }
    public int Score { get; set; }
    public int EmpId { get; set; }
    public string EmpName { get; set; }
    public string EmpNo { get; set; }
    public int SubjectId { get; set; }
    public string SubjectName { get; set; }

    // public int RankNo { get; set; }
    
    public override string ToString()
    {
        return (this.ReportAllProperties());
    }
}