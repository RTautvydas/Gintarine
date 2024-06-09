namespace Gintarine.Repositories.Entities;

public class Log : EntityBase
{
    public string Action { get; set; }
    
    public string NewValue { get; set; }
    
    public string OldValue { get; set; }
    
    public string PropertyName { get; set; }
    
    public DateTime Date { get; set; }
}