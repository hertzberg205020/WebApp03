namespace WebApp03.Models;

public class EmpScore: BaseEntity<long>
{
    public string Name { get; set; }
    public bool IsDeleted { get; set; }
    public int? Course1 { get; set; }
    public int? Course2 { get; set; }
    public int? Course3 { get; set; }
}