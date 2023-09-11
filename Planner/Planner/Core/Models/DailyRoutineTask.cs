namespace Planner.Core.Models;

public class DailyRoutineTask : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; }
    public DateOnly Date { get; set; }
}