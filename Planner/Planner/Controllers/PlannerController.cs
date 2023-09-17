using System.Globalization;
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
    private readonly IDateService _dateService;

    public PlannerController(IDailyTaskService dailyTaskService, IUserService userService, IDateService dateService)
    {
        _dailyTaskService = dailyTaskService;
        _userService = userService;
        _dateService = dateService;
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
    public async Task<IActionResult> CreateTask(string date)
    {
        var getDateResult = _dateService.GetDateTime(date);
        
        if (!getDateResult.IsSuccessful)
        {
            TempData["Error"] = getDateResult.Message;
            
            return RedirectToAction("Index");
        }
        
        var dailyRoutineTask = new DailyRoutineTaskViewModel
        {
            Date = getDateResult.Data
        };

        return View(dailyRoutineTask);
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
    
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var getResult = await _dailyTaskService.GetTaskById(id);
        
        if (!getResult.IsSuccessful)
        {
            TempData["Error"] = getResult.Message;
            
            return RedirectToAction("Index");
        }

        return View(getResult.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CompletedTask(int id)
    {
        var completeResult = await _dailyTaskService.CompleteDailyTask(id);
        
        if (!completeResult.IsSuccessful)
        {
            TempData["Error"] = completeResult.Message;
        }
        
        return RedirectToAction("Details", new {id = id});
    }
}