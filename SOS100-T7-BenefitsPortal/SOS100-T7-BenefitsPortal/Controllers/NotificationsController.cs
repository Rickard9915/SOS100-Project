using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SOS100_T7_BenefitsPortal.Services;

namespace SOS100_T7_BenefitsPortal.Controllers;

[Authorize]
public class NotificationsController : Controller
{
    private readonly NotificationApiService _notificationService;

    public NotificationsController(NotificationApiService notificationService)
    {
        _notificationService = notificationService;
    }

    public async Task<IActionResult> Index()
    {
        var userIdClaim = User.FindFirst("UserId")?.Value;

        if (userIdClaim == null || !int.TryParse(userIdClaim, out var userId))
            return RedirectToAction("Login", "Account");

        var notifications = await _notificationService.GetNotificationsAsync(userId);
        return View(notifications);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        await _notificationService.MarkAsReadAsync(id);
        return RedirectToAction("Index");
    }
}
