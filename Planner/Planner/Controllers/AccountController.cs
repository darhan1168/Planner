using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Planner.BLL.Interfaces;
using Planner.Core.Helpers;
using Planner.Core.Models;
using Planner.Models;

namespace Planner.Controllers;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerModel)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = $"Invalid {nameof(registerModel)}";
            
            return View(registerModel);
        }

        var user = new User();
        registerModel.MapTo(user);
        
        var registerResult = await _userService.Register(user);

        if (!registerResult.IsSuccessful)
        {
            TempData["Error"] = registerResult.Message;
            
            return View(registerModel);
        }
        
        await Authenticate(registerModel.Username);
        
        return RedirectToAction("Privacy", "Home");
    }
    
    private async Task Authenticate(string userName)
    {
        var claims = new List<Claim>
        {
            new (ClaimsIdentity.DefaultNameClaimType, userName)
        };

        var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }
}