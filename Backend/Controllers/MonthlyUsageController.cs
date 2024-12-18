using Backend.Services;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[Controller]
[Route("api/[controller]")]
public class MonthlyUsageController : ControllerBase
{
    private readonly IMonthlyUsageService _monthlyUsageService;

    public MonthlyUsageController(IMonthlyUsageService monthlyUsageService)
    {
        _monthlyUsageService = monthlyUsageService;
    }
    
    [Route("{userUsage:int}")]
    [HttpPost]
    [Authorize(Roles = "Admin, User")]
    public async Task<IActionResult> UpdateMonthlyUsage(int userUsage)
    {
        try
        {
            return Ok(new
                { message = "Monthly usage updated", monthlyUsage =  await _monthlyUsageService.UpdateMonthlyUsage(userUsage) });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in updating monthly usage", error = e.Message });
        }
    }
    
    [Route("")]
    [HttpGet]
    public async Task<IActionResult> GetMonthlyUsage()
    {
        try
        {
            return Ok(new
                { message = "Monthly usage retrieved", monthlyUsage =  await _monthlyUsageService.GetMonthlyUsage() });
        }
        catch (Exception e)
        {
            return BadRequest(new { message = "Failed in retrieving monthly usage", error = e.Message });
        }
    }
}