using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Planner.BLL.Interfaces;

namespace Planner.Controllers;

[Authorize]
public class PlannerController : Controller
{
    private readonly IDailyTaskService _dailyTaskService;

    public PlannerController(IDailyTaskService dailyTaskService)
    {
        _dailyTaskService = dailyTaskService;
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
}