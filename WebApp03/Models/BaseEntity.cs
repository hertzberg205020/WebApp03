namespace WebApp03.Models
{
    public abstract class BaseEntity<I>
    {
        public I Id { get; set; }
        
        public override string ToString()
        {
            return (this.ReportAllProperties());
        }
    }
}