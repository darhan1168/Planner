namespace Planner.Core.Models;

public class Reminder : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
}