using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TasteTrailIdentityManager.Core.Users.Services;

namespace TasteTrailIdentityManager.Api.Common.Controllers;

[ApiController]
[Route("/api/[controller]/[action]")]
public class AdminPanelController : ControllerBase
{
    // private readonly IUserService _userService;

    // public AdminPanelController(IUserService userService)
    // {
    //     _userService = userService;
    // }

    // [Authorize(Roles = "Admin")]
    // [HttpGet]
    // public async Task<IActionResult> DashboardInfoAsync()
    // {
    //     var users = await _userService.GetAllAsync();
    //     int feedbacks = 0;

    //     // foreach (var user in users)
    //     // {
    //     //     if(user.Feedbacks.Count() == 0)
    //     //     {
    //     //         continue;
    //     //     }
    //     //     feedbacks += user.Feedbacks.Count();
    //     // }
    //     var venues = await this._venueService.GetAllAsync();

    //     var model = new AdminDashboardViewModel
    //     {
    //         TotalUsers = users.Count(),
    //         TotalFeedbacks = feedbacks,
    //         TotalVenues = venues.Count()
    //     };

    //     return View(model);
    // }
}
