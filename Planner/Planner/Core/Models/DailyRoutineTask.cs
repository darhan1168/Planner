using Planner.Core.Enums;

namespace Planner.Core.Models;

public class DailyRoutineTask : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsDone { get; set; }
    public DateTime Date { get; set; }
    public DailyTaskPriority Priority { get; set; }
    public virtual User? User { get; set; }
}