using Planner.BLL.Interfaces;
using Planner.Core.Models;
using Planner.DAL;

namespace Planner.BLL.Services;

public class DailyTaskService : IDailyTaskService
{
    private readonly IRepository<DailyRoutineTask> _repository;
    private readonly IUserService _userService;

    public DailyTaskService(IRepository<DailyRoutineTask> repository, IUserService userService)
    {
        _repository = repository;
        _userService = userService;
    }
    
    public async Task<Result<bool>> AddDailyTask(DailyRoutineTask dailyTask)
    {
        if (dailyTask == null)
        {
            return new Result<bool>(false, $"{nameof(dailyTask)} not found");
        }

        var addResult = await _repository.AddAsync(dailyTask);

        if (!addResult.IsSuccessful)
        {
            return new Result<bool>(false, $"Failed to add daily task with name: {dailyTask.Name}. " +
                                           $"Error: {addResult.Message}");
        }
        
        return new Result<bool>(true);
    }

    public async Task<Result<bool>> DeleteDailyTask(int dailyTaskId)
    {
        var getDailyTaskResult = await _repository.GetByIdAsync(dailyTaskId);
        
        if (!getDailyTaskResult.IsSuccessful)
        {
            return new Result<bool>(false, $"Daily task by id \"{dailyTaskId}\" not found");
        }

        var deleteResult = await _repository.DeleteAsync(getDailyTaskResult.Data);

        if (!deleteResult.IsSuccessful)
        {
            return new Result<bool>(false, $"Failed to delete daily task with name: {getDailyTaskResult.Data.Name}. " +
                                           $"Error: {deleteResult.Message}");
        }
        
        return new Result<bool>(true);
    }

    public async Task<Result<bool>> UpdateDailyTask(DailyRoutineTask newDailyTask)
    {
        if (newDailyTask == null)
        {
            return new Result<bool>(false, $"{nameof(newDailyTask)} not found");
        }

        var updateResult = await _repository.UpdateAsync(newDailyTask);

        if (!updateResult.IsSuccessful)
        {
            return new Result<bool>(false, $"Failed to update daily task with name: {newDailyTask.Name}. " +
                                           $"Error: {updateResult.Message}");
        }
        
        return new Result<bool>(true);
    }


    public async Task<Result<DailyRoutineTask>> GetTaskById(int id)
    {
        var getResult = await _repository.GetByIdAsync(id);

        if (!getResult.IsSuccessful)
        {
            return new Result<DailyRoutineTask>(false, getResult.Message);
        }
        
        return new Result<DailyRoutineTask>(true, getResult.Data);
    }

    public async Task<Result<List<DailyRoutineTask>>> GetAllDailyTaskByUsername(string username)
    {
        var getUserResult = await _userService.GetUserByUsername(username);
        
        if (!getUserResult.IsSuccessful)
        {
            return new Result<List<DailyRoutineTask>>(false, getUserResult.Message);
        }
        
        var getDailyTasksResult = await _repository.GetAsync(dt => dt.User == getUserResult.Data);
        
        if (!getDailyTasksResult.IsSuccessful)
        {
            return new Result<List<DailyRoutineTask>>(false, $"Failed to get all daily tasks for user with username: {getUserResult.Data.Username}. " +
                                                             $"Error: {getDailyTasksResult.Message}");
        }
        
        if (getDailyTasksResult.Data.Count == 0)
        {
            return new Result<List<DailyRoutineTask>>(false, $"Daily tasks hasn't added yet");
        }
        
        return new Result<List<DailyRoutineTask>>(true, getDailyTasksResult.Data);
    }
}