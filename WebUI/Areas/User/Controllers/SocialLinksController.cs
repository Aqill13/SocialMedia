using BusinessLayer.Abstract;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using static EntityLayer.Entities.UserSocialLink;

namespace WebUI.Areas.User.Controllers
{
    [Area("User")]
    public class SocialLinksController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserSocialLinkService _userSocialLinkService;

        public SocialLinksController(UserManager<AppUser> userManager, IUserSocialLinkService userSocialLinkService)
        {
            _userManager = userManager;
            _userSocialLinkService = userSocialLinkService;
        }

        // Add social link
        [HttpPost]
        public async Task<IActionResult> AddSocialLink(UserSocialLink.SocialPlatform platform, string url)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var existingLink = await _userSocialLinkService.GetFirstAsync(x => x.UserId == user.Id && x.Platform == platform);
            if (existingLink != null)
                return Json(new { success = false, message = "Social link for this platform already exists" });
            var platformEnum = (SocialPlatform)platform;
            var link = new UserSocialLink
            {
                UserId = user.Id,
                Platform = platformEnum,
                Url = url,
                IsVisible = true
            };
            await _userSocialLinkService.AddAsync(link);
            var iconClass = platform switch
            {
                UserSocialLink.SocialPlatform.Instagram => "fa-brands fa-instagram",
                UserSocialLink.SocialPlatform.Facebook => "fa-brands fa-facebook-f",
                UserSocialLink.SocialPlatform.Github => "fa-brands fa-github",
                UserSocialLink.SocialPlatform.Linkedin => "fa-brands fa-linkedin-in",
                UserSocialLink.SocialPlatform.Twitter => "fa-brands fa-x-twitter",
                UserSocialLink.SocialPlatform.Youtube => "fa-brands fa-youtube",
                _ => "ph ph-globe"
            };

            return Json(new
            {
                success = true,
                message = "Added successfully",
                id = link.Id,
                platform = platformEnum.ToString(),
                platformValue = (int)platformEnum,
                url = link.Url,
                iconClass = iconClass,
                isVisible = link.IsVisible
            });
        }
        // Update Social Link Visibility
        [HttpPost]
        public async Task<IActionResult> UpdateSocialLinkVisibility(int id, bool isVisible)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var link = await _userSocialLinkService.GetByIdAsync(id);
            if (link == null || link.UserId != user.Id)
                return Json(new { success = false, message = "Social link not found" });
            link.IsVisible = isVisible;
            await _userSocialLinkService.UpdateAsync(link);
            return Json(new { success = true, message = "Visibility updated successfully" });
        }
        // Delete Social Link
        [HttpPost]
        public async Task<IActionResult> DeleteSocialLink(int id)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var link = await _userSocialLinkService.GetByIdAsync(id);
            if (link == null || link.UserId != user.Id)
                return Json(new { success = false, message = "Social link not found" });
            var platformEnum = link.Platform;
            await _userSocialLinkService.DeleteAsync(link);
            return Json(new
            {
                success = true,
                message = "Social link deleted successfully",
                platformName = platformEnum.ToString(),
                platformValue = (int)platformEnum
            });
        }
        // Update Social Link
        [HttpPost]
        public async Task<IActionResult> UpdateSocialLink(int id, string url)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account", new { area = "User" });
            var link = await _userSocialLinkService.GetByIdAsync(id);
            if (link == null || link.UserId != user.Id)
                return Json(new { success = false, message = "Social link not found" });
            link.Url = url;
            await _userSocialLinkService.UpdateAsync(link);
            return Json(new { success = true, message = "Social link updated successfully", url = url });
        }
    }
}
