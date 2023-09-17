using Planner.BLL.Services;
using Planner.Core.Models;

namespace Planner.BLL.Interfaces;

public interface IDailyTaskService
{
    Task<Result<bool>> AddDailyTask(DailyRoutineTask dailyTask);
    Task<Result<bool>> DeleteDailyTask(int dailyTaskId);
    Task<Result<bool>> UpdateDailyTask(DailyRoutineTask newDailyTask);
    Task<Result<bool>> CompleteDailyTask(int id);
    Task<Result<DailyRoutineTask>> GetTaskById(int id);
    Task<Result<List<DailyRoutineTask>>> GetAllDailyTaskByUsername(string username);
}