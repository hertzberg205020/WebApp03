namespace WebApp03.Models;

public class SubjectModel
{
    public int Id { get; set; }

    public string Name { get; set; }
    
    public override string ToString()
    {
        return (this.ReportAllProperties());
    }
}