using Microsoft.EntityFrameworkCore;
using Planner.Core.Models;

namespace Planner.DAL;

public class AppContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<DailyRoutineTask> DailyRoutineTasks { get; set; }
    public DbSet<Reminder> Reminders { get; set; }

    public AppContext(DbContextOptions<AppContext> options) : base(options)
    {
    }
}