namespace Planner.Core.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<DailyRoutineTask>? DailyRoutineTasks { get; set; }
    public List<Reminder>? Reminders { get; set; }
}