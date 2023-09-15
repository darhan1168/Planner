using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.BLL.Interfaces;
using Planner.Core.Helpers;
using Planner.Core.Models;
using Planner.Models;

namespace Planner.Controllers;

[Authorize]
public class PlannerController : Controller
{
    private readonly IDailyTaskService _dailyTaskService;
    private readonly IUserService _userService;

    public PlannerController(IDailyTaskService dailyTaskService, IUserService userService)
    {
        _dailyTaskService = dailyTaskService;
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        if (User.Identity == null)
        {
            return RedirectToAction("Login", "Account");
        }
        
        var getTasksResult = await _dailyTaskService.GetAllDailyTaskByUsername(User.Identity.Name);

        if (!getTasksResult.IsSuccessful)
        {
            TempData["Error"] = getTasksResult.Message;
            
            return View();
        }
        
        return View(getTasksResult.Data);
    }

    [HttpGet]
    public async Task<IActionResult> CreateTask()
    {
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateTask(DailyRoutineTaskViewModel dailyRoutineTaskModel)
    {
        if (User.Identity == null)
        {
            return RedirectToAction("Login", "Account");
        }

        if (!ModelState.IsValid)
        {
            TempData["Error"] = $"Invalid {nameof(dailyRoutineTaskModel)}";
            
            return View(dailyRoutineTaskModel);
        }
        
        var getUserResult = await _userService.GetUserByUsername(User.Identity.Name);
        
        if (!getUserResult.IsSuccessful)
        {
            TempData["Error"] = getUserResult.Message;
            
            return View(dailyRoutineTaskModel);
        }

        var dailyRoutineTask = new DailyRoutineTask();
        dailyRoutineTaskModel.MapTo(dailyRoutineTask);
        dailyRoutineTask.User = getUserResult.Data;
        dailyRoutineTask.IsDone = false;
        
        var addResult = await _dailyTaskService.AddDailyTask(dailyRoutineTask);
        
        if (!addResult.IsSuccessful)
        {
            TempData["Error"] = addResult.Message;
            
            return View(dailyRoutineTaskModel);
        }
        
        return RedirectToAction("Index");
    }
}