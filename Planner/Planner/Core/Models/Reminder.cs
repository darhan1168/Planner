namespace Planner.Core.Models;

public class Reminder : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public DateOnly Date { get; set; }
}