using API.Extensions;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize]
    public class NotificationsController : BaseApiController
    {
        private readonly INotificationService _notificationService;
        private readonly UserManager<AppUser> _userManager;
        public NotificationsController(INotificationService notificationService, UserManager<AppUser> userManager)
        {
            _notificationService = notificationService;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetMy(int take = 50)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

            if (user == null) return Unauthorized(new { message = "User not found" });

            var result = await _notificationService.GetMyNotificationsAsync(user.Id, take);
            
            return Ok(result);
        }

        [HttpPost("{notificationId}/read")]
        public async Task<ActionResult> MarkAsRead(Guid notificationId)
        {
            var userIdString = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userIdString)) return Unauthorized();
            if (!int.TryParse(userIdString, out var userId)) return Unauthorized();

            await _notificationService.MarkAsReadAsync(notificationId, userId);
            return NoContent();
        }
    }
}


